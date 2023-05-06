using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using LazyCache;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries
{
    public class GetAllBuildingsRequest : IRequest<Result<List<BuildingResponseBase>>>
    {
    }
    internal class GetAllBuildingsRequestHandler : IRequestHandler<GetAllBuildingsRequest, Result<List<BuildingResponseBase>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAppCache _cache;
        public GetAllBuildingsRequestHandler(IAppCache Cache, IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cache = Cache;
        }
        public async Task<Result<List<BuildingResponseBase>>> Handle(GetAllBuildingsRequest request, CancellationToken cancellationToken)
        {
            Func<Task<List<Building>>> getAll = () => _unitOfWork.Repository<Building>().GetAllAsync();
            var dataList = await _cache.GetOrAddAsync(ApplicationConstants.BuildingsCache.AllBuildingCacheKey, getAll);
            var mappedData = dataList.Select(_ => _.GetBuildingsResponse()).ToList();
            return await Result<List<BuildingResponseBase>>.SuccessAsync(mappedData);
        }
    }
}
