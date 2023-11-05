using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;

public record ConsumerCheckRequestData(string meter, decimal amount):IRequest<Result<ConsumerCheckResponse>>;


[XmlRoot(ElementName = "catvend")]
public record ConsumerCheckRequest
{
    [XmlElement(ElementName = "validation_code")]
    public string Validation_code { get; set; }
    [XmlElement(ElementName = "unit")]
    public string Unit { get; set; }
    [XmlElement(ElementName = "username")]
    public string Username { get; set; }
    [XmlElement(ElementName = "password")]
    public string Password { get; set; }
    [XmlElement(ElementName = "meter")]
    public string Meter { get; set; }
    [XmlElement(ElementName = "amount")]
    public decimal Amount { get; set; }
}

[XmlRoot("suprima")]
public class SuprimaResponse
{
    [XmlElement("thin-client")]
    public ThinClient ThinClient { get; set; }
}

public class ThinClient
{
    [XmlElement("consumer-chk")]
    public ConsumerCheck ConsumerCheck { get; set; }
}

public class ConsumerCheck
{
    [XmlElement("success")]
    public int Success { get; set; }

    [XmlElement("token")]
    public Token Token { get; set; }
}

public class Token
{
    [XmlElement("tk3")]
    public string Name { get; set; }

    [XmlElement("tk4")]
    public string Details { get; set; }

    [XmlElement("tk9")]
    public string Identifier { get; set; }

    [XmlElement("tk20")]
    public string AnotherIdentifier { get; set; }

    [XmlElement("tk22")]
    public string Contact { get; set; }

    [XmlElement("tk30")]
    public decimal Amount { get; set; }

    [XmlElement("tk40")]
    public int Number { get; set; }

    [XmlElement("tk41")]
    public string Code { get; set; }

    [XmlElement("tk42")]
    public int StatusCode { get; set; }

    [XmlElement("tk43")]
    public int AnotherStatusCode { get; set; }

    [XmlElement("tk44")]
    public int YetAnotherStatusCode { get; set; }

    [XmlElement("tk51")]
    public decimal AnotherAmount { get; set; }
}

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