using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Sales
{

    [ApiController]
    public class CashPowerController : BaseApiController<CashPowerController>
    {
        [HttpPost]
        public async Task<IActionResult> SellCredit(MakeAPaymentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetSales(GetPayementByCriteriaRequest request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
