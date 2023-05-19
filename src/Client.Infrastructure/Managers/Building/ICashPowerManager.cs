using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Enums;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Building
{
    public interface ICashPowerManager : IManager
    {
        Task<IResult<BuyCreditResponse>> BuyCredit(MakeAPaymentCommand command);
        Task<IResult<List<PayementResponseBase>>> GetPayementByCriteria(PaymentRequestCriteria criteria, string criteriavalue);
        Task<IResult<List<BuildingResponseBase>>> GetAllBuilding();
        Task<IResult<int>> SaveABuildingAsync(AddEditBuildingCommand command);
        Task<IResult<int>> DeleteBuilding(int id);
        Task<IResult<List<ShopResponseBase>>> GetStores(int id = 0);
        Task<IResult<int>> AddStore(AddEditStoreCommand command);
        Task<IResult<int>> DeleteStore(int id);
        Task<IResult<List<MeterResponseBase>>> GetAllMeters(int BuildingId = 0);
        Task<IResult<MeterResponseBase>> GetMeterById(int id);
        Task<IResult<int>> AddMeter(AddEditMeterCommand command);
        Task<IResult<int>> DeleteMeter(int id);
    }
}
