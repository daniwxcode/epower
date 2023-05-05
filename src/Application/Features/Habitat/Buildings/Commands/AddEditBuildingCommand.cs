using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

using Microsoft.Extensions.Localization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static BlazorHero.CleanArchitecture.Shared.Constants.Permission.Permissions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands
{
    public class AddEditBuildingCommand: IRequest<Result<int>>
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Address { get; set; }
        public Building CreateBuilding()=> new Building{ Name = Name, Address = Address };

    }
    internal class AddEditBuildingCommandHandler : IRequestHandler<AddEditBuildingCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditBuildingCommandHandler> _localizer;
        public AddEditBuildingCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<AddEditBuildingCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }
        public async Task<Result<int>> Handle(AddEditBuildingCommand request, CancellationToken cancellationToken)
        {
            if(request.Id == 0)
            {
                var building = request.CreateBuilding();
                await _unitOfWork.Repository<Building>().AddAsync(building);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.BuildingsCache.GetAll);
                return await Result<int>.SuccessAsync(building.Id, _localizer["Building Saved"]);
            }
            else
            {
                var building = await _unitOfWork.Repository<Building>().GetByIdAsync(request.Id);
                if(building == null)
                    return await Result<int>.FailAsync(_localizer["Building Not Found!"]);
                building.Name = request.Name ?? building.Name;
                building.Address = request.Address??building.Address;
                await _unitOfWork.Repository<Building>().UpdateAsync(building);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.AllBuildingCacheKey);
                return await Result<int>.SuccessAsync(building.Id, _localizer["Building Updated"]);
            }
        }
    }
}
