using BlazorHero.CleanArchitecture.Domain.Contracts;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail;

/// <summary>
/// Profil métier d'un vendeur ambulant, lié à un utilisateur Identity.
/// </summary>
public class Seller : AuditableEntity<int>
{
    [Required]
    public string UserId { get; set; }

    [StringLength(100)]
    public string? Zone { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Plafond de crédit autorisé par session (0 = illimité).
    /// </summary>
    public decimal MaxCreditLimit { get; set; }

    public List<CashShift> CashShifts { get; set; } = [];
}
