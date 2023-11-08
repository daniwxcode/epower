using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Features.Documents.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Enums;
using BlazorHero.CleanArchitecture.Application.Features.Products.Queries.GetAllPaged;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Application.Specifications.Catalog;
using BlazorHero.CleanArchitecture.Application.Specifications.Payements;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
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
        private IUserService _userService;
        public GetPayementByCriteriaRequestHandler(IUnitOfWork<int> unitOfWork,IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        public async Task<PaginatedResult<PayementResponseBase>> Handle(GetPayementByCriteriaRequest request, CancellationToken cancellationToken)
        {
            
            var repo = _unitOfWork.Repository<Payment>().Entities.OrderByDescending(_=>_.CreatedOn);

            Expression<Func<Payment, PayementResponseBase>> expression =  e =>  e.GetPayementResponse(_userService).Result;

            var payment = request.PaymentRequestCriteria switch
            {              
                PaymentRequestCriteria.ByUser => new PaymentFilterSpecification(string.Empty,request.Criteria),

                _ => new PaymentFilterSpecification(request.Criteria)
            };
          var response = await repo.Specify(payment).Select(expression).ToPaginatedListAsync(request.PageNumber,request.PageSize);
            return response;
           
        }
    }


}
