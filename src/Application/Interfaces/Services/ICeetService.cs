using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public record CreditRequest(string SerialNumber, int Amount);
    public interface ICeetService
    {

        public Task<(string code, string compteur, decimal credit,string reference)> BuyCredit(CreditRequest creditRequest);
    }
}
