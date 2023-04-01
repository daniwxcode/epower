using BlazorHero.CleanArchitecture.Domain.Contracts;

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

}
