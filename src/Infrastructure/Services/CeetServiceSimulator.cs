using BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend.Requests;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend.Sessions;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;

using Microsoft.Extensions.Logging;

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services;

/// <summary>
/// Local simulator for the CATvend API. Returns realistic fake responses
/// for development and testing when the real API is unavailable.
/// </summary>
public sealed class CeetServiceSimulator : ICeetService
{
    private const double KwhPerFcfa = 0.0556;
    private const int SimulatedDelayMs = 600;

    private readonly ILogger<CeetServiceSimulator> _logger;

    public CeetServiceSimulator(ILogger<CeetServiceSimulator> logger)
    {
        _logger = logger;
    }

    public async Task<CreditVendResponse> BuyCredit(CreditRequest creditRequest)
    {
        ArgumentNullException.ThrowIfNull(creditRequest);

        _logger.LogWarning(
            "[SIMULATEUR] BuyCredit appelé — Compteur: {Serial}, Montant: {Amount} FCFA",
            creditRequest.SerialNumber, creditRequest.Amount);

        await Task.Delay(SimulatedDelayMs);

        if (creditRequest.Amount <= 0)
            return null;

        var token = GenerateToken();
        var kwh = Math.Round(creditRequest.Amount * KwhPerFcfa, 2);
        var bill = $"SIM-{DateTime.UtcNow:yyyyMMddHHmmss}-{creditRequest.SerialNumber}";

        string pattern = @"(\d{4})(\d{4})(\d{4})(\d{4})(\d{4})";
        string replacement = "$1-$2-$3-$4-$5";
        string formatted = Regex.Replace(token, pattern, replacement);

        _logger.LogInformation(
            "[SIMULATEUR] Vente simulée réussie — Code: {Code}, kWh: {Kwh}, Ref: {Bill}",
            formatted, kwh, bill);

        return new CreditVendResponse(true, formatted, kwh, bill, "Vente simulée effectuée avec succès");
    }

    public async Task<ConsumerCheckResponse> ConsumerCheck(ConsumerCheckRequestData consumerCheckRequest)
    {
        ArgumentNullException.ThrowIfNull(consumerCheckRequest);

        _logger.LogWarning(
            "[SIMULATEUR] ConsumerCheck appelé — Compteur: {Meter}, Montant: {Amount}",
            consumerCheckRequest.meter, consumerCheckRequest.amount);

        await Task.Delay(SimulatedDelayMs / 2);

        // Simulate minimum amount of 200 FCFA
        const decimal minimumAmount = 200m;
        var canBuy = consumerCheckRequest.amount >= minimumAmount;

        return new ConsumerCheckResponse(canBuy, minimumAmount);
    }

    public Task Confirm(CreditVendResponse response)
    {
        _logger.LogWarning("[SIMULATEUR] Confirm appelé — Bill: {Bill}", response?.bill);
        return Task.CompletedTask;
    }

    public Task EOS()
    {
        _logger.LogWarning("[SIMULATEUR] End of Session appelé");
        return Task.CompletedTask;
    }

    private static string GenerateToken()
    {
        var rng = Random.Shared;
        var digits = new char[20];
        for (var i = 0; i < digits.Length; i++)
            digits[i] = (char)('0' + rng.Next(0, 10));
        return new string(digits);
    }
}
