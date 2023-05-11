using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
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
            HttpResponseMessage response=new();
            if (id > 0)
            {
                 response = await _httpClient.GetAsync(BuildingEndpoints.GetBuildingStores(id));
                return await response.ToResult<List<ShopResponseBase>>();
            }
            response = await _httpClient.GetAsync(BuildingEndpoints.GetAllStores);
            return await response.ToResult<List<ShopResponseBase>>();
        }
    }
}
