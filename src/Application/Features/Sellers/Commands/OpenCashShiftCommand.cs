using BlazorHero.CleanArchitecture.Application.Features.Sellers.DTOs;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Sellers.Commands;

public class OpenCashShiftCommand : IRequest<Result<int>>
{
    public decimal OpeningBalance { get; set; }
}

internal class OpenCashShiftCommandHandler(
    IUnitOfWork<int> unitOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<OpenCashShiftCommand, Result<int>>
{
    public async Task<Result<int>> Handle(OpenCashShiftCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var seller = await unitOfWork.Repository<Seller>().Entities
            .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive, cancellationToken);

        if (seller is null)
            return await Result<int>.FailAsync("Aucun profil vendeur actif trouvé pour cet utilisateur.");

        var hasOpenShift = await unitOfWork.Repository<CashShift>().Entities
            .AnyAsync(cs => cs.SellerId == seller.Id && cs.Status == CashShiftStatus.Open, cancellationToken);

        if (hasOpenShift)
            return await Result<int>.FailAsync("Vous avez déjà une caisse ouverte. Clôturez-la avant d'en ouvrir une nouvelle.");

        var shift = new CashShift
        {
            SellerId = seller.Id,
            OpenedAt = DateTime.UtcNow,
            OpeningBalance = request.OpeningBalance,
            Status = CashShiftStatus.Open
        };

        await unitOfWork.Repository<CashShift>().AddAsync(shift);
        await unitOfWork.Commit(cancellationToken);

        return await Result<int>.SuccessAsync(shift.Id, "Caisse ouverte avec succès.");
    }
}
