using BlazorHero.CleanArchitecture.Application.Features.Products.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Mappings;

internal static class ProductMappingExtensions
{
    public static Product ToProduct(this AddEditProductCommand command) => new()
    {
        Id = command.Id,
        Name = command.Name,
        Barcode = command.Barcode,
        Description = command.Description,
        ImageDataURL = command.ImageDataURL,
        Rate = command.Rate,
        BrandId = command.BrandId
    };
}
