using BlazorHero.CleanArchitecture.Application.Features.Sellers.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Sellers.DTOs;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Sellers;

public interface ISellerManager : IManager
{
    Task<IResult<List<SellerResponse>>> GetAllSellersAsync();
    Task<IResult<int>> AddEditSellerAsync(AddEditSellerCommand command);
}

public interface ICashShiftManager : IManager
{
    Task<IResult<int>> OpenShiftAsync(OpenCashShiftCommand command);
    Task<IResult<int>> CloseShiftAsync(CloseCashShiftCommand command);
    Task<IResult<ActiveShiftSummary>> GetActiveShiftAsync();
    Task<IResult<List<CashShiftResponse>>> GetAllShiftsAsync(bool openOnly = false);
    Task<IResult<int>> CreateRemittanceAsync(CreateCashRemittanceCommand command);
}
