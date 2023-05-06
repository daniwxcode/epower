using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Meter = BlazorHero.CleanArchitecture.Domain.Entities.Bail.Meter;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;

public class AddEditStoreCommand : IRequest<Result<int>>
{
    public int Id { get; set; } = 0;
    public int BuildingId { get; set; }
    public int? MeterId { get; set; }
    public string Name { get; set; }
    public string? MeterSerialNumber { get; set; }
    public string? Code { get; set; }

}

public class AddEditStoreCommandValidator : AbstractValidator<AddEditStoreCommand>
{
    public AddEditStoreCommandValidator(IStringLocalizer<AddEditStoreCommandValidator> localizer)
    {

        RuleFor(request => request.BuildingId).Must(
            x => x > 0).WithMessage(x => localizer["L'Immeuble est requis"]);

    }
}

internal class AddEditStoreCommandHandler : IRequestHandler<AddEditStoreCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    public AddEditStoreCommandHandler(IUnitOfWork<int> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    private async Task<Meter> getExistingMeter(AddEditStoreCommand command)
    {
        var meterRepos = _unitOfWork.Repository<Meter>();
        if (command.MeterId != null)
        {
            return await meterRepos.GetByIdAsync(command.MeterId.Value);
        }

        if (command.MeterSerialNumber != null)
        {
            return await meterRepos.Entities.FirstOrDefaultAsync(_ => _.SerialNumber == command.MeterSerialNumber);
        }

        if (command.Code != null)
        {
            return await meterRepos.Entities.FirstOrDefaultAsync(_ => _.Code == command.Code);
        }

        return null;
    }
    public async Task<Result<int>> Handle(AddEditStoreCommand command, CancellationToken cancellationToken)
    {
        var shopRepos = _unitOfWork.Repository<Shop>();
        if (await shopRepos.Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Name == command.Name && p.BuildingId== command.BuildingId, cancellationToken))
        {
            return await Result<int>.FailAsync("Boutique déjà Existante.");
        }
        Meter dbMeter = await getExistingMeter(command);

        if (dbMeter is null)
        {
            return await Result<int>.FailAsync("Le compteur est inexistant");
        }
        if (dbMeter.BuildingId != command.BuildingId)
        {
            return await Result<int>.FailAsync("Le compteur et la Boutique ne sont pas dans le même Immeuble");
        }

        if (command.Id == 0)
        {
            Shop shop = new Shop()
            {
                Id = command.Id,
                Name = command.Name,
                BuildingId = command.BuildingId,
                MeterId = command.MeterId.Value,

            };
            await shopRepos.AddAsync(shop);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.BuildingsCache.AllShops);
            return await Result<int>.SuccessAsync(shop.Id, "Boutique Enregistrée avec Succès");
        }


        var dbItem = await shopRepos.GetByIdAsync(command.Id);
        if (dbItem == null)
        {
            return await Result<int>.FailAsync("Immeuble Inexistant");
        }

        if (dbMeter.BuildingId != dbItem.BuildingId)
        {
            return await Result<int>.FailAsync("Le compteur et la Boutique ne sont pas dans le même Immeuble");
        }
        dbItem.Name = command.Name;
        await shopRepos.UpdateAsync(dbItem);
        await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.BuildingsCache.AllShops);
        return await Result<int>.SuccessAsync(dbItem.Id, "Boutique Mise à jour avec Succès");
    }
}
