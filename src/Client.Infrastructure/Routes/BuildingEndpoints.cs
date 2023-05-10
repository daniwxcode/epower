namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class BuildingEndpoints
    {
        public static string GetAllBuildings = "api/v1/Building/all";
        public static string AddBuilding = "api/v1/Building/add";
        public static string GetAllMeters = "api/v1/Building/all-meters";
        public static string GetAllBulding(int id) => $"api/v1/Building/{id}/meters";
        public static string AddMeter = "api/v1/Building/add-meter";
        public static string GetAllStores = "api/v1/Building/stores";
        public static string AddStore = "api/v1/Building/add-store";
        public static string AddStoreTenant = "api/v1/Building/createtenant";
        public static string createRentalAgreement = "api/v1/Building/createRentalAgreement";
        public static string SellCashPower = "api/v1/CashPower";

    }
}
