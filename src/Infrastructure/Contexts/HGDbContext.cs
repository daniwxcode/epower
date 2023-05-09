using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Contexts
{
    public partial class BlazorHeroContext
    {
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopTenant> ShopTenants { get; set; }
        public DbSet<Meter> Meters { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<InternalPayement> InternalPayements { get; set; }        
        public DbSet<RentalAgreement> RentalAgreements { get; set; }
        

    }
}
