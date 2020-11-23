using BoGroent.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoGroent.Infrastructure
{
    public class BoGroentContext : IdentityDbContext<AppUser>
    {
        public BoGroentContext(DbContextOptions<BoGroentContext> options) : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Leases> Leases { get; set; }
    }
}
