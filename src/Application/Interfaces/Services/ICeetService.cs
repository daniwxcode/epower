using BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;

using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public record CreditRequest(string SerialNumber, int Amount, string PhoneNumber);
    public record CreditVendResponse(bool Statut, string Code, double credit, string bill, string Message);
    public record ConsumerCheckResponse(bool Status, decimal amount);
    public interface ICeetService
    {

        public Task<CreditVendResponse> BuyCredit(CreditRequest creditRequest);
        public Task<ConsumerCheckResponse> ConsumerCheck(ConsumerCheckRequestData consumerCheckRequest);
        public Task Confirm(CreditVendResponse response);
    }
}
