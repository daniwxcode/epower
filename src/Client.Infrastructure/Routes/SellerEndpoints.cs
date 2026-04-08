namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes;

public static class SellerEndpoints
{
    public static string GetAll = "api/v1/Seller";
    public static string AddEdit = "api/v1/Seller";
}

public static class CashShiftEndpoints
{
    public static string Open = "api/v1/CashShift/open";
    public static string Close = "api/v1/CashShift/close";
    public static string GetActive = "api/v1/CashShift/active";
    public static string GetAll = "api/v1/CashShift/all";
    public static string GetAllOpen = "api/v1/CashShift/all?openOnly=true";
    public static string CreateRemittance = "api/v1/CashShift/remittance";
}
