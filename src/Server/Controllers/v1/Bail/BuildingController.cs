using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Meters.Queries;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Stores.Commands;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Bail
{
    public class BuildingController : BaseApiController<BuildingController>
    {
        [HttpGet("all")]
        [Authorize(Policy = Permissions.BuildingParams.View)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _mediator.Send(new GetAllBuildingsRequest());
            return Ok(data);
        }
        [HttpPost("add")]
        [Authorize(Policy = Permissions.BuildingParams.Create)]
        public async Task<IActionResult> AddBuilding(AddEditBuildingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Policy = Permissions.BuildingParams.Delete)]
        public async Task<IActionResult> DeleteBuilding(int id)
        {
            return Ok(await _mediator.Send(new DeleteBuildingCommand(id)));
        }

        [HttpGet("all-meters")]
        [Authorize(Policy = Permissions.BuildingParams.View)]
        public async Task<IActionResult> GetAllMeters()
        {
            var data = await _mediator.Send(new GetAllMetersRequest());
            return Ok(data);
        }
        [HttpGet("{id}/meters")]
        [Authorize(Policy = Permissions.BuildingParams.View)]
        public async Task<IActionResult> GetAllByBuilding(int id)
        {
            var data = await _mediator.Send(new GetAllMetersByBuildingIdRequest(id));
            return Ok(data);
        }
        [HttpGet("GetMeter/{id}")]
        [Authorize(Policy = Permissions.BuildingParams.View)]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _mediator.Send(new GetMeterByIdRequest(id));
            return Ok(data);
        }  
        [HttpGet("GetMeter-by-serial/{serial}")]
        [Authorize(Policy = Permissions.BuildingParams.View)]
        public async Task<IActionResult> GetBySerial(string serial)
        {
            var data = await _mediator.Send(new GetMeterBySerialRequest(serial));
            return Ok(data);
        }
        [HttpPost("add-meter")]
        [Authorize(Policy = Permissions.BuildingParams.Create)]
        public async Task<IActionResult> AddStore(AddEditMeterCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpGet("stores")]
        [Authorize(Policy = Permissions.BuildingParams.View)]
        public async Task<IActionResult> GetAllStores()
        {
            var data = await _mediator.Send(new GetAllShopRequest());
            return Ok(data);
        }
        [HttpGet("store-by-building/{id}")]
        [Authorize(Policy = Permissions.BuildingParams.View)]
        public async Task<IActionResult> GetBuildingStores(int id)
        {
            return Ok(await _mediator.Send(new GetAllShopByBuildingIdRequest(id)));
        }
        [HttpPost("add-store")]
        [Authorize(Policy = Permissions.BuildingParams.Create)]
        public async Task<IActionResult> AddStore(AddEditStoreCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpDelete("DeleteStore/{id}")]
        [Authorize(Policy = Permissions.BuildingParams.Delete)]
        public async Task<IActionResult> DeleteStore(int id)
        {
            return Ok(await _mediator.Send(new DeleteStoreCommand(id)));
        }
        [HttpPost("createtenant")]
        [Authorize(Policy = Permissions.BuildingParams.Create)]
        public async Task<IActionResult> AddEditShopTenant(AddEditShopTenantCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("createRentalAgreement")]
        [Authorize(Policy = Permissions.BuildingParams.Create)]
        public async Task<IActionResult> AddRentalAgreement(AddEditRentalAgreementCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
