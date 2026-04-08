using BlazorHero.CleanArchitecture.Application.Features.Documents.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Documents.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Entities.Misc;

namespace BlazorHero.CleanArchitecture.Application.Mappings;

internal static class DocumentMappingExtensions
{
    public static Document ToDocument(this AddEditDocumentCommand command) => new()
    {
        Id = command.Id,
        Title = command.Title,
        Description = command.Description,
        IsPublic = command.IsPublic,
        URL = command.URL,
        DocumentTypeId = command.DocumentTypeId
    };

    public static GetDocumentByIdResponse ToGetByIdResponse(this Document document) => new()
    {
        Id = document.Id,
        Title = document.Title,
        Description = document.Description,
        IsPublic = document.IsPublic,
        CreatedBy = document.CreatedBy,
        CreatedOn = document.CreatedOn,
        URL = document.URL,
        DocumentType = document.DocumentType?.Name,
        DocumentTypeId = document.DocumentTypeId
    };
}
