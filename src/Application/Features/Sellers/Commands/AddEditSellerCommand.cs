using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Sellers.Commands;

public class AddEditSellerCommand : IRequest<Result<int>>
{
    public int Id { get; set; }

    [Required(ErrorMessage = "L'identifiant utilisateur est requis.")]
    public string UserId { get; set; }

    public string? Zone { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal MaxCreditLimit { get; set; }
}

internal class AddEditSellerCommandHandler(
    IUnitOfWork<int> unitOfWork) : IRequestHandler<AddEditSellerCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AddEditSellerCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == 0)
        {
            var exists = await unitOfWork.Repository<Seller>().Entities
                .AnyAsync(s => s.UserId == request.UserId, cancellationToken);

            if (exists)
                return await Result<int>.FailAsync("Un profil vendeur existe déjà pour cet utilisateur.");

            var seller = new Seller
            {
                UserId = request.UserId,
                Zone = request.Zone,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.IsActive,
                MaxCreditLimit = request.MaxCreditLimit
            };

            await unitOfWork.Repository<Seller>().AddAsync(seller);
            await unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(seller.Id, "Vendeur créé avec succès.");
        }
        else
        {
            var seller = await unitOfWork.Repository<Seller>().GetByIdAsync(request.Id);
            if (seller is null)
                return await Result<int>.FailAsync("Vendeur introuvable.");

            seller.Zone = request.Zone;
            seller.PhoneNumber = request.PhoneNumber;
            seller.IsActive = request.IsActive;
            seller.MaxCreditLimit = request.MaxCreditLimit;

            await unitOfWork.Repository<Seller>().UpdateAsync(seller);
            await unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(seller.Id, "Vendeur mis à jour.");
        }
    }
}
