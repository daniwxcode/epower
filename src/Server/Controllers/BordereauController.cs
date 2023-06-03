using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers;

public class BordereauController :BaseController<BordereauController>
{
    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index(int Id)
    {

        var response = await _mediator.Send(new GetPayementReceipt() { PayementID = Id });
        if (response.Succeeded)
        {
            var model = response.Data;
            return View(model);
        }
        return View("Error");
    }
}
