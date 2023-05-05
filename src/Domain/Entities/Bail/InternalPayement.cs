using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail
{
    public class InternalPayement : Payment
    {
        [Range(1, int.MaxValue, ErrorMessage = "L'ID du compteur est requis")]
        public int MeterId { get; set; }
        public Meter Meter { get; set; }
    }

}
