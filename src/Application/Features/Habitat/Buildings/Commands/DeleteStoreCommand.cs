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

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Stores.Commands
{
    public class DeleteStoreCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DeleteStoreCommand(int id)
        {
            Id = id;
        }
    }
    internal class DeleteStoreCommandHandler : IRequestHandler<DeleteStoreCommand, Result<int>>
    {
        private readonly IAppCache _appCache;
        private readonly IUnitOfWork<int> _unitOfWork;
        public DeleteStoreCommandHandler(IAppCache appCache, IUnitOfWork<int> unitOfWork)
        {
            _appCache = appCache;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteStoreCommand request, CancellationToken cancellationToken)
        {
            var db = _unitOfWork.Repository<Shop>();
            var store = await db.GetByIdAsync(request.Id);
            if (store == null)
            {
                return await Result<int>.FailAsync("Immeuble inexistant en Base");
            }
            await _unitOfWork.Repository<Shop>().DeleteAsync(store);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.BuildingsCache.AllShopKeys(store.BuildingId));
            return await Result<int>.SuccessAsync(request.Id, "Immeuble Supprimé");
        }
    }

}
