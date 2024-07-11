using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;

namespace Data.Servicios
{
    public class KardexInventarioServicio : IKardexInventarioServicio
    {
        private readonly ApplicationDbContext _context;

        public KardexInventarioServicio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarKardex(int bodegaProductoId, string tipo, string detalle,
            int stockAnterior, int cantidad, string usuarioId)
        {
            try
            {
                var bodegaProductoDb = await _context
               .BodegaProductos.Where(bp => bp.Id == bodegaProductoId)
               .Include(p => p.Producto)
               .FirstOrDefaultAsync();

                if (bodegaProductoDb is null)
                {
                    throw new Exception("BodegaProducto no encontrado");
                }

                var costo = bodegaProductoDb.Producto.Costo;
                var stock = bodegaProductoDb.Cantidad;
                var total = stock * costo;

                var kardexInventario = new KardexInventario
                {
                    BodegaProductoId = bodegaProductoId,
                    Tipo = tipo,
                    Detalle = detalle,
                    StockAnterior = stockAnterior,
                    Cantidad = cantidad,
                    Costo = costo,
                    Stock = stock,
                    Total = total,
                    UsuarioId = usuarioId,
                    FechaRegistro = DateTime.Now
                };

                _context.kardexInventarios.Add(kardexInventario);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
