using BlazorHero.CleanArchitecture.Domain.Entities.Bail;

using System.Linq;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;

public record BuildingResponseBase(int Id, string name, string Address);

public record MeterResponseBase(int Id,string SerialNumber, string code, bool IsActive);

public record BuildingWithMeters(int Id, string name, string Address) : BuildingResponseBase(Id, name, Address);
public record ShopResponseBase(int Id,int buildingId,string BuildingName, string Name);

public static partial class DataConverter
{
    public static BuildingResponseBase GetBuildingsResponse(this Building building) =>
        new BuildingResponseBase(building.Id, building.Name, building.Address);
    public static MeterResponseBase GetMeterResponse(this Meter meter) => new MeterResponseBase(meter.Id, meter.SerialNumber, meter.Code, meter.IsActive);
    public static ShopResponseBase GetShopResponse(this Shop shop) =>
        new ShopResponseBase(shop.Id, shop.BuildingId, shop.Building.Name, shop.Name);
}