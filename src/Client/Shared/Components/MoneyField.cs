using BlazorHero.CleanArchitecture.Client.Pages.Identity;
using MudBlazor;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BlazorHero.CleanArchitecture.Client.Shared.Components;

public class MoneyTextField<T> : MudTextField<T>
{
    public MoneyTextField()
    {
        Margin = Margin.Dense;
        Format = "N0";
        Adornment = Adornment.End;
        AdornmentText = "FCFA";
        Variant = Variant.Outlined;
        Culture = CultureInfo.GetCultureInfo("de-DE");

    }
}
public class MoneyField : MudField
{
    public MoneyField()
    {
        Margin = Margin.Dense;
        Adornment = Adornment.End;
        AdornmentText = "FCFA";
        Variant = Variant.Text;
    }
}