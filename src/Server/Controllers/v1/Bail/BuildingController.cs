using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Meters.Queries;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Bail
{
    public class BuildingController : BaseApiController<BuildingController>
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _mediator.Send(new GetAllBuildingsRequest());
            return Ok(data);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddBuilding(AddEditBuildingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBuilding(int id)
        {
            return Ok(await _mediator.Send(new DeleteBuildingCommand(id)));
        }

        [HttpGet("all-meters")]
        public async Task<IActionResult> GetAllMeters()
        {
            var data = await _mediator.Send(new GetAllMetersRequest());
            return Ok(data);
        }
        [HttpGet("{id}/meters")]
        public async Task<IActionResult> GetAllByBuilding(int id)
        {
            var data = await _mediator.Send(new GetAllMetersByBuildingIdRequest(id));
            return Ok(data);
        }
        [HttpGet("GetMeter/{id}")]
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
        public async Task<IActionResult> GetAllStores()
        {
            var data = await _mediator.Send(new GetAllShopRequest());
            return Ok(data);
        }
        [HttpGet("store-by-building/{id}")]
        public async Task<IActionResult> GetBuildingStores(int id)
        {
            return Ok(await _mediator.Send(new GetAllShopByBuildingIdRequest(id)));
        }
        [HttpPost("add-store")]
        public async Task<IActionResult> AddStore(AddEditStoreCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("createtenant")]
        public async Task<IActionResult> AddEditShopTenant(AddEditShopTenantCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("createRentalAgreement")]
        public async Task<IActionResult> AddRentalAgreement(AddEditRentalAgreementCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
