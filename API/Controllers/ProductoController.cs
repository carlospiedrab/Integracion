using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Entidades;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "AdminVendedorRol")]
        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> GetProductos()
        {
            List<ProductoDto> lista = await _context.Productos
                                            .Include(c => c.Categoria)
                                            .Include(m => m.Marca)
                                            .Include(o => o.Promocion)
                                            .Select(p => new ProductoDto
                                            {
                                                NombreProducto = p.NombreProducto,
                                                Categoria = p.Categoria.Nombre,
                                                Marca = p.Marca.Nombre,
                                                Precio = p.Precio,
                                                Costo = p.Costo,
                                                PrecioActual = p.Promocion == null
                                                              ? p.Precio
                                                              : p.Promocion.NuevoPrecio,
                                                TextoPromocional = p.Promocion == null
                                                              ? null
                                                              : p.Promocion.TextoPromocional
                                            }).ToListAsync();

            return Ok(lista);
        }

        [Authorize(Policy = "AdminVendedorRol")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            Producto producto = await _context.Productos
                                    .Include(c => c.Categoria)
                                    .Include(m => m.Marca)
                                     .Include(o => o.Promocion)
                                    .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }
            return Ok(new ProductoDto
            {
                NombreProducto = producto.NombreProducto,
                Categoria = producto.Categoria.Nombre,
                Marca = producto.Marca.Nombre,
                Precio = producto.Precio,
                Costo = producto.Costo,
                PrecioActual = producto.Promocion == null
                                                    ? producto.Precio
                                                    : producto.Promocion.NuevoPrecio,
                TextoPromocional = producto.Promocion == null
                                                    ? null
                                                    : producto.Promocion.TextoPromocional
            });
        }

        [Authorize(Policy = "AdminRol")]
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(ProductoDto producto)
        {
            try
            {
                var categoria = _context.Categorias.Where(x => x.Nombre.Equals(producto.Categoria)).First();
                var marca = _context.Marcas.Where(x => x.Nombre.Equals(producto.Marca)).First();
                if (categoria.Estado==false || marca.Estado==false)
                {
                    return BadRequest("No se pudo crear el Producto");
                }
                var productoAgregar = new Producto{
                    NombreProducto = producto.NombreProducto,
                    Precio = producto.Precio,
                    Costo = producto.Costo,
                    CategoriaId = categoria.Id,
                    MarcaId = marca.Id
                };

                await _context.Productos.AddAsync(productoAgregar);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetProducto", new { id = productoAgregar.Id }, producto);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize(Policy = "AdminRol")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }
            Producto productoBD = await _context.Productos.FindAsync(id);
            if (productoBD == null) return NotFound();

            productoBD.NombreProducto = producto.NombreProducto;
            productoBD.CategoriaId = producto.CategoriaId;
            productoBD.MarcaId = producto.MarcaId;
            productoBD.Precio = producto.Precio;
            productoBD.Costo = producto.Costo;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Policy = "AdminRol")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProducto(int id)
        {
            Producto productoBD = await _context.Productos.FindAsync(id);
            if (productoBD == null) return NotFound();
            _context.Productos.Remove(productoBD);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}