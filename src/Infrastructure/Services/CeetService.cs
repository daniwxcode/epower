using BlazorHero.CleanArchitecture.Application.Configurations;
using BlazorHero.CleanArchitecture.Application.Exceptions;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend.Requests;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend.Sessions;
using BlazorHero.CleanArchitecture.Application.Features.Habitat.CatVend.Vend;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Shared.Models;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using Flurl.Http;
using Flurl.Http.Xml;

using MediatR;

using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    [XmlRoot(ElementName = "catvend")]
    public class CatVend
    {
        public CatVend()
        {

        }
        public CatVend(string unit, string validationcode, string receipt)
        {
            this.Unit = unit;
            this.ValidationCode = validationcode;
            this.Receipt = receipt;
        }
        [XmlElement(ElementName = "unit")]
        public string Unit { get; set; }

        [XmlElement(ElementName = "validation_code")]
        public string ValidationCode { get; set; }

        [XmlElement(ElementName = "receipt")]
        public string Receipt { get; set; }
    }
    public class CeetService : ICeetService
    {
        private readonly CatVendConfig _config;
        public CeetService(IOptions<CatVendConfig> config)
        {
            _config = config.Value;
        }
        public async Task<CreditVendResponse> BuyCredit(CreditRequest creditRequest)
        {
            var _canBeSold = await ConsumerCheck(new(creditRequest.SerialNumber, creditRequest.Amount));
            if (_canBeSold is null)
                return null;
            if (!_canBeSold.Status)
                return null;
            EVendRequest eVendRequest = new EVendRequest()
            {
                Amount = creditRequest.Amount,
                Meter = creditRequest.SerialNumber,
                Phone = "70705684",// creditRequest.PhoneNumber,
                Unit = _config.Unit,
                Username = _config.Username,
                Password = _config.Password,
                Validation_code = _config.ValidationCode
            };
            var call = await $"{_config.BaseUrl}{_config.Vend}"
               .PostXmlAsync(eVendRequest);
            checkResponse(call);
            if (call.StatusCode == 200)
            {
                var xmlData = await call.GetStringAsync();
                VendSuprimaResponse response = new VendSuprimaResponse();
                XmlSerializer serializer = new XmlSerializer(typeof(VendSuprimaResponse));
                using (TextReader reader = new StringReader(xmlData))
                {
                    response = (VendSuprimaResponse)serializer.Deserialize(reader);
                    if (response.ThinClient.Vend.Success == 1)
                    {
                        var token = response.ThinClient.Vend.Token;
                        string pattern = @"(\d{4})(\d{4})(\d{4})(\d{4})(\d{4})";
                        string replacement = "$1-$2-$3-$4-$5";
                        string formatted = Regex.Replace(token.Tk60, pattern, replacement);
                        if (double.TryParse(token.Tk50, NumberStyles.Any, CultureInfo.CurrentCulture, out double result))
                        {

                            return new CreditVendResponse(true, formatted, result, token.Tk10, "Vente effectuée avec succès");
                        }


                    }
                }
            }
            
            return null;
        }

        private void checkResponse(IFlurlResponse call)
        {
            if (call.StatusCode == 403)
            {
                throw new ApiException("Le revendeur est temporairement bloqué");
            }
            if (call.StatusCode == 409)
            {
                throw new ApiException("Suprima a refusé la requête (détails dans le corps de la réponse)");
            }
        }
        public async Task<ConsumerCheckResponse> ConsumerCheck(ConsumerCheckRequestData command)
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
                var call = await $"{_config.BaseUrl}{_config.ConsumerCheck}"
                    .PostXmlAsync(request);
                checkResponse(call);
               var  xmlData = await call
                    .GetStringAsync();
                SuprimaResponse response = new SuprimaResponse();
                XmlSerializer serializer = new XmlSerializer(typeof(SuprimaResponse));
                using (TextReader reader = new StringReader(xmlData))
                {
                    response = (SuprimaResponse)serializer.Deserialize(reader);
                    if (response.ThinClient.ConsumerCheck.Success == 1)
                    {
                        var amount = response.ThinClient.ConsumerCheck.Token.Amount;
                        if (request.Amount < amount)
                        {
                            return new ConsumerCheckResponse(false, amount);
                        }
                        return new ConsumerCheckResponse(true, amount);
                    }
                }
                return null;           
        }

        public async Task Confirm(CreditVendResponse response)
        {
            CatVend catVend = new CatVend(_config.Unit, _config.ValidationCode, response.bill);
            var call = await $"{_config.BaseUrl}{_config.Accuse}"
                   .PostXmlAsync(catVend);
            checkResponse (call);
            if (call.StatusCode == 200)
            {
                return;
            }
        } 
        public async Task EOS()
        {
            EOSCatVend request = new EOSCatVend()
            {
                Password = _config.Password,
                Username = _config.Username,
                Unit = _config.Unit,
                ValidationCode = _config.ValidationCode,
            };
            var call = await $"{_config.BaseUrl}{_config.EndOfSession}"
                    .PostXmlAsync(request);
        }
    }
}
