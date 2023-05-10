using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Building
{
    public interface ICashPowerManager : IManager
    {
        Task<IResult<int>> SaveABuildingAsync(AddEditBuildingCommand command);
        Task<IResult<List<BuildingResponseBase>>> GetAllBuilding();
    }
}
