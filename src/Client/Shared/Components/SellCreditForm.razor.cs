using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;

namespace BlazorHero.CleanArchitecture.Client.Shared.Components
{
    public partial class SellCreditForm
    {
        public MakeAPaymentCommand Model { get; set; } = new();
    }
}
