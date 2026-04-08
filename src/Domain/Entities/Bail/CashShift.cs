using BlazorHero.CleanArchitecture.Domain.Contracts;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail;

/// <summary>
/// Session de caisse : ouverte au début du service, clôturée à la fin.
/// </summary>
public class CashShift : AuditableEntity<int>
{
    [Required]
    public int SellerId { get; set; }

    [ForeignKey(nameof(SellerId))]
    public Seller Seller { get; set; }

    public DateTime OpenedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// Montant en caisse à l'ouverture.
    /// </summary>
    public decimal OpeningBalance { get; set; }

    /// <summary>
    /// Montant en caisse à la clôture (saisi par le vendeur).
    /// </summary>
    public decimal? ClosingBalance { get; set; }

    /// <summary>
    /// Montant total des ventes calculé par le système à la clôture.
    /// </summary>
    public decimal? ExpectedBalance { get; set; }

    /// <summary>
    /// Notes libres (ex. : justification d'écart).
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }

    public CashShiftStatus Status { get; set; } = CashShiftStatus.Open;

    public List<Payment> Payments { get; set; } = [];
    public List<CashRemittance> Remittances { get; set; } = [];
}

public enum CashShiftStatus
{
    Open = 0,
    Closed = 1
}
