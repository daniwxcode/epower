using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;


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
[XmlRoot(ElementName = "catvend")]
public record EVendRequest : ConsumerCheckRequest
{
    [XmlElement(ElementName = "customer_phone")]
    public string Phone { get; set; }
}