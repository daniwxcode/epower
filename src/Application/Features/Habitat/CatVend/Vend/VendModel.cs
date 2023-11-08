
using System.Globalization;
using System.Xml.Serialization;
namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend.Vend;

[XmlRoot(ElementName = "suprima")]
public class VendSuprimaResponse
{
    [XmlAttribute(AttributeName = "version")]
    public int Version { get; set; }

    [XmlElement(ElementName = "thin-client")]
    public ThinClient ThinClient { get; set; }
}

public class ThinClient
{
    [XmlElement(ElementName = "vend")]
    public VendResponse Vend { get; set; }
}

public class VendResponse
{
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; }

    [XmlAttribute(AttributeName = "format-by")]
    public string FormatBy { get; set; }

    [XmlElement(ElementName = "success")]
    public int Success { get; set; }

    [XmlElement(ElementName = "token")]
    public Token Token { get; set; }

    [XmlElement(ElementName = "token-count")]
    public int TokenCount { get; set; }
}

public class Token
{
    [XmlElement(ElementName = "tk1")]
    public string Tk1 { get; set; }

    [XmlElement(ElementName = "tk2")]
    public string Tk2 { get; set; }

    [XmlElement(ElementName = "tk3")]
    public string Tk3 { get; set; }

    [XmlElement(ElementName = "tk4")]
    public string Tk4 { get; set; }

    [XmlElement(ElementName = "tk7")]
    public string Tk7 { get; set; }

    [XmlElement(ElementName = "tk10")]
    public string Tk10 { get; set; }

    [XmlElement(ElementName = "tk11")]
    public int Tk11 { get; set; }

    [XmlElement(ElementName = "tk12")]
    public string Tk12 { get; set; }

    [XmlElement(ElementName = "tk14")]
    public string Tk14 { get; set; }

    [XmlElement(ElementName = "tk15")]
    public string Tk15 { get; set; }

    [XmlElement(ElementName = "tk20")]
    public string Tk20 { get; set; }

    [XmlElement(ElementName = "tk21")]
    public string Tk21 { get; set; }

    [XmlElement(ElementName = "tk22")]
    public string Tk22 { get; set; }

    [XmlElement(ElementName = "tk23")]
    public string Tk23 { get; set; }

    [XmlElement(ElementName = "tk30")]
    public string Tk30 { get; set; }

    [XmlElement(ElementName = "tk31")]
    public string Tk31 { get; set; }

    [XmlElement(ElementName = "tk32", IsNullable = true)]
    public string Tk32 { get; set; }

    [XmlElement(ElementName = "tk40")]
    public string Tk40 { get; set; }

    [XmlElement(ElementName = "tk41")]
    public string Tk41 { get; set; }

    [XmlElement(ElementName = "tk43")]
    public string Tk43 { get; set; }

    [XmlElement(ElementName = "tk46")]
    public int Tk46 { get; set; }

    [XmlElement(ElementName = "tk50")]
    public string Tk50 { get; set; }

    [XmlElement(ElementName = "tk60")]
    public string Tk60 { get; set; }
    public double GetTk50AsDouble()
    {
        if (double.TryParse(Tk50, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
        {
            return result;
        }
        if (double.TryParse(Tk50, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"), out double value))
        {
            return value;
        }        
        return 0.0; 
    }

    [XmlElement(ElementName = "tk61")]
    public string Tk61 { get; set; }

    [XmlElement(ElementName = "tk62")]
    public string Tk62 { get; set; }

    [XmlElement(ElementName = "tk63")]
    public string Tk63 { get; set; }

    [XmlElement(ElementName = "tk71")]
    public string Tk71 { get; set; }

    [XmlElement(ElementName = "tk73")]
    public string Tk73 { get; set; }

    [XmlElement(ElementName = "tk80")]
    public string Tk80 { get; set; }

    [XmlElement(ElementName = "tk90")]
    public string Tk90 { get; set; }

    [XmlElement(ElementName = "tk92")]
    public string Tk92 { get; set; }

    [XmlElement(ElementName = "tk95")]
    public string Tk95 { get; set; }
}



