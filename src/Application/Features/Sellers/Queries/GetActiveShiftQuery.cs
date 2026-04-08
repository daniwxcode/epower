using BlazorHero.CleanArchitecture.Application.Features.Sellers.DTOs;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Sellers.Queries;

/// <summary>
/// Retourne la session de caisse active du vendeur connecté.
/// </summary>
public class GetActiveShiftQuery : IRequest<Result<ActiveShiftSummary>>
{
}

internal class GetActiveShiftQueryHandler(
    IUnitOfWork<int> unitOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<GetActiveShiftQuery, Result<ActiveShiftSummary>>
{
    public async Task<Result<ActiveShiftSummary>> Handle(GetActiveShiftQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var shift = await unitOfWork.Repository<CashShift>().Entities
            .Where(cs => cs.Seller.UserId == userId && cs.Status == CashShiftStatus.Open)
            .Select(cs => new
            {
                cs.Id,
                cs.OpenedAt,
                cs.OpeningBalance,
                SalesCount = unitOfWork.Repository<Payment>().Entities.Count(p => p.CashShiftId == cs.Id),
                SalesTotal = unitOfWork.Repository<Payment>().Entities
                    .Where(p => p.CashShiftId == cs.Id).Sum(p => p.Amount),
                FeesTotal = unitOfWork.Repository<Payment>().Entities
                    .Where(p => p.CashShiftId == cs.Id).Sum(p => p.BilledAmount),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (shift is null)
            return await Result<ActiveShiftSummary>.FailAsync("Aucune caisse ouverte.");

        var summary = new ActiveShiftSummary(
            shift.Id,
            shift.OpenedAt,
            shift.OpeningBalance,
            shift.SalesCount,
            shift.SalesTotal,
            shift.FeesTotal);

        return await Result<ActiveShiftSummary>.SuccessAsync(summary);
    }
}
