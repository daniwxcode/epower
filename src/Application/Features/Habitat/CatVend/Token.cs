using System.Xml.Serialization;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;

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
