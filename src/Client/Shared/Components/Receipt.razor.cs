using Microsoft.AspNetCore.Components;

namespace BlazorHero.CleanArchitecture.Client.Shared.Components
{
    public partial class Receipt
    {
        [Parameter]
        public string Serial { get; set; } = "000000000000";
        [Parameter]
        public decimal Amount { get; set; } = 0;
        private int Total { get { return (int)Amount + getFees(); }}
        private int getFees()
        {
            return (int)Amount switch
            {
                <=0 => 0,
                > 1_000_000  => 800,
                > 300_000 => 600,
                > 50_000 => 400,
                > 5_000 => 200,
                _=> 150
            };
        }
    }
}
