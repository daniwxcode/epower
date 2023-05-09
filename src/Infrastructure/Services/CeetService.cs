using BlazorHero.CleanArchitecture.Application.Interfaces.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public class CeetService: ICeetService
    {
        public async Task<(string code, string compteur, decimal credit, string reference)> BuyCredit(CreditRequest creditRequest)
        {
            return new(Guid.NewGuid().ToString().Substring(0, 12), creditRequest.SerialNumber,42, Guid.NewGuid().ToString().Substring(3, 15));
        }
    }
}
