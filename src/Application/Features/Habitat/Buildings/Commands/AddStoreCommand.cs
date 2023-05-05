using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;

public class AddEditStoreCommand : IRequest<Result<int>>
{
    public int Id { get; set; } = 0;
    public int BuildingId { get; set; }
    public string Name { get; set; }
    public string MeterSerialNumber { get; set; }

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
    public async Task<Result<int>> Handle(AddEditStoreCommand command, CancellationToken cancellationToken)
    {
        var shopRepos = _unitOfWork.Repository<Shop>();
        if (await shopRepos.Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Name == command.Name, cancellationToken))
        {
            return await Result<int>.FailAsync("Boutique déjà Existante.");
        }
        var meter = await _unitOfWork.Repository<Meter>().Entities.FirstOrDefaultAsync(_ => _.SerialNumber == command.MeterSerialNumber, cancellationToken);
        if (meter is null)

            if (command.Id == 0)
            {
                Shop shop = new Shop()
                {
                    Id = command.Id,
                    Name = command.Name,
                    BuildingId = command.BuildingId,
                    
                };
                await shopRepos.AddAsync(shop);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.BuildingsCache.Shops);
                return await Result<int>.SuccessAsync(shop.Id, "Boutique Enregistré avec Succès");
            }

        var dbItem = await shopRepos.GetByIdAsync(command.Id);
        if (dbItem == null)
        {
            return await Result<int>.FailAsync("Immeuble Inexistant");
        }
        dbItem.Name = command.Name;
        dbItem.BuildingId = command.BuildingId;
        await shopRepos.UpdateAsync(dbItem);
        await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.BuildingsCache.Shops);
        return await Result<int>.SuccessAsync(dbItem.Id, "Boutique Mise à jour avec Succès");
    }
}
