using Microsoft.EntityFrameworkCore;

namespace TP6_ICS_G10_2024.Clases
{
    public class TPDBcontext: DbContext
    {
        public TPDBcontext(DbContextOptions options) : base(options)
        {
            
        }
        public virtual DbSet<Domicilio> Domicilios { get; set; }
        public virtual DbSet<Localidad> Localidades { get; set; }

        public virtual DbSet<Provincia> Provincias { get; set; }

        public virtual DbSet<TipoCarga> TipoCargas { get; set; }
        public virtual DbSet<FormaDePago> FormaDePagos { get; set; }

        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Pais> Paises { get; set; }

        //caundo hago la consulta no tengas en cuenta al tributo Imagen del pedido 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>().Ignore(p => p.DomiciolioDeUsuario);
        }


    }
}
