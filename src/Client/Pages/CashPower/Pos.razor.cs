using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;

using System;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.CashPower
{
    public partial class Pos
    {
        private MakeAPaymentCommand model;
        private Decimal Amount;
        private string SerialNumber;


        public Task SubmitPayement()
        {
            return Task.CompletedTask;
        }
    }
}
