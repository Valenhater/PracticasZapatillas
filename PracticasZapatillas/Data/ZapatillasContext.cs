using Microsoft.EntityFrameworkCore;
using PracticasZapatillas.Models;

namespace PracticasZapatillas.Data
{
    public class ZapatillasContext:DbContext
    {
        public ZapatillasContext(DbContextOptions<ZapatillasContext> options) : base(options) { }

        public DbSet<Zapatilla> Zapatillas { get; set;}

        public DbSet<Imagen> Imagenes { get; set;}

        public DbSet<ModelPaginacion> ModelPaginacion { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ModelPaginacion>().HasNoKey();
        }

    }
}
