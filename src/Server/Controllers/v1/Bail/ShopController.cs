using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Bail
{
    public class ShopController:BaseApiController<ShopController>
    {
        [HttpGet]
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
