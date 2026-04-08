using BlazorHero.CleanArchitecture.Application.Interfaces.Services;

using Flurl.Http;

using Hangfire;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public  class PDFService : IPdfService
    {
        public PDFService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }
        public Task GeneratePdf(string url, string fileName)
        {
            BackgroundJob.Enqueue(() => Generate(url, fileName));
            return Task.CompletedTask;
        }
        public async Task Generate(string url, string fileName)
        {
            var htmlContent = await url.GetStringAsync();
            var textContent = Regex.Replace(htmlContent, "<[^>]+>", " ");
            textContent = Regex.Replace(textContent, @"\s+", " ").Trim();

            var folder = new FileInfo(fileName).Directory!.FullName;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.Content().Text(textContent).FontSize(10);
                });
            }).GeneratePdf(fileName);
        }
    }
}
