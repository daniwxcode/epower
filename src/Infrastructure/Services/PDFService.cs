using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;

using Hangfire;

using QRCoder;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public class PDFService : IPdfService
    {
        private static readonly CultureInfo FrCulture = CultureInfo.GetCultureInfo("fr-FR");

        // Thermal receipt width ≈ 80 mm
        private static readonly PageSize ReceiptPageSize = new(80, 200, Unit.Millimetre);

        private const string CompanyName = "HABITAT DU GOLF";
        private const string CompanyPhone = "(+228) 91 37 83 64";
        private const string CompanyAddress = "Lomé, Togo";
        private const string PrimaryHex = "#004976";
        private const string LightGrayHex = "#F5F5F5";

        public PDFService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public Task GenerateReceiptAsync(BuyCreditResponse sale, string fileName)
        {
            BackgroundJob.Enqueue(() => Generate(sale, fileName));
            return Task.CompletedTask;
        }

        public void Generate(BuyCreditResponse sale, string fileName)
        {
            var folder = new FileInfo(fileName).Directory!.FullName;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(ReceiptPageSize);
                    page.MarginHorizontal(4, Unit.Millimetre);
                    page.MarginVertical(6, Unit.Millimetre);
                    page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

                    page.Content().Column(col =>
                    {
                        col.Spacing(3);

                        ComposeHeader(col);
                        ComposeDivider(col);
                        ComposeReceiptInfo(col, sale);
                        ComposeDivider(col);
                        ComposeCode(col, sale);
                        ComposeDivider(col);
                        ComposeDetails(col, sale);
                        ComposeDivider(col);
                        ComposeFooter(col, sale);
                    });
                });
            }).GeneratePdf(fileName);
        }

        private static void ComposeHeader(ColumnDescriptor col)
        {
            col.Item().AlignCenter().Text(text =>
            {
                text.Line(CompanyName).Bold().FontSize(12).FontColor(PrimaryHex);
            });
            col.Item().AlignCenter().Text(text =>
            {
                text.Line(CompanyPhone).FontSize(7);
            });
            col.Item().AlignCenter().Text(text =>
            {
                text.Line(CompanyAddress).FontSize(7).Italic();
            });
            col.Item().AlignCenter().PaddingTop(2).Text(text =>
            {
                text.Line("REÇU DE VENTE").Bold().FontSize(10).FontColor(PrimaryHex);
            });
        }

        private static void ComposeReceiptInfo(ColumnDescriptor col, BuyCreditResponse sale)
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Text(text =>
                {
                    text.Span("Reçu N° ").FontSize(7);
                    text.Span(sale.InternalReference).Bold().FontSize(7);
                });
                row.RelativeItem().AlignRight().Text(text =>
                {
                    text.Span(sale.date.ToString("dd/MM/yyyy HH:mm")).FontSize(7);
                });
            });

            col.Item().PaddingTop(2).Row(row =>
            {
                row.RelativeItem().Text(text =>
                {
                    text.Span("Compteur : ").FontSize(8);
                    text.Span(sale.SerialNumber).Bold().FontSize(9);
                });
            });
        }

        private static void ComposeCode(ColumnDescriptor col, BuyCreditResponse sale)
        {
            col.Item().Background(LightGrayHex).Padding(4).AlignCenter().Column(inner =>
            {
                inner.Spacing(2);
                inner.Item().AlignCenter().Text("CODE").FontSize(7).FontColor(Colors.Grey.Medium);
                inner.Item().AlignCenter().Text(sale.Code)
                    .Bold().FontSize(16).FontColor(PrimaryHex).LetterSpacing(0.15f);
            });
        }

        private static void ComposeDetails(ColumnDescriptor col, BuyCreditResponse sale)
        {
            var total = (int)sale.Amount + (int)sale.fees;

            ComposeDetailRow(col, "Montant", $"{sale.Amount.ToString("N0", FrCulture)} FCFA");
            ComposeDetailRow(col, "Frais", $"{sale.fees.ToString("N0", FrCulture)} FCFA");

            col.Item().Background(LightGrayHex).PaddingVertical(2).PaddingHorizontal(2).Row(row =>
            {
                row.RelativeItem().Text("TOTAL").Bold().FontSize(9);
                row.RelativeItem().AlignRight().Text($"{total.ToString("N0", FrCulture)} FCFA")
                    .Bold().FontSize(9).FontColor(PrimaryHex);
            });

            col.Item().PaddingTop(2);
            ComposeDetailRow(col, "kWh", $"{sale.Credit}");
            ComposeDetailRow(col, "Référence", sale.Reference);
            ComposeDetailRow(col, "Vendeur", sale.Seller);
        }

        private static void ComposeDetailRow(ColumnDescriptor col, string label, string value)
        {
            col.Item().PaddingHorizontal(2).Row(row =>
            {
                row.RelativeItem().Text(label).FontSize(7).FontColor(Colors.Grey.Darken1);
                row.RelativeItem().AlignRight().Text(value).Bold().FontSize(8);
            });
        }

        private static void ComposeFooter(ColumnDescriptor col, BuyCreditResponse sale)
        {
            var qrBytes = GenerateQrCode(sale);

            col.Item().AlignCenter().Width(25, Unit.Millimetre).Height(25, Unit.Millimetre)
                .Image(qrBytes);

            col.Item().AlignCenter().PaddingTop(3).Text(text =>
            {
                text.Line("Merci pour votre achat !").FontSize(7).Italic();
            });
            col.Item().AlignCenter().Text(text =>
            {
                text.Line("Conservez ce reçu comme preuve de paiement").FontSize(6).FontColor(Colors.Grey.Medium);
            });
        }

        private static byte[] GenerateQrCode(BuyCreditResponse sale)
        {
            var qrContent = $"REF:{sale.InternalReference}|CODE:{sale.Code}|METER:{sale.SerialNumber}|AMT:{sale.Amount}|DATE:{sale.date:yyyy-MM-dd HH:mm}";
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            using var pngQrCode = new PngByteQRCode(qrCodeData);
            return pngQrCode.GetGraphic(5, [0, 73, 118], [255, 255, 255]);
        }

        private static void ComposeDivider(ColumnDescriptor col)
        {
            col.Item().PaddingVertical(1).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);
        }
    }
}

