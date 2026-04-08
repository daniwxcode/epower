using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Enums;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Application.Specifications.Payements;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries
{
    public class GetPayementByCriteriaRequest : IRequest<PaginatedResult<PayementResponseBase>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int BuildingId { get; set; } = 0;
        public string Criteria { get; set; } = string.Empty;
        public PaymentRequestCriteria PaymentRequestCriteria { get; set; }
        public GetPayementByCriteriaRequest(int pageNumber, int pageSize, int buildingId = 0, string criteria = "")
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            BuildingId = buildingId;
            Criteria = criteria;

        }
    }
    internal class GetPayementByCriteriaRequestHandler : IRequestHandler<GetPayementByCriteriaRequest, PaginatedResult<PayementResponseBase>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private IUserService _userService;
        public GetPayementByCriteriaRequestHandler(IUnitOfWork<int> unitOfWork,IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        public async Task<PaginatedResult<PayementResponseBase>> Handle(GetPayementByCriteriaRequest request, CancellationToken cancellationToken)
        {
            
            var repo = _unitOfWork.Repository<Payment>().Entities.OrderByDescending(_=>_.CreatedOn);

            var paymentSpec = request.PaymentRequestCriteria switch
            {              
                PaymentRequestCriteria.ByUser => new PaymentFilterSpecification(string.Empty,request.Criteria),

                _ => new PaymentFilterSpecification(request.Criteria)
            };

            var paginatedPayments = await repo
                .Specify(paymentSpec)
                .Select(e => new
                {
                    e.Id,
                    e.InternalReference,
                    e.CreatedOn,
                    e.Amount,
                    e.BilledAmount,
                    e.SerialNumber,
                    e.CreatedBy
                })
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            var userIds = paginatedPayments.Data.Select(p => p.CreatedBy).Where(id => id != null).Distinct().ToList();
            var usersResult = await _userService.GetAllAsync();
            var usersLookup = usersResult.Succeeded
                ? usersResult.Data
                    .Where(u => userIds.Contains(u.Id))
                    .ToDictionary(u => u.Id, u => u.UserFullName)
                : new Dictionary<string, string>();

            var mapped = paginatedPayments.Data
                .Select(p => new PayementResponseBase(
                    p.Id,
                    p.InternalReference,
                    p.CreatedOn,
                    p.Amount,
                    p.BilledAmount,
                    p.SerialNumber,
                    null,
                    usersLookup.GetValueOrDefault(p.CreatedBy, "Inconnu")))
                .ToList();

            return PaginatedResult<PayementResponseBase>.Success(mapped, paginatedPayments.TotalCount, paginatedPayments.CurrentPage, request.PageSize);
        }
    }


}
