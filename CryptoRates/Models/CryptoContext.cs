using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CryptoRates.Models
{
    public class CryptoContext : IdentityDbContext
    {
        public CryptoContext(DbContextOptions<CryptoContext> options) : base(options)
        {
            this.Database.Migrate();
        }
    }
}
