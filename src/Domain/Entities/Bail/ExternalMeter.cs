using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail
{
    public class ExternalMeter : Meter
    {
        public List<ExternalPayement> Payments { get; set; } = new();
    }

}
