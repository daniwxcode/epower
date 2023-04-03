using BlazorHero.CleanArchitecture.Shared.Wrapper;

using FluentValidation;
using MediatR;

using Microsoft.Extensions.Localization;

using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;

public class AddEditStoreCommand : IRequest<Result<int>>
{
    public int Id { get; set; } = 0;
    public int BuildingId { get; set; }        
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public string Code { get; set; }
}
public class AddEditStoreCommandValidator : AbstractValidator<AddEditStoreCommand>
{
    public AddEditStoreCommandValidator(IStringLocalizer<AddEditStoreCommandValidator> localizer)
    {
        RuleFor(request => request.SerialNumber)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Serial Number is required!"]);
        RuleFor(request => request.BuildingId).Must(
            x => x > 0).WithMessage(x=>localizer["L'Immeuble est requis"]);
        RuleFor(request => request.Code)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"]);
    }
}
internal class AddEditStoreCommandHandler: IRequestHandler<AddEditStoreCommand,Result<int>>
{
    public Task<Result<int>> Handle(AddEditStoreCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
