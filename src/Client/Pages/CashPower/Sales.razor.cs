using BlazorHero.CleanArchitecture.Application.Features.Documents.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Enums;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

using MudBlazor;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.CashPower
{
    public partial class Sales
    {
        [Parameter]
        public int Criteria { get; set; } = 0;
        [Parameter]
        public string CriteriaValue { get; set; } = string.Empty;
       
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        //private List<PayementResponseBase> _List = new();
        private string _searchString = "";
        private string CurrentUserId { get; set; }
        private int _totalItems;
        private int _currentPage;
        private IEnumerable<PayementResponseBase> _pagedData;
        private MudTable<PayementResponseBase> _table;
        private ClaimsPrincipal _currentUser;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task<TableData<PayementResponseBase>> ServerReload(TableState state, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = state.Page;
                state.PageSize = state.PageSize;
            }
            await GetDataAsync(state.Page, state.PageSize, state);
            return new TableData<PayementResponseBase> { TotalItems = _totalItems, Items = _pagedData };
        }
        private async Task GetDataAsync(int pageNumber, int pageSize, TableState state)
        {
            PaymentRequestCriteria criteria = (PaymentRequestCriteria)Criteria;
            if (CriteriaValue == string.Empty)
                CriteriaValue = _searchString;
            var response = await _cashPowerManager.GetPayementByCriteria(criteria, CriteriaValue, pageNumber,pageSize);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                var data = response.Data.Where(_=>Search(_)).ToList();
                _pagedData = data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }
        public void Print(int id)
        {
            _navigationManager.NavigateTo(ApplicationConstants.FileConstants.GetReceipt(id),true);
        }
        private bool Search(PayementResponseBase item)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (item.MeterSerial?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.Agent?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }



    }

}
