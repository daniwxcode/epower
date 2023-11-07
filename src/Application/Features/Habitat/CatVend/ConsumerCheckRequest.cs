using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;

public record ConsumerCheckRequestData(string meter, decimal amount):IRequest<Result<ConsumerCheckResponse>>;






internal class ConsumerCheckRequestHandler: IRequestHandler<ConsumerCheckRequestData,Result<ConsumerCheckResponse>>
{
    private readonly ICeetService _ceetService;
    public ConsumerCheckRequestHandler(ICeetService ceetService)
    {
        _ceetService = ceetService;
    }
    public async Task<Result<ConsumerCheckResponse>> Handle(ConsumerCheckRequestData request, CancellationToken cancellationToken)
    {
        var data = await _ceetService.ConsumerCheck(request);
        if(data== null)
        return await Result<ConsumerCheckResponse>.FailAsync("No Data");
        if(data.Status)
        return await Result<ConsumerCheckResponse>.SuccessAsync(data);
        return await Result<ConsumerCheckResponse>.FailAsync($"Pas assez d'argent pour acheter: montant min: {data.amount} FCFA") ;
    }
}