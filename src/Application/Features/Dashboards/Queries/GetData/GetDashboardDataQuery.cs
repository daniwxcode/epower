using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;
using BlazorHero.CleanArchitecture.Domain.Entities.Misc;
using Microsoft.Extensions.Localization;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;

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

        public GetDashboardDataQueryHandler(IUnitOfWork<int> unitOfWork, IUserService userService, IRoleService roleService, IStringLocalizer<GetDashboardDataQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _roleService = roleService;
            _localizer = localizer;
        }

        public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
        {
            var response = new DashboardDataResponse
            {
                BuildingCount = await _unitOfWork.Repository<Building>().Entities.CountAsync(cancellationToken),
                MeterCount = await _unitOfWork.Repository<Meter>().Entities.CountAsync(cancellationToken),
                StoreCount = await _unitOfWork.Repository<Shop>().Entities.CountAsync(cancellationToken),
                SalesCount = await _unitOfWork.Repository<Payment>().Entities.CountAsync(cancellationToken),                
                SalesSum =(int) await _unitOfWork.Repository<Payment>().Entities.SumAsync(_=>_.Amount,cancellationToken),                
                UserCount = await _userService.GetCountAsync(),
                RoleCount = await _roleService.GetCountAsync()
            };

            var selectedYear = DateTime.Now.Year;
            double[] buildingFigure = new double[13];
            double[] storeFigure = new double[13];
            double[] meterFigure = new double[13];
            double[] payementFigure = new double[13];
           
            for (int i = 1; i <= 12; i++)
            {
                var month = i;
                var filterStartDate = new DateTime(selectedYear, month, 01);
                var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59); // Monthly Based

                buildingFigure[i - 1] = await _unitOfWork.Repository<Building>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);

                storeFigure[i - 1] = await _unitOfWork.Repository<Shop>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                meterFigure[i - 1] = await _unitOfWork.Repository<Meter>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                payementFigure[i - 1] = await _unitOfWork.Repository<Payment>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);               
            }

            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Immeubles"], Data = buildingFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Boutiques"], Data = storeFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Compteurs"], Data = meterFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Ventes"], Data = payementFigure });          

            return await Result<DashboardDataResponse>.SuccessAsync(response);
        }
    }
}