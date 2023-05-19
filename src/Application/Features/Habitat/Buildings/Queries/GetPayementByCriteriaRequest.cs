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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries
{
    public class GetPayementByCriteriaRequest : IRequest<Result<List<PayementResponseBase>>>
    {
        public int BuildingId { get; set; } = 0;
        public string Criteria { get; set; } = string.Empty;
        public PaymentRequestCriteria PaymentRequestCriteria { get; set; }
    }
    internal class GetPayementByCriteriaRequestHandler : IRequestHandler<GetPayementByCriteriaRequest, Result<List<PayementResponseBase>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        public GetPayementByCriteriaRequestHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<PayementResponseBase>>> Handle(GetPayementByCriteriaRequest request, CancellationToken cancellationToken)
        {
            List<PayementResponseBase> response = new();
            var repo = _unitOfWork.Repository<Payment>().Entities;
            response = request.PaymentRequestCriteria switch
            {
                PaymentRequestCriteria.ByMeter => await repo.Where(_ => _.SerialNumber == request.Criteria).Select(_ => _.GetPayementResponse()).ToListAsync(),
                PaymentRequestCriteria.ByUser => await repo.Where(_ => _.CreatedBy == request.Criteria).Select(_ => _.GetPayementResponse()).ToListAsync(),

                _ => await repo.Select(_ => _.GetPayementResponse()).ToListAsync()
            };
            return await Result<List<PayementResponseBase>>.SuccessAsync(response);
        }
    }


}
