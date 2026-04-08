using System.Collections.Generic;
using System.Linq;
using BlazorHero.CleanArchitecture.Application.Features.DocumentTypes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.DocumentTypes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.DocumentTypes.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Entities.Misc;

namespace BlazorHero.CleanArchitecture.Application.Mappings;

internal static class DocumentTypeMappingExtensions
{
    public static DocumentType ToDocumentType(this AddEditDocumentTypeCommand command) => new()
    {
        Id = command.Id,
        Name = command.Name,
        Description = command.Description
    };

    public static GetDocumentTypeByIdResponse ToGetByIdResponse(this DocumentType entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description
    };

    public static GetAllDocumentTypesResponse ToGetAllResponse(this DocumentType entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description
    };

    public static List<GetAllDocumentTypesResponse> ToGetAllResponseList(this IEnumerable<DocumentType> entities)
        => entities.Select(e => e.ToGetAllResponse()).ToList();
}
