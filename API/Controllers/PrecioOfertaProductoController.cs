using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrecioOfertaProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrecioOfertaProductoController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPut]
        public async Task<ActionResult> AgregarOferta(
             int productoId,
             decimal NuevoPrecio,
             string TextoPromocional
         )
        {
            if (!await ProductoExiste(productoId))
                return BadRequest("El producto no existe");

            try
            {
                
                var BDprod = await _context.PrecioOfertas.FindAsync( productoId );

                if( BDprod == null) return NotFound();

                BDprod.NuevoPrecio = NuevoPrecio;
                BDprod.TextoPromocional = TextoPromocional;
                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (System.Exception)
            {

                throw;

            }

        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Producto>> EliminarOferta( int productoId )
        {
            if (!await ProductoExiste(productoId))
                return BadRequest("El producto no existe");

            try
            {

                var BDprod = await _context.PrecioOfertas.FindAsync( productoId );


                if( BDprod == null) return NotFound();
                
                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (System.Exception)
            {

                throw;

            }

        }

        private async Task<bool> ProductoExiste(int productoId)
        {
            return await _context.Productos.AnyAsync(p => p.Id == productoId);
        }
    }
}