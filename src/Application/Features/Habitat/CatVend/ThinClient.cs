using System.Xml.Serialization;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;

public class ThinClient
{
    [XmlElement("consumer-chk")]
    public ConsumerCheck ConsumerCheck { get; set; }
}
