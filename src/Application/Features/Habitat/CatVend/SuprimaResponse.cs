using System.Xml.Serialization;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;

[XmlRoot("suprima")]
public class SuprimaResponse
{
    [XmlElement("thin-client")]
    public ThinClient ThinClient { get; set; }
}
