using System;
using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Application.Features.Sellers.DTOs;

public record SellerResponse(
    int Id,
    string UserId,
    string FullName,
    string? Zone,
    string? PhoneNumber,
    bool IsActive,
    decimal MaxCreditLimit);

public record CashShiftResponse(
    int Id,
    int SellerId,
    string SellerName,
    DateTime OpenedAt,
    DateTime? ClosedAt,
    decimal OpeningBalance,
    decimal? ClosingBalance,
    decimal? ExpectedBalance,
    string? Notes,
    string Status,
    int SalesCount,
    decimal SalesTotal);

public record CashRemittanceResponse(
    int Id,
    int CashShiftId,
    decimal Amount,
    string ReceivedBy,
    string ReceivedByName,
    DateTime RemittedAt,
    string? Notes);

public record ActiveShiftSummary(
    int ShiftId,
    DateTime OpenedAt,
    decimal OpeningBalance,
    int SalesCount,
    decimal SalesTotal,
    decimal FeesTotal);
