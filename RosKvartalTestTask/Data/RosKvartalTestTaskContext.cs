using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RosKvartalTestTask.Models;

namespace RosKvartalTestTask.Data
{
    public class RosKvartalTestTaskContext : DbContext
    {
        public RosKvartalTestTaskContext (DbContextOptions<RosKvartalTestTaskContext> options)
            : base(options)
        {
        }

        public DbSet<RosKvartalTestTask.Models.InspectionsRegister> InspectionsRegister { get; set; } = default!;
    }
}
