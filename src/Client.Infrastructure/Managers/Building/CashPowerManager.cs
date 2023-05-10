using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Routes;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using System.Collections.Generic;
using System.Net.Http;
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
        public Task<IResult<int>> SaveABuildingAsync(AddEditBuildingCommand command)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IResult<List<BuildingResponseBase>>> GetAllBuilding()
        {
            var request = await _httpClient.GetAsync(BuildingEndpoints.GetAllBuildings);
            return await request.ToResult<List<BuildingResponseBase>>();
        }
    }
}
