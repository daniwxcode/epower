using BlazorHero.CleanArchitecture.Domain.Contracts;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail;

/// <summary>
/// Remise de fonds d'un vendeur vers un superviseur à la fin d'un shift.
/// </summary>
public class CashRemittance : AuditableEntity<int>
{
    [Required]
    public int CashShiftId { get; set; }

    [ForeignKey(nameof(CashShiftId))]
    public CashShift CashShift { get; set; }

    /// <summary>
    /// Montant effectivement remis.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// UserId du superviseur qui réceptionne les fonds.
    /// </summary>
    [Required]
    public string ReceivedBy { get; set; }

    public DateTime RemittedAt { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}
