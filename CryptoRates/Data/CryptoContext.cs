using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CryptoRates.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CryptoRates.Data
{
    public class CryptoContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public CryptoContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Pair>()
                .Property(p => p.HistoricalData)
                .HasConversion(v => string.Join(";", v), v => v.Split(new[] { ';' }).ToList());
        }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Pair> Pairs { get; set; }
    }
}
