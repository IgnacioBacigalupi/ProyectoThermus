using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Thermus.Api.Infrastructure
{
    public class ThermusDbContext : DbContext
    {
        public ThermusDbContext (DbContextOptions<ThermusDbContext> options)
            : base(options)
        {}
    }
}