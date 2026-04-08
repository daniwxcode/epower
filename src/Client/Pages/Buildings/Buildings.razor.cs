using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

using MudBlazor;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Buildings
{
    public partial class Buildings
    {


        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<BuildingResponseBase> _List = new();
        private BuildingResponseBase item;
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
        private void ViewProfile(int buildingId)
        {
            _navigationManager.NavigateTo($"/Building/Details/{buildingId}");
        }
        private async Task GetBuildingAsync()
        {
            var response = await _cashPowerManager.GetAllBuilding();
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
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id) }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
            var dialog = await  _dialogService.ShowAsync<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var response = await _cashPowerManager.DeleteBuilding(id);
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

        //private async Task ExportToExcel()
        //{
        //    var response = await BrandManager.ExportToExcelAsync(_searchString);
        //    if (response.Succeeded)
        //    {
        //        await _jsRuntime.InvokeVoidAsync("Download", new
        //        {
        //            ByteArray = response.Data,
        //            FileName = $"{nameof(Building).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
        //            MimeType = ApplicationConstants.MimeTypes.OpenXml
        //        });
        //        _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
        //            ? _localizer["Building exported"]
        //            : _localizer["Filtered Building exported"], Severity.Success);
        //    }
        //    else
        //    {
        //        foreach (var message in response.Messages)
        //        {
        //            _snackBar.Add(message, Severity.Error);
        //        }
        //    }
        //}

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                item = _List.FirstOrDefault(c => c.Id == id);
                if (item != null)
                {
                    parameters.Add("Model", new AddEditBuildingCommand
                    {
                        Id = item.Id,
                        Name = item.name,
                        Address = item.Address,

                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
            var dialog = await _dialogService.ShowAsync<AddEditBuildingModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await Reset();
            }
        }

        //private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        //{
        //    var request = new ImportBuildingCommand { UploadRequest = uploadFile };
        //    var result = await BrandManager.ImportAsync(request);
        //    return result;
        //}

        //private async Task InvokeImportModal()
        //{
        //    var parameters = new DialogParameters
        //    {
        //        { nameof(ImportExcelModal.ModelName), _localizer["Building"].ToString() }
        //    };
        //    Func<UploadRequest, Task<IResult<int>>> importExcel = ImportExcel;
        //    parameters.Add(nameof(ImportExcelModal.OnSaved), importExcel);
        //    var options = new DialogOptions
        //    {
        //        CloseButton = true,
        //        MaxWidth = MaxWidth.Small,
        //        FullWidth = true,
        //        BackdropClick = false
        //    };
        //    var dialog = await _dialogService.ShowAsync<ImportExcelModal>(_localizer["Import"], parameters, options);
        //    var result = await dialog.Result;
        //    if (!result.Canceled)
        //    {
        //        await Reset();
        //    }
        //}

        private async Task Reset()
        {
            _loaded = false;
            _List = new();
            await GetBuildingAsync();
            _loaded = true;
        }

        private bool Search(BuildingResponseBase item)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (item.name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return item.Address?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}

