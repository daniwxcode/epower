using BlazorHero.CleanArchitecture.Application.Exceptions;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands
{
    public class MakeAPaymentCommand : IRequest<Result<BuyCreditResponse>>
    {
        public string Orign { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal DueValue
        {
            get
            {
                return (int)Amount switch
                {
                    <= 0 => 0,
                    > 1_000_000 => 1000,
                    > 300_000 => 700,
                    > 50_000 => 500,
                    > 5_000 => 300,
                    _ => 200
                };
            }
        }



        public int MeterId { get; set; } = 0;
        public string SerialNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid Reference { get; init; } = Guid.NewGuid();

    }

    internal class MakeAPaymentCommandHandler : IRequestHandler<MakeAPaymentCommand, Result<BuyCreditResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICeetService _ceetService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
        private readonly IPdfService _pdfService;
        public MakeAPaymentCommandHandler(IUnitOfWork<int> unitOfWork, ICeetService ceetService, ICurrentUserService currentUserService, IUserService userService, IPdfService pdfService)
        {
            _unitOfWork = unitOfWork;
            _ceetService = ceetService;
            _currentUserService = currentUserService;
            _userService = userService;
            _pdfService = pdfService;
        }
        public async Task<Result<BuyCreditResponse>> Handle(MakeAPaymentCommand request, CancellationToken cancellationToken)
        {
            if (request.MeterId == 0 && request.SerialNumber == string.Empty)
                return await Result<BuyCreditResponse>.FailAsync("Veuillez renseigner un numéro de compteur ou sélectionner une boutique.");
            var db = _unitOfWork.Repository<Payment>();
            var itemdb = db.Entities.FirstOrDefault(_ => _.InternalReference == request.Reference.ToString());
            if (itemdb != null)
                return await Result<BuyCreditResponse>.FailAsync("Cette vente a déjà été effectuée (référence dupliquée).");

            // Rattacher la vente à la caisse active du vendeur (si existante)
            var activeShift = await _unitOfWork.Repository<CashShift>().Entities
                .FirstOrDefaultAsync(cs => cs.Seller.UserId == _currentUserService.UserId && cs.Status == CashShiftStatus.Open, cancellationToken);

            var user = await _userService.GetAsync(_currentUserService.UserId);
            Meter dbMeter = null;
            if (request.SerialNumber != string.Empty && request.MeterId == 0)
                dbMeter = await _unitOfWork.Repository<Meter>().Entities.FirstOrDefaultAsync(_ => _.SerialNumber == request.SerialNumber);

            if (dbMeter != null)
                request.MeterId = dbMeter.Id;

            if (request.MeterId > 0)
            {
                if (dbMeter == null)
                    dbMeter = await _unitOfWork.Repository<Meter>().GetByIdAsync(request.MeterId);
                if (dbMeter == null)
                    return await Result<BuyCreditResponse>.FailAsync("Compteur introuvable en base de données. Vérifiez l'identifiant.");

                CreditVendResponse ceetvente;
                try
                {
                    ceetvente = await _ceetService.BuyCredit(new CreditRequest(dbMeter.SerialNumber, (int)request.Amount, request.PhoneNumber));
                }
                catch (ApiException ex)
                {
                    return await Result<BuyCreditResponse>.FailAsync(ex.Message);
                }

                if (ceetvente == null)
                    return await Result<BuyCreditResponse>.FailAsync("Le service de vente n'a pas retourné de résultat. Veuillez réessayer.");

                InternalPayement internalPayement = new InternalPayement()
                {
                    Amount = request.Amount,
                    SerialNumber = dbMeter.SerialNumber,
                    InternalReference = request.Reference.ToString(),
                    Id = 0,
                    Credits = ceetvente.credit,
                    ExternalReference = ceetvente.bill,
                    MeterId = dbMeter.Id,
                    CreditCode = ceetvente.Code,
                    BilledAmount = request.DueValue,
                    CashShiftId = activeShift?.Id,
                };
                await db.AddAsync(internalPayement);
                await _unitOfWork.Commit(cancellationToken);

                try
                {
                    await _ceetService.Confirm(ceetvente);
                }
                catch (ApiException)
                {
                    // Confirmation failed but payment was already recorded — continue
                }

                internalPayement.IsConfirmed = true;
                internalPayement.ConfirmationDate = DateTime.UtcNow;
                await db.UpdateAsync(internalPayement);
                await _unitOfWork.Commit(cancellationToken);
                var internalSale = new BuyCreditResponse(internalPayement.Id, (int)request.Amount, request.DueValue, internalPayement.SerialNumber, internalPayement.ExternalReference, DateTime.UtcNow, internalPayement.InternalReference, ceetvente.Code, ceetvente.credit, user.Data.UserFullName);
                await _pdfService.GenerateReceiptAsync(internalSale, ApplicationConstants.FileConstants.GetReceipt(internalPayement.Id));

                return Result<BuyCreditResponse>.Success(internalSale, "Vente Effectuée avec Succès!");
            }

            CreditVendResponse venteExt;
            try
            {
                venteExt = await _ceetService.BuyCredit(new CreditRequest(request.SerialNumber, (int)request.Amount, request.PhoneNumber));
            }
            catch (ApiException ex)
            {
                return await Result<BuyCreditResponse>.FailAsync(ex.Message);
            }

            if (venteExt == null)
                return await Result<BuyCreditResponse>.FailAsync("Le service de vente n'a pas retourné de résultat. Veuillez réessayer.");

            Payment payment = new Payment()
            {
                Id = 0,
                Amount = request.Amount,
                SerialNumber = request.SerialNumber,
                Credits = (double)venteExt.credit,
                InternalReference = request.Reference.ToString(),
                ExternalReference = venteExt.bill,
                CreditCode = venteExt.Code,
                BilledAmount = request.DueValue,
                CashShiftId = activeShift?.Id,
            };
            await db.AddAsync(payment);
            await _unitOfWork.Commit(cancellationToken);

            try
            {
                await _ceetService.Confirm(venteExt);
            }
            catch (ApiException)
            {
                // Confirmation failed but payment was already recorded — continue
            }

            payment.ConfirmationDate = DateTime.UtcNow;
            payment.IsConfirmed = true;
            await db.UpdateAsync(payment);
            await _unitOfWork.Commit(cancellationToken);
            var externalSale = new BuyCreditResponse(payment.Id, (int)request.Amount, request.DueValue, request.SerialNumber, payment.InternalReference, DateTime.UtcNow, payment.InternalReference, venteExt.Code, venteExt.credit, user.Data.UserFullName);
            await _pdfService.GenerateReceiptAsync(externalSale, ApplicationConstants.FileConstants.GetReceipt(payment.Id));
            return await Result<BuyCreditResponse>.SuccessAsync(externalSale, "Vente éffectuée avec succès");

        }
    }
}
