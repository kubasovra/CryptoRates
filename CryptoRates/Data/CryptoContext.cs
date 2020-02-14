using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CryptoRates.Data
{
    public class CryptoContext : IdentityDbContext
    {
        public CryptoContext(DbContextOptions<CryptoContext> options)
            : base(options)
        {
            //this.Database.Migrate();
        }

        public DbSet<Currency> Currencies { get; set; }
    }
}
