using BlazorHero.CleanArchitecture.Application.Features.Sellers.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Sellers.Queries;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Sales;

[ApiController]
public class CashShiftController : BaseApiController<CashShiftController>
{
    /// <summary>
    /// Ouvre une nouvelle session de caisse pour le vendeur connecté.
    /// </summary>
    [HttpPost("open")]
    [Authorize(Policy = Permissions.CashShifts.Open)]
    public async Task<IActionResult> Open(OpenCashShiftCommand command)
        => Ok(await _mediator.Send(command));

    /// <summary>
    /// Clôture la session de caisse spécifiée.
    /// </summary>
    [HttpPost("close")]
    [Authorize(Policy = Permissions.CashShifts.Close)]
    public async Task<IActionResult> Close(CloseCashShiftCommand command)
        => Ok(await _mediator.Send(command));

    /// <summary>
    /// Retourne la session de caisse active du vendeur connecté.
    /// </summary>
    [HttpGet("active")]
    [Authorize(Policy = Permissions.CashShifts.ViewOwn)]
    public async Task<IActionResult> GetActive()
        => Ok(await _mediator.Send(new GetActiveShiftQuery()));

    /// <summary>
    /// Retourne toutes les sessions de caisse (admin/superviseur).
    /// </summary>
    [HttpGet("all")]
    [Authorize(Policy = Permissions.CashShifts.ViewAll)]
    public async Task<IActionResult> GetAll([FromQuery] bool openOnly = false)
        => Ok(await _mediator.Send(new GetAllCashShiftsQuery { OpenOnly = openOnly }));

    /// <summary>
    /// Enregistre une remise de fonds pour un shift clôturé.
    /// </summary>
    [HttpPost("remittance")]
    [Authorize(Policy = Permissions.Remittances.Create)]
    public async Task<IActionResult> CreateRemittance(CreateCashRemittanceCommand command)
        => Ok(await _mediator.Send(command));
}
