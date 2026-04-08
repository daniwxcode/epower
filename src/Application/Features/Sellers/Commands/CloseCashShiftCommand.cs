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

public class CloseCashShiftCommand : IRequest<Result<int>>
{
    public int ShiftId { get; set; }
    public decimal ClosingBalance { get; set; }
    public string? Notes { get; set; }
}

internal class CloseCashShiftCommandHandler(
    IUnitOfWork<int> unitOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<CloseCashShiftCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CloseCashShiftCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var shift = await unitOfWork.Repository<CashShift>().Entities
            .Include(cs => cs.Seller)
            .FirstOrDefaultAsync(cs => cs.Id == request.ShiftId && cs.Status == CashShiftStatus.Open, cancellationToken);

        if (shift is null)
            return await Result<int>.FailAsync("Session de caisse introuvable ou déjà clôturée.");

        if (shift.Seller.UserId != userId)
            return await Result<int>.FailAsync("Vous ne pouvez clôturer que votre propre caisse.");

        var salesTotal = await unitOfWork.Repository<Payment>().Entities
            .Where(p => p.CashShiftId == shift.Id)
            .SumAsync(p => p.Amount, cancellationToken);

        var feesTotal = await unitOfWork.Repository<Payment>().Entities
            .Where(p => p.CashShiftId == shift.Id)
            .SumAsync(p => p.BilledAmount, cancellationToken);

        shift.ClosedAt = DateTime.UtcNow;
        shift.ClosingBalance = request.ClosingBalance;
        shift.ExpectedBalance = shift.OpeningBalance + salesTotal + feesTotal;
        shift.Notes = request.Notes;
        shift.Status = CashShiftStatus.Closed;

        await unitOfWork.Repository<CashShift>().UpdateAsync(shift);
        await unitOfWork.Commit(cancellationToken);

        return await Result<int>.SuccessAsync(shift.Id, "Caisse clôturée avec succès.");
    }
}
