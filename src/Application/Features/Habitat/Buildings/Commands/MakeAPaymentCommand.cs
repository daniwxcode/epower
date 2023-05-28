using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands
{
    public class MakeAPaymentCommand : IRequest<Result<BuyCreditResponse>>
    {
        public decimal Amount { get; set; }
        public int MeterId { get; set; } = 0;
        public string SerialNumber { get; set; } = string.Empty;
        public Guid Reference { get; init; } = Guid.NewGuid();

    }
    internal class MakeAPaymentCommandHandler : IRequestHandler<MakeAPaymentCommand, Result<BuyCreditResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICeetService _ceetService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
         public MakeAPaymentCommandHandler(IUnitOfWork<int> unitOfWork, ICeetService ceetService, ICurrentUserService currentUserService,IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _ceetService = ceetService;
            _currentUserService = currentUserService;
            _userService = userService;

        }
        public async Task<Result<BuyCreditResponse>> Handle(MakeAPaymentCommand request, CancellationToken cancellationToken)
        {
            if (request.MeterId == 0 && request.SerialNumber == string.Empty)
                return await Result<BuyCreditResponse>.FailAsync("Impossible d'effectuer cette vente");
            var db = _unitOfWork.Repository<Payment>();
            var itemdb = db.Entities.FirstOrDefault(_ => _.InternalReference == request.Reference.ToString());
            if (itemdb != null)
                return await Result<BuyCreditResponse>.FailAsync("Cette Vente a déjà été effectuée");
            var User = await _userService.GetAsync(_currentUserService.UserId);
            var userName = $"{User.Data.LastName.ToUpper()} {User.Data.FirstName}";
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
                    return await Result<BuyCreditResponse>.FailAsync("Impossible d'effectuer cette vente, Compteur Id Erronée");
                var ceetvente = await _ceetService.BuyCredit(new CreditRequest(dbMeter.SerialNumber, (int)request.Amount));
                InternalPayement internalPayement = new InternalPayement()
                {
                    Amount = request.Amount,
                    SerialNumber = dbMeter.SerialNumber,
                    InternalReference = request.Reference.ToString(),
                    Id = 0,
                    Credits = (double)ceetvente.credit,
                    ExternalReference = ceetvente.reference,
                    MeterId = dbMeter.Id
                };
                await db.AddAsync(internalPayement);
                await _unitOfWork.Commit(cancellationToken);
                return Result<BuyCreditResponse>.Success(new BuyCreditResponse(internalPayement.Id, (int)request.Amount, internalPayement.SerialNumber, internalPayement.InternalReference, DateTime.UtcNow, internalPayement.InternalReference, ceetvente.code, ceetvente.credit,userName ), "Vente Effectuée avec Succès!");
            }
            var venteExt = await _ceetService.BuyCredit(new CreditRequest(request.SerialNumber, (int)request.Amount));
            Payment payment = new Payment()
            {
                Id = 0,
                Amount = request.Amount,
                SerialNumber = request.SerialNumber,
                Credits = (double)venteExt.credit,
                InternalReference = request.Reference.ToString(),
                ExternalReference = venteExt.reference,
                
            };
            await db.AddAsync(payment);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<BuyCreditResponse>.SuccessAsync(new BuyCreditResponse(payment.Id, (int)request.Amount, payment.SerialNumber, payment.InternalReference, DateTime.UtcNow, payment.InternalReference, venteExt.code, venteExt.credit, userName), "Vente Effectué avec succès");



        }
    }
}
