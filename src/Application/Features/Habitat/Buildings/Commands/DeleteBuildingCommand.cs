using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using LazyCache;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands
{
    public class DeleteBuildingCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DeleteBuildingCommand(int id)
        {
            Id = id;
        }
    }
    internal class DeleteBuildingCommandHandler : IRequestHandler<DeleteBuildingCommand, Result<int>>
    {
        private readonly IAppCache _appCache;
        private readonly IUnitOfWork<int> _unitOfWork;
        public DeleteBuildingCommandHandler(IAppCache appCache, IUnitOfWork<int> unitOfWork)
        {
            _appCache = appCache;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
            var db = _unitOfWork.Repository<Building>();
            var building = await db.GetByIdAsync(request.Id);
            if (building == null)
            {
                return await Result<int>.FailAsync("Immeuble inexistant en Base");
            }
            await _unitOfWork.Repository<Building>().DeleteAsync(building);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.BuildingsCache.AllBuildingCacheKey);
            return await Result<int>.SuccessAsync(request.Id, "Immeuble Supprimé");
        }
    }

}
