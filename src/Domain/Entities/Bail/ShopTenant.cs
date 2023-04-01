using BlazorHero.CleanArchitecture.Domain.Contracts;

using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Bail
{
    public class ShopTenant : AuditableEntity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public List<RentalAgreement> RentalAgreements { get; set; } = new();
    }

}
