using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
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
    public int BuildingId { get; set; }
    public string SerialNumber { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public Meter GetMeter() => new Meter()
    {
        Id = Id,
        BuildingId = BuildingId,
        SerialNumber = SerialNumber,
        Code = Code,
        IsActive = IsActive
    };


}
public class AddEditMeterCommandValidator : AbstractValidator<AddEditMeterCommand>
{
    public AddEditMeterCommandValidator(IStringLocalizer<AddEditMeterCommand> localizer)
    {
        RuleFor(request => request.SerialNumber)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Le Numéro de Série est Obligatoire"]);
        RuleFor(request => request.Code)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Le code est réquis"]);
        RuleFor(request => request.BuildingId).Must(x => x > 0).WithMessage(x => localizer["L'immeuble doit être renseigné"]);
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
        var repository = _unitOfWork.Repository<Meter>();
        var existingMeter = repository.Entities.FirstOrDefault(_ =>
    _.Id != request.Id &&
    (_.SerialNumber == request.SerialNumber ||
     (_.BuildingId == request.BuildingId && _.Code == request.Code) ||
     (_.BuildingId != request.BuildingId && _.SerialNumber == request.SerialNumber))
);

        if (existingMeter != null)
        {
            if (existingMeter.SerialNumber == request.SerialNumber)
                return await Result<int>.FailAsync(_localizer["Un compteur avec un même numéro de Série existe dejà"]);

            if (existingMeter.BuildingId == request.BuildingId && existingMeter.Code == request.Code)
                return await Result<int>.FailAsync(_localizer["Un compteur avec un même code de Série existe dejà dans cet Immeuble"]);

            if (existingMeter.BuildingId != request.BuildingId && existingMeter.SerialNumber == request.SerialNumber)
                return await Result<int>.FailAsync(_localizer["Ce compteur est déjà lié à un autre Immeuble"]);
        }
        try
        {
            if (request.Id == 0)
            {

                var meter = request.GetMeter();
                await repository.AddAsync(meter);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.BuildingsCache.BuildindMetersCacheKey(request.BuildingId) );
                return await Result<int>.SuccessAsync(meter.Id, _localizer["Compteur Enregistré avec Succès"]);
            }

            var dbBuilding = await _unitOfWork.Repository<Building>().GetByIdAsync(request.BuildingId);
            if (dbBuilding is null)
                return await Result<int>.FailAsync("Impossible d'ajouter un Compteur à un immeuble inexistant");          
            var dbItem = await repository.GetByIdAsync(request.Id);
            if (dbItem is null)
                return await Result<int>.FailAsync(_localizer["Compteur inexistant"]);
            dbItem.SerialNumber = request.SerialNumber;
            dbItem.Code = request.Code;
            dbItem.BuildingId = request.BuildingId;
            dbItem.IsActive = request.IsActive;
            await repository.UpdateAsync(dbItem);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.BuildingsCache.BuildindMetersCacheKey(request.BuildingId));
            return await Result<int>.SuccessAsync(_localizer["Compteur Mis à jour avec succès"]);
        }catch(Exception e)
        {
            return await Result<int>.FailAsync(e.Message);
        }

    }
}



