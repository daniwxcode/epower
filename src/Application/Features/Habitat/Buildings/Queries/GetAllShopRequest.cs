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
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries
{
    public class GetAllShopRequest : IRequest<Result<List<ShopResponseBase>>>
    {
    }
    internal class GetAllShopRequestHandler : IRequestHandler<GetAllShopRequest, Result<List<ShopResponseBase>>>
    {
        private IUnitOfWork<int> _unitOfWork;
        private IAppCache _cache;
        public GetAllShopRequestHandler(IUnitOfWork<int> unitOfWork, IAppCache Cache)
        {
            _unitOfWork = unitOfWork;
            _cache = Cache;
        }

        public async Task<Result<List<ShopResponseBase>>> Handle(GetAllShopRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Shop>().Entities.
            Include(_=>_.Meter).
            Include(_ => _.Building);

            Func<Task<List<Shop>>> getAll = () => repo.ToListAsync();
            var dataList = await _cache.GetOrAddAsync(ApplicationConstants.BuildingsCache.AllShops, getAll);
            var mappedData = dataList.Select(_ => _.GetShopResponse()).ToList();
            return await Result<List<ShopResponseBase>>.SuccessAsync(mappedData);
        }
    }
}
