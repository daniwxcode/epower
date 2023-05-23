namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class BuildingEndpoints
    {

        public static string MakePayement = "/api/v1/CashPower";
        public static string GetAllPayement = "/api/v1/CashPower/all-payement";
        public static string GetAllPayementByMeter(string meterid) => $"/api/v1/CashPower/all-payement/{meterid}";
        public static string GetAllUserSales(string userid) => $"/api/v1/CashPower/all-payement/byuser/{userid}";
        public static string CurrentUserSales => "/api/v1/CashPower/all-payement/mysales";




        public static string GetAllBuildings = "api/v1/Building/all";
        public static string AddBuilding = "api/v1/Building/add";
        public static string DeleteBuilding(int id) => $"api/v1/Building/Delete/{id}";

        public static string GetAllMeters = "api/v1/Building/all-meters";
        public static string AddMeter = "api/v1/Building/add-meter";

        public static string GetAllBuldingMeters(int id) => $"api/v1/Building/{id}/meters";
        public static string GetMeterById(int id) => $"api/v1/Building/GetMeter/{id}";
        public static string GetMeterBySerial(string serial) => $"api/v1/Building/GetMeter-by-serial/{serial}";

        public static string GetAllStores = "api/v1/Building/stores";
        public static string AddStore = "api/v1/Building/add-store";
        public static string GetBuildingStores(int id) => $"api/v1/Building/store-by-building/{id}";
        public static string DeleteStore(int id) => $"api/v1/Building/delete-store/{id}";
        public static string AddStoreTenant = "api/v1/Building/createtenant";
        public static string createRentalAgreement = "api/v1/Building/createRentalAgreement";

        public static string SellCashPower = "api/v1/CashPower";

    }
}
