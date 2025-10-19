using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Parcial3.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class ApplicationDbContext : DbContext
    {
        //CodeFirst :)
        public DbSet<Client> Clients{ get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Este es el objeto que lee las configuracion
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
              optionsBuilder.UseSqlServer(connectionString);
            }

        }

    


    //Vamos a configurar las relaciones
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Invoices)
                .WithOne(f => f.Client)
                .HasForeignKey(f => f.ClientId);

            // Relación: Una Factura tiene muchos Items
            modelBuilder.Entity<Invoice>()
                .HasMany(f => f.Items)
                .WithOne(i => i.Invoice)
                .HasForeignKey(i => i.InvoiceId);
        }
    }
}
