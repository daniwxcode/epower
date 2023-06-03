using BlazorHero.CleanArchitecture.Domain.Contracts;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail
{
    public class Shop : AuditableEntity<int>
    {
        public int BuildingId { get; set; }
        [ForeignKey(nameof(BuildingId))]
        public Building Building { get; set; }

        [Required(ErrorMessage = "Le nom du magasin est requis.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom du magasin doit contenir entre 3 et 50 caractères.")]
        public string Name { get; set; }

        public int? MeterId { get; set; }

        public Meter? Meter { get; set; }       
        public List<RentalAgreement> RentalAgreements { get; set; } = new();
    }

}
