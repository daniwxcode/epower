using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using LazyCache;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Meters.Queries
{
    public class GetAllMetersByBuildingIdRequest : IRequest<Result<List<MeterResponseBase>>>
    {
        public int Id { get; set; }
        public GetAllMetersByBuildingIdRequest(int id)
        {
            Id = id;
        }
    }
    internal class GetAllMetersByBuildingIdRequestHandler : IRequestHandler<GetAllMetersByBuildingIdRequest, Result<List<MeterResponseBase>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAppCache _cache;
        public GetAllMetersByBuildingIdRequestHandler(IAppCache Cache, IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cache = Cache;
        }
        public async Task<Result<List<MeterResponseBase>>> Handle(GetAllMetersByBuildingIdRequest request, CancellationToken cancellationToken)
        {
            Func<Task<List<Meter>>> getAll = () => _unitOfWork.Repository<Meter>().Entities.Where(_=>_.BuildingId== request.Id).ToListAsync();
            var dataList = await _cache.GetOrAddAsync(ApplicationConstants.BuildingsCache.BuildindMetersCacheKey(request.Id), getAll);
            var mappedData = dataList.Select(_ => _.GetMeterResponse()).ToList();
            return await Result<List<MeterResponseBase>>.SuccessAsync(mappedData);
        }
    }
}
