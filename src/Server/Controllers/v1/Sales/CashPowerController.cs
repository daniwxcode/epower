using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;

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
    }
}
