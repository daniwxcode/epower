using BlazorHero.CleanArchitecture.Application.Features.Sellers.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Sellers.Queries;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Sales;

[ApiController]
public class SellerController : BaseApiController<SellerController>
{
    /// <summary>
    /// Récupère la liste de tous les vendeurs.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = Permissions.Sellers.View)]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllSellersQuery()));

    /// <summary>
    /// Crée ou modifie un vendeur.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = Permissions.Sellers.Create)]
    public async Task<IActionResult> AddEdit(AddEditSellerCommand command)
        => Ok(await _mediator.Send(command));
}
