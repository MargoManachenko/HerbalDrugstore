using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HerbalDrugstore.Models;

namespace HerbalDrugstore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Compound> Compound { get; set; }
        public virtual DbSet<Drug> Drug { get; set; }
        public virtual DbSet<Herb> Herb { get; set; }
        public virtual DbSet<Lot> Lot { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<Supply> Supply { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Compound>()
               .HasKey(c => new { c.DrugId, c.HerbId });
        }
    }
}
