using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Polly;

using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Sales
{

    [ApiController]
    public class CashPowerController : BaseApiController<CashPowerController>
    {
        private readonly ICurrentUserService _currentUserService;
        public CashPowerController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        [HttpPost]
        [Authorize(Policy = Permissions.CashPower.Create)]
        public async Task<IActionResult> SellCredit(MakeAPaymentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("all-payement")]
        [Authorize(Policy = Permissions.CashPower.View)]
        public async Task<IActionResult> GetSales()
        {
            var request = new GetPayementByCriteriaRequest()
            {
                PaymentRequestCriteria = Application.Features.Habitat.Enums.PaymentRequestCriteria.All
            };
            return Ok(await _mediator.Send(request));
        }
        [HttpGet("all-payement/{serialnumber}")]
        [Authorize(Policy = Permissions.CashPower.View)]
        public async Task<IActionResult> GetSalesByMeter(string serialnumber)
        {
            var request = new GetPayementByCriteriaRequest()
            {
                PaymentRequestCriteria = Application.Features.Habitat.Enums.PaymentRequestCriteria.ByMeter,
                Criteria = serialnumber,
            };
            return Ok(await _mediator.Send(request));
        }
        [HttpGet("all-payement/byuser/{user}")]
        [Authorize(Policy = Permissions.CashPower.View)]
        public async Task<IActionResult> GetSalesByUser(string user)
        {
            var request = new GetPayementByCriteriaRequest()
            {
                PaymentRequestCriteria = Application.Features.Habitat.Enums.PaymentRequestCriteria.ByUser,
                Criteria = user,
            };
            return Ok(await _mediator.Send(request));
        }
        [HttpGet("all-payement/mysales/")]
        [Authorize(Policy = Permissions.CashPower.View)]
        public async Task<IActionResult> GetCurrentUserSales()
        {
            
            var request = new GetPayementByCriteriaRequest()
            {
                PaymentRequestCriteria = Application.Features.Habitat.Enums.PaymentRequestCriteria.ByUser,
                Criteria = _currentUserService.UserId,
            };
            return Ok(await _mediator.Send(request));
        }
    }
}
