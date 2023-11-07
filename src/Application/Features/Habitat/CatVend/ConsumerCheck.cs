using System.Xml.Serialization;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;

public class ConsumerCheck
{
    [XmlElement("success")]
    public int Success { get; set; }

    [XmlElement("token")]
    public Token Token { get; set; }
}
