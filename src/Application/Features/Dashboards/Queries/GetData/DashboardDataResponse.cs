using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Application.Features.Dashboards.Queries.GetData
{
    public class DashboardDataResponse
    {
        public int BuildingCount { get; set; }
        public int StoreCount { get; set; }
        public int MeterCount { get; set; }
        public int SalesCount { get; set; }
        public int SalesSum { get; set; }       
        public int UserCount { get; set; }
        public int RoleCount { get; set; }
        public List<ChartSeries> DataEnterBarChart { get; set; } = new();
        public Dictionary<string, double> MeterByBuildingPieChart { get; set; }
    }

    public class ChartSeries
    {
        public ChartSeries() { }

        public string Name { get; set; }
        public double[] Data { get; set; }
    }

}