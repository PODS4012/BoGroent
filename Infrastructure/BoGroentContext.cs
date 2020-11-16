using BoGroent.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoGroent.Infrastructure
{
    public class BoGroentContext : DbContext
    {
        public BoGroentContext(DbContextOptions<BoGroentContext> options) : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
    }
}
