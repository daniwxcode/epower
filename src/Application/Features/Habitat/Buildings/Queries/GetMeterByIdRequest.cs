using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries
{
    public class GetMeterByIdRequest : IRequest<Result<MeterResponseBase>>
    {
        public int Id { get; set; }
        public GetMeterByIdRequest(int id)
        {
            Id= id;
        }
    }
    internal class GetMeterByIdRequestHandler : IRequestHandler<GetMeterByIdRequest, Result<MeterResponseBase>>
    {
        private IUnitOfWork<int> _unitOfWork;
        public async Task<Result<MeterResponseBase>> Handle(GetMeterByIdRequest request, CancellationToken cancellationToken)
        {
            var dbItem = await _unitOfWork.Repository<Meter>().GetByIdAsync(request.Id);
            if (dbItem == null)
                return await Result<MeterResponseBase>.FailAsync("Compteur Inexistant");
            return await Result<MeterResponseBase>.SuccessAsync(dbItem.GetMeterResponse());            
        }
    }
}
