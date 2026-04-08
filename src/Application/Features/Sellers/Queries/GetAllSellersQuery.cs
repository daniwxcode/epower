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

public class GetAllSellersQuery : IRequest<Result<List<SellerResponse>>>
{
}

internal class GetAllSellersQueryHandler(
    IUnitOfWork<int> unitOfWork,
    IUserService userService) : IRequestHandler<GetAllSellersQuery, Result<List<SellerResponse>>>
{
    public async Task<Result<List<SellerResponse>>> Handle(GetAllSellersQuery request, CancellationToken cancellationToken)
    {
        var sellers = await unitOfWork.Repository<Seller>().Entities
            .OrderBy(s => s.Id)
            .ToListAsync(cancellationToken);

        var usersResult = await userService.GetAllAsync();
        var usersLookup = usersResult.Succeeded
            ? usersResult.Data.ToDictionary(u => u.Id, u => u.UserFullName)
            : new Dictionary<string, string>();

        var result = sellers.Select(s => new SellerResponse(
            s.Id,
            s.UserId,
            usersLookup.GetValueOrDefault(s.UserId, "Inconnu"),
            s.Zone,
            s.PhoneNumber,
            s.IsActive,
            s.MaxCreditLimit)).ToList();

        return await Result<List<SellerResponse>>.SuccessAsync(result);
    }
}
