using System;
using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Application.Features.Dashboards.Queries.GetData
{
    public class DashboardDataResponse
    {
        // Existing counts
        public int BuildingCount { get; set; }
        public int StoreCount { get; set; }
        public int MeterCount { get; set; }
        public int UserCount { get; set; }
        public int RoleCount { get; set; }

        // Global sales KPIs
        public int SalesCount { get; set; }
        public decimal SalesSum { get; set; }
        public decimal FeesSum { get; set; }

        // Today KPIs
        public int TodaySalesCount { get; set; }
        public decimal TodaySalesSum { get; set; }
        public decimal TodayFeesSum { get; set; }

        // This month KPIs
        public int MonthSalesCount { get; set; }
        public decimal MonthSalesSum { get; set; }

        // Charts
        public List<ChartSeries> DataEnterBarChart { get; set; } = [];
        public double[] MonthlySalesRevenue { get; set; } = new double[12];
        public List<DailySalesPoint> DailySalesTrend { get; set; } = [];

        // Cashier breakdown
        public List<CashierSalesSummary> CashierSales { get; set; } = [];

        // Top meters
        public List<TopMeterSummary> TopMeters { get; set; } = [];
    }

    public class ChartSeries
    {
        public string Name { get; set; }
        public double[] Data { get; set; }
    }

    public record CashierSalesSummary(
        string UserId,
        string FullName,
        int SalesCount,
        decimal SalesAmount,
        decimal FeesAmount,
        int TodaySalesCount,
        decimal TodaySalesAmount);

    public record DailySalesPoint(DateTime Date, decimal Amount, int Count);

    public record TopMeterSummary(string SerialNumber, int SalesCount, decimal TotalAmount);
}