using BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public record CreditRequest(string SerialNumber, int Amount);
    public record ConsumerCheckResponse(bool Status, decimal amount);
    public interface ICeetService
    {

        public Task<(string code, string compteur, decimal credit,string reference)> BuyCredit(CreditRequest creditRequest);
        public Task<ConsumerCheckResponse> ConsumerCheck(ConsumerCheckRequestData consumerCheckRequest);
    }
}
