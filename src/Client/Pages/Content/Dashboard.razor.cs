using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Dashboards.Queries.GetData;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Dashboard;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using ChartSeries = MudBlazor.ChartSeries<double>;

namespace BlazorHero.CleanArchitecture.Client.Pages.Content
{
    public partial class Dashboard
    {
        [Inject] private IDashboardManager DashboardManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private DashboardDataResponse Model { get; set; }

        private static readonly CultureInfo _frCulture = CultureInfo.GetCultureInfo("fr-FR");

        private static readonly string[] _monthLabels =
            ["Jan", "Fév", "Mar", "Avr", "Mai", "Juin", "Juil", "Aoû", "Sep", "Oct", "Nov", "Déc"];

        private readonly List<ChartSeries> _revenueBarSeries = [];
        private readonly List<ChartSeries> _dailyTrendSeries = [];
        private string[] _dailyLabels = [];

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            HubConnection.On(ApplicationConstants.SignalR.ReceiveUpdateDashboard, async () =>
            {
                await LoadDataAsync();
                await InvokeAsync(StateHasChanged);
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
                Model = response.Data;
                BuildCharts();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void BuildCharts()
        {
            // Monthly revenue bar chart
            _revenueBarSeries.Clear();
            foreach (var item in Model.DataEnterBarChart)
            {
                _revenueBarSeries.Add(new ChartSeries { Name = item.Name, Data = item.Data });
            }

            // Daily trend line chart (last 30 days)
            _dailyTrendSeries.Clear();
            if (Model.DailySalesTrend.Count > 0)
            {
                _dailyLabels = Model.DailySalesTrend
                    .Select(d => d.Date.ToString("dd/MM"))
                    .ToArray();

                _dailyTrendSeries.Add(new ChartSeries
                {
                    Name = "CA (FCFA)",
                    Data = Model.DailySalesTrend.Select(d => (double)d.Amount).ToArray()
                });
            }
            else
            {
                _dailyLabels = [];
            }
        }
    }
}