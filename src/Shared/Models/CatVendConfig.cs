using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Shared.Models
{

    public class CatVendConfig
    {
        /// <summary>
        /// When true, uses a local simulator instead of the real CATvend API.
        /// Set to true for development, false for production deployment.
        /// </summary>
        public bool UseSimulator { get; set; }

        public string BaseUrl { get; set; }
        public string ConsumerCheck { get; set; }
        public string Vend { get; set; }
        public string Accuse { get; set; }
        public string EndOfSession { get; set; }
        public string Transactions { get; set; }
        public string LastTransactions { get; set; }
        public string CurrentMonth { get; set; }
        public string ValidationCode { get; set; }
        public string Unit { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
