using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Sellers.Commands;

/// <summary>
/// Enregistre une remise de fonds par le superviseur pour un shift donné.
/// </summary>
public class CreateCashRemittanceCommand : IRequest<Result<int>>
{
    public int CashShiftId { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}

internal class CreateCashRemittanceCommandHandler(
    IUnitOfWork<int> unitOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<CreateCashRemittanceCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateCashRemittanceCommand request, CancellationToken cancellationToken)
    {
        var shift = await unitOfWork.Repository<CashShift>().Entities
            .FirstOrDefaultAsync(cs => cs.Id == request.CashShiftId, cancellationToken);

        if (shift is null)
            return await Result<int>.FailAsync("Session de caisse introuvable.");

        if (shift.Status != CashShiftStatus.Closed)
            return await Result<int>.FailAsync("La caisse doit être clôturée avant de pouvoir enregistrer une remise.");

        var remittance = new CashRemittance
        {
            CashShiftId = request.CashShiftId,
            Amount = request.Amount,
            ReceivedBy = currentUserService.UserId,
            RemittedAt = DateTime.UtcNow,
            Notes = request.Notes
        };

        await unitOfWork.Repository<CashRemittance>().AddAsync(remittance);
        await unitOfWork.Commit(cancellationToken);

        return await Result<int>.SuccessAsync(remittance.Id, "Remise de fonds enregistrée.");
    }
}
