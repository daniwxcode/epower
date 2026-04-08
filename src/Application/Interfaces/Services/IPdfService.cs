using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;

using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public interface IPdfService
    {
        /// <summary>
        /// Generates a PDF sales receipt for the given sale data.
        /// </summary>
        Task GenerateReceiptAsync(BuyCreditResponse sale, string fileName);
    }
}
