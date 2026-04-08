using BlazorHero.CleanArchitecture.Application.Features.Sellers.DTOs;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Sellers.Queries;

/// <summary>
/// Retourne toutes les sessions de caisse (pour superviseur/admin).
/// </summary>
public class GetAllCashShiftsQuery : IRequest<Result<List<CashShiftResponse>>>
{
    public bool OpenOnly { get; set; }
}

internal class GetAllCashShiftsQueryHandler(
    IUnitOfWork<int> unitOfWork,
    IUserService userService) : IRequestHandler<GetAllCashShiftsQuery, Result<List<CashShiftResponse>>>
{
    public async Task<Result<List<CashShiftResponse>>> Handle(GetAllCashShiftsQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.Repository<CashShift>().Entities
            .Include(cs => cs.Seller)
            .AsQueryable();

        if (request.OpenOnly)
            query = query.Where(cs => cs.Status == CashShiftStatus.Open);

        var shifts = await query
            .OrderByDescending(cs => cs.OpenedAt)
            .Select(cs => new
            {
                cs.Id,
                cs.SellerId,
                cs.Seller.UserId,
                cs.OpenedAt,
                cs.ClosedAt,
                cs.OpeningBalance,
                cs.ClosingBalance,
                cs.ExpectedBalance,
                cs.Notes,
                cs.Status,
                SalesCount = unitOfWork.Repository<Payment>().Entities.Count(p => p.CashShiftId == cs.Id),
                SalesTotal = unitOfWork.Repository<Payment>().Entities
                    .Where(p => p.CashShiftId == cs.Id).Sum(p => p.Amount),
            })
            .ToListAsync(cancellationToken);

        var usersResult = await userService.GetAllAsync();
        var usersLookup = usersResult.Succeeded
            ? usersResult.Data.ToDictionary(u => u.Id, u => u.UserFullName)
            : new Dictionary<string, string>();

        var result = shifts.Select(cs => new CashShiftResponse(
            cs.Id,
            cs.SellerId,
            usersLookup.GetValueOrDefault(cs.UserId, "Inconnu"),
            cs.OpenedAt,
            cs.ClosedAt,
            cs.OpeningBalance,
            cs.ClosingBalance,
            cs.ExpectedBalance,
            cs.Notes,
            cs.Status.ToString(),
            cs.SalesCount,
            cs.SalesTotal)).ToList();

        return await Result<List<CashShiftResponse>>.SuccessAsync(result);
    }
}
