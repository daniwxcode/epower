using BlazorHero.CleanArchitecture.Shared.Wrapper;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.Localization;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;

public class AddEditMeterCommand : IRequest<Result<int>>
{
    public int Id { get; set; } = 0;
    public string SerialNumber { get; set; }
    public string Code { get; set; }
}

public class AddEditMeterCommandValidator : AbstractValidator<AddEditMeterCommand>
{
    public AddEditMeterCommandValidator(IStringLocalizer<AddEditMeterCommandValidator> localizer)
    {
        RuleFor(request => request.SerialNumber)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Serial Number is required!"]);
        RuleFor(request => request.Code)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"]);
    }
}

internal class AddEditMeterCommandHandler : IRequestHandler<AddEditMeterCommand, Result<int>>
{
    public Task<Result<int>> Handle(AddEditMeterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
