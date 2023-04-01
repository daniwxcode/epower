using BlazorHero.CleanArchitecture.Domain.Contracts;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail
{
    public class Building : AuditableEntity<int>
    {
        [Required(ErrorMessage = "Le nom du bâtiment est requis.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom du bâtiment doit contenir entre 3 et 50 caractères.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "L'adresse du bâtiment est requise.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "L'adresse du bâtiment doit contenir entre 5 et 100 caractères.")]
        public string Address { get; set; }

        public List<Shop> Shops { get; set; } = new();
    }

    public class Shop : AuditableEntity<int>
    {
        public int BuildingId { get; set; }

        public Building Building { get; set; }

        [Required(ErrorMessage = "Le nom du magasin est requis.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom du magasin doit contenir entre 3 et 50 caractères.")]
        public string Name { get; set; }

        public int MeterId { get; set; }

        public Meter Meter { get; set; }
        public List<InternalPayement> Payments { get; set; } = new();
        public List<RentalAgreement> RentalAgreements { get; set; } = new();
    }
    public abstract class Meter : AuditableEntity<int>
    {
        [Required(ErrorMessage = "Le numéro de série du compteur est requis.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le numéro de série du compteur doit contenir entre 3 et 50 caractères.")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Le code du compteur est requis.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Le code du compteur doit contenir entre 2 et 10 caractères.")]
        public string Code { get; set; }
    }
    public abstract class Payment : AuditableEntity<int>
    {
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "La référence externe du paiement est requise.")]
        public string ExternalReference { get; set; }

        [Required(ErrorMessage = "La référence interne du paiement est requise.")]
        public string InternalReference { get; set; }
        public double Credits { get; set; }

    }
    public class InternalMeter : Meter
    {
        public bool IsActive { get; set; } = true;
        public List<Shop> Shops { get; set; } = new();
        public List<InternalPayement> Payements { get; set; } = new();
    }
    public class ExternalMeter : Meter
    {
        public List<ExternalPayement> Payments { get; set; } = new();
    }

    public class InternalPayement : Payment
    {
        public int MeterId { get; set; }
        public InternalMeter Meter { get; set; }
    }
    public class ExternalPayement : Payment
    {
        public string ClientName { get; set; }
        public int MeterId { get; set; }
        public ExternalMeter Meter { get; set; }
    }

    public class ShopTenant : AuditableEntity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public List<RentalAgreement> RentalAgreements { get; set; } = new();
    }
    public class RentalAgreement : AuditableEntity<int>
    {
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public int ShopTenantId { get; set; }
        public ShopTenant ShopTenant { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double StartDateCounterValue { get; set; }
        public double? EndDateCounterValue { get; set; }
    }

}
