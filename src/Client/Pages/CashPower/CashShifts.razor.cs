using BlazorHero.CleanArchitecture.Application.Features.Sellers.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Sellers.DTOs;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Sellers;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.CashPower;

public partial class CashShifts
{
    [Inject] private ICashShiftManager CashShiftManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }

    private static readonly CultureInfo _fr = CultureInfo.GetCultureInfo("fr-FR");

    private List<CashShiftResponse> _shifts = [];
    private bool _loaded;
    private bool _loading;
    private bool _openOnly;
    private bool _canCreateRemittance;

    protected override async Task OnInitializedAsync()
    {
        var user = await _stateProvider.GetAuthenticationStateProviderUserAsync();
        _canCreateRemittance = (await _authorizationService.AuthorizeAsync(user, Permissions.Remittances.Create)).Succeeded;

        await LoadDataAsync();
        _loaded = true;
    }

    private async Task LoadDataAsync()
    {
        _loading = true;
        var response = await CashShiftManager.GetAllShiftsAsync(_openOnly);
        if (response.Succeeded)
        {
            _shifts = response.Data;
        }
        else
        {
            foreach (var msg in response.Messages)
            {
                _snackBar.Add(msg, Severity.Error);
            }
        }
        _loading = false;
    }

    private async Task OnOpenOnlyChanged(bool value)
    {
        _openOnly = value;
        await LoadDataAsync();
    }

    private async Task OpenRemittanceDialog(CashShiftResponse shift)
    {
        var parameters = new DialogParameters
        {
            ["ShiftId"] = shift.Id,
            ["SellerName"] = shift.SellerName,
            ["ExpectedBalance"] = shift.ExpectedBalance ?? 0m
        };
        var dialog = await DialogService.ShowAsync<RemittanceDialog>(
            "Enregistrer une remise de fonds", parameters,
            new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await LoadDataAsync();
        }
    }
}
