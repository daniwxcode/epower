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

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Meters.Queries
{
    public class GetAllMetersRequest : IRequest<Result<List<MeterResponseBase>>>
    {
    }
    internal class GetAllMetersRequestHandler : IRequestHandler<GetAllMetersRequest, Result<List<MeterResponseBase>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAppCache _cache;
        public GetAllMetersRequestHandler(IAppCache Cache, IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cache = Cache;
        }
        public async Task<Result<List<MeterResponseBase>>> Handle(GetAllMetersRequest request, CancellationToken cancellationToken)
        {
            Func<Task<List<Meter>>> getAll = () => _unitOfWork.Repository<Meter>().GetAllAsync();
            var dataList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.AllMeterCacheKey, getAll);
            var mappedData = dataList.Select(_ => _.GetMeterResponse()).ToList();
            return await Result<List<MeterResponseBase>>.SuccessAsync(mappedData);
        }
    }
}
