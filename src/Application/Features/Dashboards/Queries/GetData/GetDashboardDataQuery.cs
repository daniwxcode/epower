using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.Dashboards.Queries.GetData
{
    public class GetDashboardDataQuery : IRequest<Result<DashboardDataResponse>>
    {
    }

    internal class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<GetDashboardDataQueryHandler> _localizer;

        public GetDashboardDataQueryHandler(
            IUnitOfWork<int> unitOfWork,
            IUserService userService,
            IRoleService roleService,
            IStringLocalizer<GetDashboardDataQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _roleService = roleService;
            _localizer = localizer;
        }

        public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
        {
            var payments = _unitOfWork.Repository<Payment>().Entities;
            var now = DateTime.UtcNow;
            var todayStart = now.Date;
            var monthStart = new DateTime(now.Year, now.Month, 1);

            // ── Basic counts ──
            var response = new DashboardDataResponse
            {
                BuildingCount = await _unitOfWork.Repository<Building>().Entities.CountAsync(cancellationToken),
                MeterCount = await _unitOfWork.Repository<Meter>().Entities.CountAsync(cancellationToken),
                StoreCount = await _unitOfWork.Repository<Shop>().Entities.CountAsync(cancellationToken),
                UserCount = await _userService.GetCountAsync(),
                RoleCount = await _roleService.GetCountAsync(),

                // Global sales
                SalesCount = await payments.CountAsync(cancellationToken),
                SalesSum = await payments.SumAsync(p => p.Amount, cancellationToken),
                FeesSum = await payments.SumAsync(p => p.BilledAmount, cancellationToken),

                // Today
                TodaySalesCount = await payments.CountAsync(p => p.CreatedOn >= todayStart, cancellationToken),
                TodaySalesSum = await payments.Where(p => p.CreatedOn >= todayStart).SumAsync(p => p.Amount, cancellationToken),
                TodayFeesSum = await payments.Where(p => p.CreatedOn >= todayStart).SumAsync(p => p.BilledAmount, cancellationToken),

                // This month
                MonthSalesCount = await payments.CountAsync(p => p.CreatedOn >= monthStart, cancellationToken),
                MonthSalesSum = await payments.Where(p => p.CreatedOn >= monthStart).SumAsync(p => p.Amount, cancellationToken),
            };

            // ── Monthly revenue chart (current year) ──
            var selectedYear = now.Year;
            double[] monthlySalesRevenue = new double[12];
            double[] monthlySalesCount = new double[12];

            var monthlyData = await payments
                .Where(p => p.CreatedOn.Year == selectedYear)
                .GroupBy(p => p.CreatedOn.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(p => (double)p.Amount), Count = g.Count() })
                .ToListAsync(cancellationToken);

            foreach (var m in monthlyData)
            {
                monthlySalesRevenue[m.Month - 1] = m.Total;
                monthlySalesCount[m.Month - 1] = m.Count;
            }
            response.MonthlySalesRevenue = monthlySalesRevenue;

            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Chiffre d'affaires (FCFA)"], Data = monthlySalesRevenue });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Nombre de ventes"], Data = monthlySalesCount });

            // ── Daily trend (last 30 days) ──
            var thirtyDaysAgo = todayStart.AddDays(-29);
            var dailyData = await payments
                .Where(p => p.CreatedOn >= thirtyDaysAgo)
                .GroupBy(p => p.CreatedOn.Date)
                .Select(g => new { Date = g.Key, Amount = g.Sum(p => p.Amount), Count = g.Count() })
                .OrderBy(g => g.Date)
                .ToListAsync(cancellationToken);

            response.DailySalesTrend = dailyData
                .Select(d => new DailySalesPoint(d.Date, d.Amount, d.Count))
                .ToList();

            // ── Cashier breakdown ──
            var cashierGroups = await payments
                .GroupBy(p => p.CreatedBy)
                .Select(g => new
                {
                    UserId = g.Key,
                    SalesCount = g.Count(),
                    SalesAmount = g.Sum(p => p.Amount),
                    FeesAmount = g.Sum(p => p.BilledAmount),
                    TodaySalesCount = g.Count(p => p.CreatedOn >= todayStart),
                    TodaySalesAmount = g.Where(p => p.CreatedOn >= todayStart).Sum(p => p.Amount),
                })
                .OrderByDescending(g => g.SalesAmount)
                .ToListAsync(cancellationToken);

            var usersResult = await _userService.GetAllAsync();
            var usersLookup = usersResult.Succeeded
                ? usersResult.Data.ToDictionary(u => u.Id, u => u.UserFullName)
                : new Dictionary<string, string>();

            response.CashierSales = cashierGroups
                .Select(g => new CashierSalesSummary(
                    g.UserId,
                    usersLookup.GetValueOrDefault(g.UserId, "Inconnu"),
                    g.SalesCount,
                    g.SalesAmount,
                    g.FeesAmount,
                    g.TodaySalesCount,
                    g.TodaySalesAmount))
                .ToList();


            // ── Top meters ──
            var topMetersData = await payments
                .GroupBy(p => p.SerialNumber)
                .Select(g => new { SerialNumber = g.Key, SalesCount = g.Count(), TotalAmount = g.Sum(p => p.Amount) })
                .OrderByDescending(t => t.TotalAmount)
                .Take(5)
                .ToListAsync(cancellationToken);

            response.TopMeters = topMetersData
                .Select(t => new TopMeterSummary(t.SerialNumber, t.SalesCount, t.TotalAmount))
                .ToList();

            return await Result<DashboardDataResponse>.SuccessAsync(response);
        }
    }
}