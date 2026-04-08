using BlazorHero.CleanArchitecture.Application.Features.Sellers.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Sellers.DTOs;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Sellers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.CashPower;

public partial class MyShift
{
    [Inject] private ICashShiftManager CashShiftManager { get; set; }

    private static readonly CultureInfo _fr = CultureInfo.GetCultureInfo("fr-FR");

    private ActiveShiftSummary _activeShift;
    private bool _loading = true;
    private bool _closing;

    private decimal _openingBalance;
    private decimal _closingBalance;
    private string _closeNotes;

    protected override async Task OnInitializedAsync()
    {
        await LoadActiveShiftAsync();
        _loading = false;
    }

    private async Task LoadActiveShiftAsync()
    {
        var response = await CashShiftManager.GetActiveShiftAsync();
        _activeShift = response.Succeeded ? response.Data : null;
    }

    private async Task OpenShiftAsync()
    {
        _loading = true;
        var result = await CashShiftManager.OpenShiftAsync(new OpenCashShiftCommand
        {
            OpeningBalance = _openingBalance
        });

        if (result.Succeeded)
        {
            _snackBar.Add(result.Messages[0], Severity.Success);
            await LoadActiveShiftAsync();
        }
        else
        {
            foreach (var msg in result.Messages)
            {
                _snackBar.Add(msg, Severity.Error);
            }
        }
        _loading = false;
    }

    private async Task CloseShiftAsync()
    {
        if (_activeShift is null) return;

        _closing = true;
        var result = await CashShiftManager.CloseShiftAsync(new CloseCashShiftCommand
        {
            ShiftId = _activeShift.ShiftId,
            ClosingBalance = _closingBalance,
            Notes = _closeNotes
        });

        if (result.Succeeded)
        {
            _snackBar.Add(result.Messages[0], Severity.Success);
            _activeShift = null;
            _closingBalance = 0;
            _closeNotes = null;
        }
        else
        {
            foreach (var msg in result.Messages)
            {
                _snackBar.Add(msg, Severity.Error);
            }
        }
        _closing = false;
    }
}
