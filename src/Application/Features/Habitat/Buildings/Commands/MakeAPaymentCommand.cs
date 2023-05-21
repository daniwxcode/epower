using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

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
        public Guid Reference { get; init; } = new Guid();
    }
    internal class MakeAPaymentCommandHandler : IRequestHandler<MakeAPaymentCommand, Result<BuyCreditResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICeetService _ceetService;
        public MakeAPaymentCommandHandler(IUnitOfWork<int> unitOfWork, ICeetService ceetService)
        {
            _unitOfWork = unitOfWork;
            _ceetService = ceetService;
        }
        public async Task<Result<BuyCreditResponse>> Handle(MakeAPaymentCommand request, CancellationToken cancellationToken)
        {
            if (request.MeterId == 0 && request.SerialNumber == string.Empty)
                return await Result<BuyCreditResponse>.FailAsync("Impossible d'effectuer cette vente");
            var db = _unitOfWork.Repository<Payment>();
            var itemdb = db.Entities.FirstOrDefault(_ => _.InternalReference == request.Reference.ToString());
            if (itemdb != null)
                return await Result<BuyCreditResponse>.FailAsync("Cette Vente a déjà été effectuée");
            Meter dbMeter = null;
            if (request.MeterId > 0)
            {
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
                return Result<BuyCreditResponse>.Success(new BuyCreditResponse(internalPayement.Id, (int)request.Amount, internalPayement.SerialNumber, internalPayement.InternalReference, DateTime.UtcNow, internalPayement.InternalReference, ceetvente.code, ceetvente.credit));
            }

            return await Result<BuyCreditResponse>.FailAsync("Vente des Compteurs Externes non actif");


        }
    }
}
