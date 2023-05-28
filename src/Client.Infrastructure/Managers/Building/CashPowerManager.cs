using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Enums;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Routes;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Building
{
    public class CashPowerManager : ICashPowerManager
    {
        private readonly HttpClient _httpClient;
        public CashPowerManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> SaveABuildingAsync(AddEditBuildingCommand command)
        {
            var request = await _httpClient.PostAsJsonAsync(BuildingEndpoints.AddBuilding, command);
            return await request.ToResult<int>();
        }

        public async Task<IResult<List<BuildingResponseBase>>> GetAllBuilding()
        {
            var request = await _httpClient.GetAsync(BuildingEndpoints.GetAllBuildings);
            return await request.ToResult<List<BuildingResponseBase>>();
        }

        public async Task<IResult<int>> DeleteBuilding(int id)
        {
            var request = await _httpClient.DeleteAsync(BuildingEndpoints.DeleteBuilding(id));
            return await request.ToResult<int>();
        }

        public async Task<IResult<List<ShopResponseBase>>> GetStores(int id = 0)
        {
            var url = id > 0 ? BuildingEndpoints.GetBuildingStores(id) : BuildingEndpoints.GetAllStores;
            var response = await _httpClient.GetAsync(url);
            return await response.ToResult<List<ShopResponseBase>>();
        }

        public async Task<IResult<int>> AddStore(AddEditStoreCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(BuildingEndpoints.AddStore, command);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteStore(int id)
        {
            var response = await _httpClient.DeleteAsync(BuildingEndpoints.DeleteStore(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<MeterResponseBase>>> GetAllMeters(int BuildingId = 0)
        {
            string url = BuildingId > 0 ? BuildingEndpoints.GetAllBuldingMeters(BuildingId) : BuildingEndpoints.GetAllMeters;
            var response = await _httpClient.GetAsync(url);
            return await response.ToResult<List<MeterResponseBase>>();

        }

        public async Task<IResult<MeterResponseBase>> GetMeterById(int id)
        {
            var response = await _httpClient.GetAsync(BuildingEndpoints.GetMeterById(id));
            return await response.ToResult<MeterResponseBase>();
        }
        public async Task<IResult<MeterResponseBase>> GetMeterBySerial(string strial)
        {
            var response = await _httpClient.GetAsync(BuildingEndpoints.GetMeterBySerial(strial));
            return await response.ToResult<MeterResponseBase>();
        }

        public async Task<IResult<int>> AddMeter(AddEditMeterCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(BuildingEndpoints.AddMeter, command);
            return await response.ToResult<int>();
        }

        public Task<IResult<int>> DeleteMeter(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IResult<BuyCreditResponse>> BuyCredit(MakeAPaymentCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(BuildingEndpoints.MakePayement, command);
            return await response.ToResult<BuyCreditResponse>();
        }

        public async Task<PaginatedResult<PayementResponseBase>> GetPayementByCriteria(PaymentRequestCriteria criteria, string criteriavalue, int pageNumber, int pageSize)
        {
            var url = criteria switch
            {
                PaymentRequestCriteria.ByMeter => BuildingEndpoints.GetAllPayementByMeter(criteriavalue),
                PaymentRequestCriteria.ByUser =>
                criteriavalue == string.Empty ? BuildingEndpoints.CurrentUserSales : BuildingEndpoints.GetAllUserSales(criteriavalue),
                _ => BuildingEndpoints.GetAllPayement
            };
            var response = await _httpClient.GetAsync(url+ $"?pageNumber={pageNumber+1}&pageSize={pageSize}");
            return await response.ToPaginatedResult<PayementResponseBase>();
        }
    }
}
