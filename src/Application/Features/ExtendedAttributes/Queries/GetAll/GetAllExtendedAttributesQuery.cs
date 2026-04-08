using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Mappings;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using LazyCache;
using MediatR;

namespace BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAll
{
    public class GetAllExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequest<Result<List<GetAllExtendedAttributesResponse<TId, TEntityId>>>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        public GetAllExtendedAttributesQuery()
        {
        }
    }

    internal class GetAllExtendedAttributesQueryHandler<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequestHandler<GetAllExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>, Result<List<GetAllExtendedAttributesResponse<TId, TEntityId>>>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        private readonly IUnitOfWork<TId> _unitOfWork;
        private readonly IAppCache _cache;

        public GetAllExtendedAttributesQueryHandler(IUnitOfWork<TId> unitOfWork, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<Result<List<GetAllExtendedAttributesResponse<TId, TEntityId>>>> Handle(GetAllExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute> request, CancellationToken cancellationToken)
        {
            Func<Task<List<TExtendedAttribute>>> getAllExtendedAttributes = () => _unitOfWork.Repository<TExtendedAttribute>().GetAllAsync();
            var extendedAttributeList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllEntityExtendedAttributesCacheKey(typeof(TEntity).Name), getAllExtendedAttributes);
            var mappedExtendedAttributes = extendedAttributeList.Cast<AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>>().ToGetAllResponseList<TId, TEntityId, TEntity>();
            return await Result<List<GetAllExtendedAttributesResponse<TId, TEntityId>>>.SuccessAsync(mappedExtendedAttributes);
        }
    }
}