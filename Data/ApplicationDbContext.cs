using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models.Entidades;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<PrecioOferta> PrecioOfertas { get; set; }
        public DbSet<ProductoProveedor> ProductosProveedores { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<BodegaProducto> BodegaProductos { get; set; }
        public DbSet<KardexInventario> kardexInventarios { get; set; }
        public DbSet<Compania> Companias { get; set; }
        public DbSet<OrdenCompra> OrdenCompras { get; set; }
        public DbSet<OrdenCompraDetalle> OrdenCompraDetalles { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrdenCompraDetalle>()
                .Property(e => e.Subtotal)
                .HasComputedColumnSql("CAST((Costo * Cantidad) AS DECIMAL(18,2)) PERSISTED", stored: true)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<OrdenCompraDetalle>()
                 .ToTable(tb => tb.UseSqlOutputClause(false));
        }
        
    }
}
