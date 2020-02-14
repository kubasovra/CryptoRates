using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CryptoRates.Data
{
    public class CryptoContext : IdentityDbContext
    {
        //To do:
        //Replace AlterColumns with AddColumns (try to force EF to do it automatically) and then manually delete old columns
        public CryptoContext(DbContextOptions<CryptoContext> options)
            : base(options)
        {
            //this.Database.Migrate();
        }

        public DbSet<Currency> Currencies { get; set; }
    }
}
