using Blazored.FluentValidation;

using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Client.Extensions;

using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.SignalR.Client;

using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Shared.Components
{
    public partial class SellCreditForm
    {
        public MakeAPaymentCommand Model { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private bool loading = false;
        protected override async Task OnInitializedAsync()
        {           
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        
        private async Task SaveAsync()
        {            
            loading= true;
            var response = await _cashPowerManager.BuyCredit(Model);
            loading = false;
            if(response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], MudBlazor.Severity.Success);
                Model = new MakeAPaymentCommand();
            }
            else
            {
                response.Messages.ForEach(_=> _snackBar.Add(_, MudBlazor.Severity.Error));
            }
        }
    }
}
