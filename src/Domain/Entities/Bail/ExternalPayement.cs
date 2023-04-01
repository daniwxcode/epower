using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail
{
    public class ExternalPayement : Payment
    {
        public string ClientName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "L'ID du compteur est requis")]
        public int MeterId { get; set; }
        public ExternalMeter Meter { get; set; }
    }

}
