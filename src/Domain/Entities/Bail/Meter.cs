using BlazorHero.CleanArchitecture.Domain.Contracts;

using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail
{
    public abstract class Meter : AuditableEntity<int>
    {
        [Required(ErrorMessage = "Le numéro de série du compteur est requis.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le numéro de série du compteur doit contenir entre 3 et 50 caractères.")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Le code du compteur est requis.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Le code du compteur doit contenir entre 2 et 10 caractères.")]
        public string Code { get; set; }
    }

}
