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
                                                Id = p.Id,
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
                                                              : p.Promocion.TextoPromocional,
                                                Estado = p.Estado == true
                                                        ? "Activo" 
                                                        : "Incactivo"
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
                                                    : producto.Promocion.TextoPromocional,
                Estado = producto.Estado == true
                                                        ? "Activo"
                                                        : "Incactivo"
            });
        }




        [Authorize(Policy = "AdminVendedorRol")]
        [HttpGet("GetProductosActivos")]
        public async Task<ActionResult<List<ProductoDto>>> GetProductosActivos()
        {
            List<ProductoDto> lista = await _context.Productos
                                            .Include(c => c.Categoria)
                                            .Include(m => m.Marca)
                                            .Include(o => o.Promocion)
                                            .Where(p => p.Estado == true)
                                            .Select(p => new ProductoDto
                                            {
                                                Id = p.Id,
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
                                                              : p.Promocion.TextoPromocional,
                                                Estado = p.Estado == true
                                                        ? "Activo"
                                                        : "Incactivo"
                                            }).ToListAsync();

            return Ok(lista);

        }


        [Authorize(Policy = "AdminRol")]
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(ProductoPostDto productoPostDto)
        {
            try
            {

                Producto productoNew = new Producto();
                productoNew.NombreProducto = productoPostDto.NombreProducto;
                productoNew.CategoriaId = productoPostDto.CategoriaId;
                productoNew.MarcaId = productoPostDto.MarcaId;
                productoNew.Precio = productoPostDto.Precio;
                productoNew.Costo = productoPostDto.Costo;
                productoNew.Estado = true;



                await _context.Productos.AddAsync(productoNew);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetProducto", new { id = productoNew.Id }, productoPostDto);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize(Policy = "AdminRol")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProducto(int id, ProductoPutDto productoPutDto)
        {
            if (id != productoPutDto.Id)
            {
                return BadRequest();
            }
            Producto productoBD = await _context.Productos.FindAsync(id);
            if (productoBD == null) return NotFound();

            productoBD.NombreProducto = productoPutDto.NombreProducto;
            productoBD.CategoriaId = productoPutDto.CategoriaId;
            productoBD.MarcaId = productoPutDto.MarcaId;
            productoBD.Precio = productoPutDto.Precio;
            productoBD.Costo = productoPutDto.Costo;
            productoBD.Estado  = productoPutDto.Estado;
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