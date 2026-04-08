using System;
using System.Collections.Generic;
using System.Linq;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Contracts;

namespace BlazorHero.CleanArchitecture.Application.Mappings;

internal static class ExtendedAttributeMappingExtensions
{
    public static TExtendedAttribute ToExtendedAttribute<TId, TEntityId, TEntity, TExtendedAttribute>(
        this AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> command)
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>, new()
        where TId : IEquatable<TId>
        => new()
        {
            Id = command.Id,
            EntityId = command.EntityId,
            Type = command.Type,
            Key = command.Key,
            Text = command.Text,
            Decimal = command.Decimal,
            DateTime = command.DateTime,
            Json = command.Json,
            ExternalId = command.ExternalId,
            Group = command.Group,
            Description = command.Description,
            IsActive = command.IsActive
        };

    public static GetExtendedAttributeByIdResponse<TId, TEntityId> ToGetByIdResponse<TId, TEntityId, TEntity>(
        this AuditableEntityExtendedAttribute<TId, TEntityId, TEntity> entity)
        where TEntity : IEntity<TEntityId>
        => new()
        {
            Id = entity.Id,
            EntityId = entity.EntityId,
            Type = entity.Type,
            Key = entity.Key,
            Text = entity.Text,
            Decimal = entity.Decimal,
            DateTime = entity.DateTime,
            Json = entity.Json,
            ExternalId = entity.ExternalId,
            Group = entity.Group,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            CreatedOn = entity.CreatedOn,
            LastModifiedBy = entity.LastModifiedBy,
            LastModifiedOn = entity.LastModifiedOn
        };

    public static GetAllExtendedAttributesResponse<TId, TEntityId> ToGetAllResponse<TId, TEntityId, TEntity>(
        this AuditableEntityExtendedAttribute<TId, TEntityId, TEntity> entity)
        where TEntity : IEntity<TEntityId>
        => new()
        {
            Id = entity.Id,
            EntityId = entity.EntityId,
            Type = entity.Type,
            Key = entity.Key,
            Text = entity.Text,
            Decimal = entity.Decimal,
            DateTime = entity.DateTime,
            Json = entity.Json,
            ExternalId = entity.ExternalId,
            Group = entity.Group,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            CreatedOn = entity.CreatedOn,
            LastModifiedBy = entity.LastModifiedBy,
            LastModifiedOn = entity.LastModifiedOn
        };

    public static List<GetAllExtendedAttributesResponse<TId, TEntityId>> ToGetAllResponseList<TId, TEntityId, TEntity>(
        this IEnumerable<AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>> entities)
        where TEntity : IEntity<TEntityId>
        => entities.Select(e => e.ToGetAllResponse<TId, TEntityId, TEntity>()).ToList();

    public static GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId> ToGetAllByEntityIdResponse<TId, TEntityId, TEntity>(
        this AuditableEntityExtendedAttribute<TId, TEntityId, TEntity> entity)
        where TEntity : IEntity<TEntityId>
        => new()
        {
            Id = entity.Id,
            EntityId = entity.EntityId,
            Type = entity.Type,
            Key = entity.Key,
            Text = entity.Text,
            Decimal = entity.Decimal,
            DateTime = entity.DateTime,
            Json = entity.Json,
            ExternalId = entity.ExternalId,
            Group = entity.Group,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            CreatedOn = entity.CreatedOn,
            LastModifiedBy = entity.LastModifiedBy,
            LastModifiedOn = entity.LastModifiedOn
        };

    public static List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>> ToGetAllByEntityIdResponseList<TId, TEntityId, TEntity>(
        this IEnumerable<AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>> entities)
        where TEntity : IEntity<TEntityId>
        => entities.Select(e => e.ToGetAllByEntityIdResponse<TId, TEntityId, TEntity>()).ToList();
}
