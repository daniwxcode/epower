using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Meters.Queries;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Bail
{
    public class BuildingController:BaseApiController<BuildingController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _mediator.Send(new GetAllBuildingsRequest());
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> AddBuilding(AddEditBuildingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpGet("all-meters")]
        public async Task<IActionResult> GetAllMeters()
        {
            var data = await _mediator.Send(new GetAllMetersRequest());
            return Ok(data);
        }
        [HttpGet("building-meters//{id}")]
        public async Task<IActionResult> GetAllByBuilding(int id)
        {
            var data = await _mediator.Send(new GetAllMetersByBuildingIdRequest(id));
            return Ok(data);
        }
        [HttpGet("meters/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _mediator.Send(new GetMeterByIdRequest(id));
            return Ok(data);
        }
        [HttpPost("add-meter")]
        public async Task<IActionResult> AddStore(AddEditMeterCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpGet("stores")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _mediator.Send(new GetAllShopRequest());
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> AddStore(AddEditStoreCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

    }
}
