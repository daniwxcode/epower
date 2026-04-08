using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

using QRCoder;

using System;
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
            ViewData["QrCode"] = GetQrCodeAsHtml(Request.GetDisplayUrl());
            return View(model);

        }
        return View("Error");
    }

    private static string GetQrCodeAsHtml(string url)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using var pngQrCode = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = pngQrCode.GetGraphic(5, [0, 73, 118], [255, 255, 255]);
        var base64 = Convert.ToBase64String(qrCodeBytes);
        return $"data:image/png;base64,{base64}";
    }
}
