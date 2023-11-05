using BlazorHero.CleanArchitecture.Application.Configurations;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Shared.Models;

using Flurl.Http;
using Flurl.Http.Xml;

using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public class CeetService : ICeetService
    {
        private readonly CatVendConfig _config;
        public CeetService(IOptions<CatVendConfig> config)
        {
            _config = config.Value;
        }
        public async Task<(string code, string compteur, decimal credit, string reference)> BuyCredit(CreditRequest creditRequest)
        {
            return new(Guid.NewGuid().ToString().Substring(0, 12), creditRequest.SerialNumber, 42, Guid.NewGuid().ToString().Substring(3, 15));
        }

        public async Task<ConsumerCheckResponse> ConsumerCheck(ConsumerCheckRequestData command)
        {
            try
            {
                var request = new ConsumerCheckRequest()
                {
                    Amount = command.amount,
                    Meter = command.meter,
                    Username = _config.Username,
                    Unit = _config.Unit,
                    Password = _config.Password,
                    Validation_code = _config.ValidationCode
                };
                var xmlData = await $"{_config.BaseUrl}{_config.ConsumerCheck}"
                    .PostXmlAsync(request)
                    .ReceiveString();
                SuprimaResponse response = new SuprimaResponse();
                XmlSerializer serializer = new XmlSerializer(typeof(SuprimaResponse));
                using (TextReader reader = new StringReader(xmlData))
                {
                    response = (SuprimaResponse)serializer.Deserialize(reader);
                    if (response.ThinClient.ConsumerCheck.Success == 1)
                    { var amount = response.ThinClient.ConsumerCheck.Token.Amount;
                        if (request.Amount< amount)
                        {
                            return new ConsumerCheckResponse(false, amount);
                        }
                        return new ConsumerCheckResponse(true, amount);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return null;
            }


        }
    }
}
