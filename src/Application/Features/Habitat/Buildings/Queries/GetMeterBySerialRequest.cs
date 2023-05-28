using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
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
    public class GetMeterBySerialRequest : IRequest<Result<MeterResponseBase>>
    {
        public string Serial { get; set; }
        public GetMeterBySerialRequest(string serial)
        {
            Serial= serial;
        }
    }
    internal class GetMeterBySerialRequestHandler : IRequestHandler<GetMeterBySerialRequest, Result<MeterResponseBase>>
    {
        private IUnitOfWork<int> _unitOfWork;
        public async Task<Result<MeterResponseBase>> Handle(GetMeterBySerialRequest request, CancellationToken cancellationToken)
        {
            var dbItem = await _unitOfWork.Repository<Meter>()
                .Entities.Include(_=>_.Building)
                .FirstOrDefaultAsync(_=>_.SerialNumber==request.Serial);
            if (dbItem == null)
                return await Result<MeterResponseBase>.FailAsync("Compteur Inexistant");
            return await Result<MeterResponseBase>.SuccessAsync(dbItem.GetMeterResponse());            
        }
    }
}
