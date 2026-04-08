using BlazorHero.CleanArchitecture.Application.Features.Sellers.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Sellers.DTOs;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Routes;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Sellers;

public class SellerManager(HttpClient httpClient) : ISellerManager
{
    public async Task<IResult<List<SellerResponse>>> GetAllSellersAsync()
    {
        var response = await httpClient.GetAsync(SellerEndpoints.GetAll);
        return await response.ToResult<List<SellerResponse>>();
    }

    public async Task<IResult<int>> AddEditSellerAsync(AddEditSellerCommand command)
    {
        var response = await httpClient.PostAsJsonAsync(SellerEndpoints.AddEdit, command);
        return await response.ToResult<int>();
    }
}

public class CashShiftManager(HttpClient httpClient) : ICashShiftManager
{
    public async Task<IResult<int>> OpenShiftAsync(OpenCashShiftCommand command)
    {
        var response = await httpClient.PostAsJsonAsync(CashShiftEndpoints.Open, command);
        return await response.ToResult<int>();
    }

    public async Task<IResult<int>> CloseShiftAsync(CloseCashShiftCommand command)
    {
        var response = await httpClient.PostAsJsonAsync(CashShiftEndpoints.Close, command);
        return await response.ToResult<int>();
    }

    public async Task<IResult<ActiveShiftSummary>> GetActiveShiftAsync()
    {
        var response = await httpClient.GetAsync(CashShiftEndpoints.GetActive);
        return await response.ToResult<ActiveShiftSummary>();
    }

    public async Task<IResult<List<CashShiftResponse>>> GetAllShiftsAsync(bool openOnly = false)
    {
        var url = openOnly ? CashShiftEndpoints.GetAllOpen : CashShiftEndpoints.GetAll;
        var response = await httpClient.GetAsync(url);
        return await response.ToResult<List<CashShiftResponse>>();
    }

    public async Task<IResult<int>> CreateRemittanceAsync(CreateCashRemittanceCommand command)
    {
        var response = await httpClient.PostAsJsonAsync(CashShiftEndpoints.CreateRemittance, command);
        return await response.ToResult<int>();
    }
}
