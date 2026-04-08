using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Enums;
using BlazorHero.CleanArchitecture.Application.Features.Sellers.DTOs;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Sellers;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;

using MudBlazor;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.CashPower
{
    public partial class Pos
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private ICashShiftManager CashShiftManager { get; set; }

        private static readonly CultureInfo _frCulture = CultureInfo.GetCultureInfo("fr-FR");

        private static readonly decimal[] _quickAmounts =
            [1_000, 2_000, 5_000, 10_000, 20_000, 50_000, 100_000];

        // Search state
        private int _searchTabIndex;
        private string _serialSearch = string.Empty;
        private ShopResponseBase _selectedShop;
        private MeterResponseBase _foundMeter;
        private bool _searchPerformed;
        private List<ShopResponseBase> _allShops = [];

        // Shift state
        private ActiveShiftSummary _activeShift;

        // Sale state
        private decimal _amount;
        private bool _loading;
        private BuyCreditResponse _lastSale;
        private List<PayementResponseBase> _recentSales = [];

        // Error state
        private string _errorMessage;
        private string _errorTitle;

        private bool CanSubmit =>
            _amount > 0
            && !_loading
            && (_foundMeter is not null || !string.IsNullOrWhiteSpace(_serialSearch));

        protected override async Task OnInitializedAsync()
        {
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            await LoadActiveShiftAsync();
            await LoadRecentSalesAsync();
            await LoadShopsAsync();
        }

        private async Task LoadActiveShiftAsync()
        {
            try
            {
                var response = await CashShiftManager.GetActiveShiftAsync();
                _activeShift = response.Succeeded ? response.Data : null;
            }
            catch
            {
                _activeShift = null;
            }
        }

        private void ClearError()
        {
            _errorMessage = null;
            _errorTitle = null;
        }

        private void SetError(string title, string message)
        {
            _errorTitle = title;
            _errorMessage = message;
        }

        // ── Search logic ──

        private async Task OnSerialKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && !string.IsNullOrWhiteSpace(_serialSearch))
            {
                await SearchBySerialAsync();
            }
        }

        private async Task SearchBySerialAsync()
        {
            ClearError();
            _searchPerformed = true;
            _foundMeter = null;

            try
            {
                var response = await _cashPowerManager.GetMeterBySerial(_serialSearch.Trim());
                if (response.Succeeded)
                {
                    _foundMeter = response.Data;
                    _serialSearch = _foundMeter.SerialNumber;
                }
                else
                {
                    foreach (var msg in response.Messages)
                    {
                        _snackBar.Add(msg, Severity.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                SetError("Erreur de recherche", $"Impossible de rechercher le compteur : {ex.Message}");
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task LoadShopsAsync()
        {
            try
            {
                var response = await _cashPowerManager.GetStores();
                if (response.Succeeded)
                {
                    _allShops = response.Data;
                }
            }
            catch (Exception)
            {
                _snackBar.Add("Impossible de charger la liste des boutiques.", Severity.Warning);
            }
        }

        private Task<IEnumerable<ShopResponseBase>> SearchShopsAsync(string value, System.Threading.CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Task.FromResult(_allShops.AsEnumerable());

            var filtered = _allShops
                .Where(s => s.Name.Contains(value, StringComparison.OrdinalIgnoreCase)
                         || s.BuildingName.Contains(value, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(filtered);
        }

        private async Task OnShopSelected(ShopResponseBase shop)
        {
            ClearError();
            _selectedShop = shop;
            _foundMeter = null;
            _searchPerformed = false;

            if (shop?.Meter is not null)
            {
                _foundMeter = shop.Meter;
                _serialSearch = _foundMeter.SerialNumber;
                _searchPerformed = true;
            }
            else if (shop is not null)
            {
                _searchPerformed = true;
            }

            await InvokeAsync(StateHasChanged);
        }

        // ── Fee calculation ──

        private static int GetFees(decimal amount) => (int)amount switch
        {
            <= 0 => 0,
            > 1_000_000 => 1000,
            > 300_000 => 700,
            > 50_000 => 500,
            > 5_000 => 300,
            _ => 200
        };

        // ── Sale execution ──

        private async Task ConfirmAndPayAsync()
        {
            ClearError();
            var serial = _foundMeter?.SerialNumber ?? _serialSearch.Trim();
            var total = (_amount + GetFees(_amount)).ToString("N0", _frCulture);

            var confirmParams = new DialogParameters
            {
                ["ContentText"] = $"Compteur : {serial} — Montant : {_amount.ToString("N0", _frCulture)} FCFA — Total : {total} FCFA. Confirmer cette vente ?"
            };
            var confirmDialog = await DialogService.ShowAsync<Shared.Dialogs.DeleteConfirmation>(
                "Confirmer la vente", confirmParams,
                new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true });
            var confirmResult = await confirmDialog.Result;
            if (confirmResult.Canceled) return;

            _loading = true;
            await InvokeAsync(StateHasChanged);

            try
            {
                var command = new MakeAPaymentCommand
                {
                    SerialNumber = serial,
                    MeterId = _foundMeter?.Id ?? 0,
                    Amount = _amount
                };

                var response = await _cashPowerManager.BuyCredit(command);
                _loading = false;

                if (response.Succeeded)
                {
                    _lastSale = response.Data;
                    _snackBar.Add(response.Messages[0], Severity.Success);
                    await LoadRecentSalesAsync();
                    await LoadActiveShiftAsync();
                }
                else
                {
                    var joined = string.Join(" | ", response.Messages);
                    SetError("Échec de la vente", joined);
                    foreach (var msg in response.Messages)
                    {
                        _snackBar.Add(msg, Severity.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                _loading = false;
                SetError("Erreur inattendue", $"Une erreur est survenue lors de la vente : {ex.Message}");
                _snackBar.Add("Une erreur inattendue est survenue. Veuillez réessayer.", Severity.Error);
            }

            await InvokeAsync(StateHasChanged);
        }

        private void NewSale()
        {
            ClearError();
            _lastSale = null;
            _foundMeter = null;
            _selectedShop = null;
            _serialSearch = string.Empty;
            _amount = 0;
            _searchPerformed = false;
        }

        // ── Recent sales ──

        private async Task LoadRecentSalesAsync()
        {
            try
            {
                var response = await _cashPowerManager.GetPayementByCriteria(
                    PaymentRequestCriteria.All, string.Empty, 0, 5);
                if (response.Succeeded)
                {
                    _recentSales = response.Data.ToList();
                }
            }
            catch (Exception)
            {
                // Non-blocking — recent sales is not critical
            }
        }

        // ── Printing ──

        private async Task PrintReceiptAsync()
        {
            if (_lastSale is null) return;
            await ShowPdfDialog(_lastSale.Id);
        }

        private async Task PrintSaleAsync(int saleId)
        {
            await ShowPdfDialog(saleId);
        }

        private async Task ShowPdfDialog(int saleId)
        {
            var parameters = new DialogParameters
            {
                ["url"] = ApplicationConstants.FileConstants.GetReceipt(saleId)
            };

            var dialog = await DialogService.ShowAsync<Shared.Components.DocumentView>(
                title: "REÇU DE VENTE",
                parameters,
                new DialogOptions
                {
                    FullWidth = true,
                    MaxWidth = MaxWidth.ExtraLarge,
                    FullScreen = true,
                    CloseButton = true,
                    BackdropClick = false,
                    NoHeader = false
                });

            await dialog.Result;
        }
    }
}

