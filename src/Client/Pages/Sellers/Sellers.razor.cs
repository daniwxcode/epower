using BlazorHero.CleanArchitecture.Application.Features.Sellers.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Sellers.DTOs;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Sellers;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Sellers;

public partial class Sellers
{
    [Inject] private ISellerManager SellerManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }

    private List<SellerResponse> _sellers = [];
    private string _searchString = string.Empty;
    private bool _loaded;
    private bool _loading;
    private bool _canCreate;
    private bool _canEdit;

    protected override async Task OnInitializedAsync()
    {
        var user = await _stateProvider.GetAuthenticationStateProviderUserAsync();
        _canCreate = (await _authorizationService.AuthorizeAsync(user, Permissions.Sellers.Create)).Succeeded;
        _canEdit = (await _authorizationService.AuthorizeAsync(user, Permissions.Sellers.Edit)).Succeeded;

        await LoadDataAsync();
        _loaded = true;
    }

    private async Task LoadDataAsync()
    {
        _loading = true;
        var response = await SellerManager.GetAllSellersAsync();
        if (response.Succeeded)
        {
            _sellers = response.Data;
        }
        else
        {
            foreach (var message in response.Messages)
            {
                _snackBar.Add(message, Severity.Error);
            }
        }
        _loading = false;
    }

    private bool Search(SellerResponse seller)
    {
        if (string.IsNullOrWhiteSpace(_searchString)) return true;
        return seller.FullName.Contains(_searchString, StringComparison.OrdinalIgnoreCase)
            || (seller.Zone?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false)
            || (seller.PhoneNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false);
    }

    private async Task InvokeAddModal()
    {
        var parameters = new DialogParameters();
        var dialog = await DialogService.ShowAsync<AddEditSellerModal>("Nouveau vendeur", parameters,
            new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await LoadDataAsync();
        }
    }

    private async Task InvokeEditModal(SellerResponse seller)
    {
        var parameters = new DialogParameters
        {
            [nameof(AddEditSellerModal.Model)] = new AddEditSellerCommand
            {
                Id = seller.Id,
                UserId = seller.UserId,
                Zone = seller.Zone,
                PhoneNumber = seller.PhoneNumber,
                IsActive = seller.IsActive,
                MaxCreditLimit = seller.MaxCreditLimit
            }
        };
        var dialog = await DialogService.ShowAsync<AddEditSellerModal>("Modifier le vendeur", parameters,
            new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await LoadDataAsync();
        }
    }
}
