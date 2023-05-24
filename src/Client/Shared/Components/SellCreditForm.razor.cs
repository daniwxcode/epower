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
        private bool canSellAgain = true;
        private string Ref { get; set; } = "000000000000";
        private string Code { get; set; } = "000000000000";
        
        public decimal Kw { get; set; } = 0;
        private int getSize { get { return canSellAgain ? 6 : 12; } }
        protected override async Task OnInitializedAsync()
        {           
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private void reset()
        {
            canSellAgain = true;
            Model = new MakeAPaymentCommand();
            Code = string.Empty;
            Kw = 0;
            Ref = string.Empty;
        }
        
        private async Task SaveAsync()
        {            
            loading= true;
            var response = await _cashPowerManager.BuyCredit(Model);
            loading = false;
            if(response.Succeeded)
            {
                canSellAgain = false;
                Kw = response.Data.Credit;
                Code = response.Data.Code;
                Ref = response.Data.Reference;
                _snackBar.Add(response.Messages[0], MudBlazor.Severity.Success);
               
            }
            else
            {
                response.Messages.ForEach(_=> _snackBar.Add(_, MudBlazor.Severity.Error));
            }
            await InvokeAsync(StateHasChanged);
        }
    }
}
