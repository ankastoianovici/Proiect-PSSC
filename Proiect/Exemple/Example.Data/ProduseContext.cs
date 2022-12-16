using Example.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data
{
    public class ProduseContext : DbContext
    {
        public static void Main(string[] args)
        {
        }
        public ProduseContext(DbContextOptions<ProduseContext> options) : base(options)
        {
        }

        public DbSet<ProdusDto> produse { get; set; }

        public DbSet<OmDto> oameni { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OmDto>().ToTable("Student").HasKey(s => s.OmId);
            modelBuilder.Entity<ProdusDto>().ToTable("Grade").HasKey(s => s.ProdusId);
        }
    }
}