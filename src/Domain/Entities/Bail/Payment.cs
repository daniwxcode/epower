using BlazorHero.CleanArchitecture.Domain.Contracts;

using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail
{
    public class Payment : AuditableEntity<int>
    {
        public decimal Amount { get; set; }
        public decimal BilledAmount { get; set; } = 0;
        public bool IsConfirmed { get; set; } = false;
        public DateTime? ConfirmationDate { get; set; }
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "La référence externe du paiement est requise.")]
        public string ExternalReference { get; set; }
        public string? CreditCode { get; set; }

        [Required(ErrorMessage = "La référence interne du paiement est requise.")]
        public string InternalReference { get; set; }
        public double Credits { get; set; } = 0;

    }

}
