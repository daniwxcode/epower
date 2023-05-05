using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.Localization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;

public class AddEditMeterCommand : IRequest<Result<int>>
{
    public int Id { get; set; } = 0;
    public string SerialNumber { get; set; }
    public string Code { get; set; }
    public List<int> Stores { get; set; } = new();


    public Meter GetInternalMeter()
    {
        return new Meter()
        {
            Id = Id,
            SerialNumber = SerialNumber,
            Code = Code,
            IsActive = true

        };
    }

    public bool IsInvalid
    {
        get { return (!Stores.Any()); }
    }

    public class AddEditMeterCommandValidator : AbstractValidator<AddEditMeterCommand>
    {
        public AddEditMeterCommandValidator(IStringLocalizer<AddEditMeterCommand> localizer)
        {
            RuleFor(request => request.SerialNumber)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Serial Number is required!"]);
            RuleFor(request => request.Code)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"]);
        }
    }

    internal class AddEditMeterCommandHandler : IRequestHandler<AddEditMeterCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditMeterCommand> _localizer;
        public AddEditMeterCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<AddEditMeterCommand> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }
        public async Task<Result<int>> Handle(AddEditMeterCommand request, CancellationToken cancellationToken)
        {
            if (request.IsInvalid)
                return await Result<int>.FailAsync(_localizer["Internal Meter needs to be linked to at least a store"]);
            return await SaveInternalMeter(request.GetInternalMeter(), cancellationToken);

        }

        private async Task<Result<int>> SaveInternalMeter(Meter meter, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.Repository<Meter>();
            if (meter.Id == 0)
            {
                await repository.AddAsync(meter);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(meter.Id, _localizer["Internal Meter saved successfully"]);
            }
            var dbItem = await repository.GetByIdAsync(meter.Id);
            if (dbItem is null)
                return await Result<int>.FailAsync(_localizer["Meter Not Found"]);
            dbItem.SerialNumber = meter.SerialNumber;
            dbItem.Code = meter.Code;
            await repository.UpdateAsync(dbItem);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(_localizer["Internal Meter updated Successfully"]);
        }
    }


}
