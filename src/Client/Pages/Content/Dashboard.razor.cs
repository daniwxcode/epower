using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Dashboard;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Application.Features.Dashboards.Queries.GetData;
using ChartSeries = MudBlazor.ChartSeries<double>;

namespace BlazorHero.CleanArchitecture.Client.Pages.Content
{
    public partial class Dashboard
    {
        [Inject] private IDashboardManager DashboardManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public DashboardDataResponse Model { get; set; } 
        

        private readonly string[] _dataEnterBarChartXAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };       
        private readonly List<ChartSeries> _dataEnterBarChartSeries = new();
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            HubConnection.On(ApplicationConstants.SignalR.ReceiveUpdateDashboard, async () =>
            {
                await LoadDataAsync();
                StateHasChanged();
            });
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            var response = await DashboardManager.GetDataAsync();
            if (response.Succeeded)
            {
                Model= response.Data;
                foreach (var item in response.Data.DataEnterBarChart)
                {
                    _dataEnterBarChartSeries
                        .RemoveAll(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                    _dataEnterBarChartSeries.Add(new ChartSeries { Name = item.Name, Data = item.Data });
                }
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
    }
}