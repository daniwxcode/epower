using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;

using System;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;

public record BuildingResponseBase(int Id, string name, string Address);
public record MeterResponseBase(int Id, string SerialNumber, string code, bool IsActive);
public record BuildingWithMeters(int Id, string name, string Address) : BuildingResponseBase(Id, name, Address);
public record ShopResponseBase(int Id, int buildingId, string BuildingName, string Name, MeterResponseBase Meter);
public record PayementResponseBase(int Id,string internalref, DateTime Date, decimal Amount, string MeterSerial, int? MeterId, string Agent);
public record BuyCreditResponse(int Id, int Amount, string SerialNumber, string Reference, DateTime date, string InternalReference, string Code, decimal Credit, string Seller)
{
    public string Message() => $"Vous avez payé {Amount} FCFA de crédit CASH POWER (Ref: {InternalReference} ce {date.ToLongDateString()}. Le Code de Votre paiement est : {Code}, Compteur: {SerialNumber} ";
}
public static partial class DataConverter
{
    public static BuildingResponseBase GetBuildingsResponse(this Building building) =>
        new BuildingResponseBase(building.Id, building.Name, building.Address);
    public static MeterResponseBase GetMeterResponse(this Meter meter) => new MeterResponseBase(meter.Id, meter.SerialNumber, meter.Code, meter.IsActive);
    public static ShopResponseBase GetShopResponse(this Shop shop) =>
        new ShopResponseBase(shop.Id, shop.BuildingId, shop.Building.Name, shop.Name, shop.Meter.GetMeterResponse());
    public async static Task<PayementResponseBase> GetPayementResponse(this Payment payment, IUserService userService)
    {
        var user = await userService.GetAsync(payment.CreatedBy);

        return new PayementResponseBase(payment.Id,payment.InternalReference, payment.CreatedOn, payment.Amount, payment.SerialNumber,null, user.Data.UserFullName);
    }
    public async static Task<PayementResponseBase> GetPayementResponse(this Payment payment, int meterId,IUserService userService)
    {
        var user = await userService.GetAsync(payment.CreatedBy);
        return new PayementResponseBase(payment.Id, payment.InternalReference, payment.CreatedOn, payment.Amount, payment.SerialNumber, meterId, user.Data.UserFullName);
    }
    public async static Task<PayementResponseBase> GetPayementResponse(this InternalPayement p, IUserService userService) => await p.GetPayementResponse(p.MeterId, userService);


}