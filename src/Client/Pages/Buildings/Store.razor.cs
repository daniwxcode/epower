using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

using MudBlazor;

using System.Collections.Generic;

using System.Security.Claims;

using System.Threading.Tasks;

using System;
using BlazorHero.CleanArchitecture.Client.Extensions;
using System.Linq;

namespace BlazorHero.CleanArchitecture.Client.Pages.Buildings
{
    public partial class Store
    {
        [Parameter]
        public int Id { get; set; } = 0;
        [Parameter]
        public string BuildingName { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<ShopResponseBase> _List = new();
        private ShopResponseBase item;
        private string _searchString = "";

        private ClaimsPrincipal _currentUser;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();


            await GetBuildingAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetBuildingAsync()
        {
            var response = await _cashPowerManager.GetStores(Id);
            if (response.Succeeded)
            {
                _List = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = "Supprimer cette Boutique?";
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id) }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
            var dialog = await _dialogService.ShowAsync<Shared.Dialogs.DeleteConfirmation>(_localizer["Supprimer"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var response = await _cashPowerManager.DeleteStore(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }


        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (Id == 0)
                return;
            string libelle = "";
            if (id > 0)
            {
                item = _List.FirstOrDefault(c => c.Id == id);
                if (item != null)
                {
                    libelle = item.Name;
                }
            }
            parameters.Add("Model", new AddEditStoreCommand
            {
                BuildingId = Id,
                Id = id,
                Name = libelle,
            });
            parameters.Add("BuildingId", Id);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
            
            var dialog = await _dialogService.ShowAsync<AddEditStoreModal>(id == 0 ? _localizer["Ajouter"] : _localizer["Modifier"], parameters, options);
            
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await Reset();
            }
        }

        private void ViewProfile(int userId)
        {
            _navigationManager.NavigateTo($"/Building/Details/{userId}");
        }

        private async Task Reset()
        {
            _loaded = false;
            _List = new();
            await GetBuildingAsync();
            _loaded = true;
        }

        private bool Search(ShopResponseBase item)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (item.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.BuildingName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if(DisplayMeter(item.Meter).Contains(_searchString, StringComparison.OrdinalIgnoreCase) )
            {
                return true;
            }
            return false;
        }
        private string DisplayMeter(MeterResponseBase meterResponse)
        => meterResponse is null ? "" : $"{meterResponse.code}({meterResponse.SerialNumber})";

    }
}
