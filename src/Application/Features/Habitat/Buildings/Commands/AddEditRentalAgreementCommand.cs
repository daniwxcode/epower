using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using LazyCache;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands
{
    public class AddEditRentalAgreementCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int ShopTenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double StartDateCounterValue { get; set; } = 0.0;
        public double? EndDateCounterValue { get; set; }

    }
    internal class AddEditRentalAgreementCommandHandler : IRequestHandler<AddEditRentalAgreementCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAppCache _appCache;
        public AddEditRentalAgreementCommandHandler(IUnitOfWork<int> unitOfWork, IAppCache appCache)
        {
            _unitOfWork = unitOfWork;
            _appCache = appCache;
        }
        public async Task<Result<int>> Handle(AddEditRentalAgreementCommand request, CancellationToken cancellationToken)
        {
            var db = _unitOfWork.Repository<RentalAgreement>();
            var dbitem = await db.Entities.FirstOrDefaultAsync(_ => _.Id != request.Id && _.ShopId == request.ShopId && (_.EndDate == null || (_.EndDate != null && _.EndDate > request.StartDate)));
            if (dbitem is not null)
            {
                if (dbitem.ShopTenantId == request.ShopTenantId)
                    return await Result<int>.FailAsync("Cette Location Existe déjà");
                if (dbitem.EndDate != null)
                    return await Result<int>.FailAsync("Une Location couvre déjà cette période");
                return await Result<int>.FailAsync("Un Contrat de location est déjà en cours pour cette boutique");
            }

            if (request.Id == 0)
            {
                RentalAgreement item = new RentalAgreement
                {
                    Id = request.Id,
                    ShopId = request.ShopId,
                    EndDate = null,
                    StartDate = request.StartDate,
                    StartDateCounterValue = request.StartDateCounterValue,
                    EndDateCounterValue = null,
                };
                await db.AddAsync(item);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id, "Contrat de Location enregistré avec succès");
            }
            var existingItem = await db.GetByIdAsync(request.Id);
            if (existingItem is null)
            {
                return await Result<int>.FailAsync("Element Inexistant");
            }
            dbitem.StartDate = request.StartDate;
            dbitem.EndDate = request.EndDate;
            dbitem.StartDateCounterValue = request.StartDateCounterValue;
            dbitem.EndDateCounterValue = request.EndDateCounterValue;
            dbitem.ShopId = request.ShopId;
            await db.UpdateAsync(dbitem);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync("Contrat Mise à jour avec succès");

        }
    }
}
