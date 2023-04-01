using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail
{
    public class InternalMeter : Meter
    {
        public bool IsActive { get; set; } = true;
        public List<Shop> Shops { get; set; } = new();
        public List<InternalPayement> Payements { get; set; } = new();
    }

}
