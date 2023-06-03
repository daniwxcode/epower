using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries;

using IronBarCode;

using IronSoftware.Drawing;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

using SixLabors.ImageSharp;

using System;
using System.Threading.Tasks;

using Color = IronSoftware.Drawing.Color;

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

    private string GetQrCodeAsHtml(string url)
    {
        var code = QRCodeWriter.CreateQrCode(url, 150);
        code.ChangeBarCodeColor(Color.FromArgb(255, 0, 73, 118));
        int i = 0;
        while (!code.Verify())
        {
            i++;
            code.ChangeBarCodeColor(Color.FromArgb(255, 0 + i, 73 + i, 118 + i));
        }
        return code.ToDataUrl();
        //return code.ToHtmlTag();
    }
}
