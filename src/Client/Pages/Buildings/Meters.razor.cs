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
    public partial class Meters
    {
        [Parameter]
        public int Id { get; set; } = 0;
        [Parameter]
        public string BuildingName { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<MeterResponseBase> _List = new();
        private MeterResponseBase item;
        private string _searchString = "";

        private ClaimsPrincipal _currentUser;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();


            await GetDataAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetDataAsync()
        {
            var response = await _cashPowerManager.GetAllMeters(Id);
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
            string deleteContent = $"Compteur?";
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id) }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Supprimer"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
            }
        }


        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (Id == 0)
                return;
            string libelle = string.Empty;
            string code = string.Empty;
            if (id > 0)
            {
                item = _List.FirstOrDefault(c => c.Id == id);
                if (item != null)
                {
                    libelle = item.SerialNumber;
                    code = item.code;
                }
            }
            parameters.Add("Model", new AddEditMeterCommand
            {
                BuildingId = Id,
                Id = id,
                SerialNumber = libelle,
                Code = code,
                IsActive = true,
            });
            parameters.Add("BuildingId", Id);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };

            var dialog = _dialogService.Show<AddEditStoreModal>(id == 0 ? _localizer["Ajouter"] : _localizer["Modifier"], parameters, options);

            var result = await dialog.Result;
            if (!result.Cancelled)
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
            await GetDataAsync();
            _loaded = true;
        }

        private bool Search(MeterResponseBase item)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (item.SerialNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }


    }
}
