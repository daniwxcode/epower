using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Features.Documents.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Enums;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
        public GetPayementByCriteriaRequestHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginatedResult<PayementResponseBase>> Handle(GetPayementByCriteriaRequest request, CancellationToken cancellationToken)
        {
            PaginatedResult<PayementResponseBase> response = new PaginatedResult<PayementResponseBase>(new List<PayementResponseBase>());
            
            var repo = _unitOfWork.Repository<Payment>().Entities.OrderByDescending(_=>_.CreatedOn);

            Expression<Func<Payment, PayementResponseBase>> expression = e => e.GetPayementResponse();




            response = request.PaymentRequestCriteria switch
            {
                PaymentRequestCriteria.ByMeter => await repo.Where(_ => _.SerialNumber == request.Criteria).Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize),

                PaymentRequestCriteria.ByUser => await repo.Where(_ => _.CreatedBy == request.Criteria).Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize),

                _ => await repo.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize)
            };
            return response;
        }
    }


}
