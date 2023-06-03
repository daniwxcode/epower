using BlazorHero.CleanArchitecture.Application.Interfaces.Services;

using Flurl.Http;

using Hangfire;

using IronPdf;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public  class PDFService : IPdfService
    {
        public PDFService()
        {
            License.LicenseKey = "IRONPDF.OTR.IRO220405.2499.75108.504022-C58226B4C5-CD2VLU6ONWP66XZ-RL44IBRWC6ND-B2CMHUHEXN2U-TKB4FT6V2LJ2-7HCMFKOVUA42-UKHBNE-LAOLMXLCVGOJUA-UNLIMITED.SUB-3XDWJR.RENEW.SUPPORT.13.MAY.2023";
            IronPdf.Installation.LinuxAndDockerDependenciesAutoConfig = true;
        }
        public Task GeneratePdf(string url, string fileName)
        {
            BackgroundJob.Enqueue(() => Generate(url, fileName));
            return Task.CompletedTask;
        }
        public async Task Generate(string url, string fileName)
        {
            var renderer = new ChromePdfRenderer();
            renderer.RenderingOptions.MarginTop = 20;
            renderer.RenderingOptions.MarginBottom = 0;
            renderer.RenderingOptions.MarginRight = 0;
            renderer.RenderingOptions.MarginLeft = 0;
            renderer.RenderingOptions.PrintHtmlBackgrounds = true;

            var htmldoc = await url.GetStringAsync();
            var pdf = renderer.RenderHtmlAsPdf(htmldoc);           

            var folder = new FileInfo(fileName).Directory.FullName;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                await pdf.Stream.CopyToAsync(stream);
            }
        }
    }
}
