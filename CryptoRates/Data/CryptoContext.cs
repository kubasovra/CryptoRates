using System;
using System.Collections.Generic;
using System.Text;
using CryptoRates.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CryptoRates.Data
{
    public class CryptoContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public CryptoContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Pair> Pairs { get; set; }
    }
}
