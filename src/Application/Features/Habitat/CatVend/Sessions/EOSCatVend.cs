using System.Xml.Serialization;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend.Sessions;
[XmlRoot("catvend")]
public class EOSCatVend
{
    [XmlElement("unit")]
    public string Unit { get; set; }

    [XmlElement("validation_code")]
    public string ValidationCode { get; set; }

    [XmlElement("username")]
    public string Username { get; set; }

    [XmlElement("password")]
    public string Password { get; set; }
}


