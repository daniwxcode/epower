using Blazored.FluentValidation;

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
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Buildings
{
    public partial class AddEditStoreModal
    {
        [Parameter]
        public AddEditStoreCommand Model { get; set; } = new();

        [Parameter]
        public int BuildingId { get; set; }
        private string BuildingName { get; set; }
        private List<MeterResponseBase> _buildingMeters = new();
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await _cashPowerManager.AddStore(Model);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync(BuildingId);
            Model.BuildingId = BuildingId;
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private string MeterDisplay(MeterResponseBase model) =>
           model == null ? string.Empty : $"{model.code} -({model.SerialNumber})";
        private async Task<IEnumerable<int>> Search(string value, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(value))
                return _buildingMeters.Select(x => x.Id);
            return _buildingMeters
                .Where(t => t.code.Contains(value, StringComparison.InvariantCultureIgnoreCase) || t.SerialNumber.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(t => t.Id)
                .ToList();
        }
        private async Task LoadDataAsync(int id)
        {
            var response = await _cashPowerManager.GetAllMeters(id);
            if (response.Succeeded)
            {
                _buildingMeters = response.Data;
            }
            else
            {
                response.Messages.ForEach(_ => _snackBar.Add(_, Severity.Error));
            }

        }
    }
}
