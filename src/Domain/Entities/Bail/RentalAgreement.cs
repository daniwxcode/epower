using BlazorHero.CleanArchitecture.Domain.Contracts;

using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail
{
    public class RentalAgreement : AuditableEntity<int>
    {
        [Range(1, int.MaxValue, ErrorMessage = "L'ID de la boutique est requis")]
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "L'ID du Locataire est Exigé")]
        public int ShopTenantId { get; set; }
        public ShopTenant ShopTenant { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double StartDateCounterValue { get; set; } = 0.0;
        public double? EndDateCounterValue { get; set; }
    }

}
