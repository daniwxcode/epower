using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Meters.Queries;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Bail
{
    public class MetersController : BaseApiController<MetersController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _mediator.Send(new GetAllMetersRequest());
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> AddStore(AddEditMeterCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
