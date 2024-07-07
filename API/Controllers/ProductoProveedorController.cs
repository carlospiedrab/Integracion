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
    public class ProductoProveedorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductoProveedorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminVendedorRol")]
        public async Task<ActionResult<ProductoProveedorDto>> GetProductoProveedor(int id)
        {
            ProductoProveedor productoProveedor = await _context.ProductosProveedores
                                    .Include(pr => pr.Proveedor)
                                    .Include(p => p.Producto)
                                    .Include(c => c.Producto.Categoria)
                                    .Include(m => m.Producto.Marca)
                                    .FirstOrDefaultAsync(p => p.ProductoProveedorId == id);

            if (productoProveedor == null)
            {
                return NotFound();
            }
            return Ok(new ProductoProveedorDto()
            {
                ProductoNombre = productoProveedor.Producto.NombreProducto,
                CategoriaNombre = productoProveedor.Producto.Categoria.Nombre,
                MarcaNombre = productoProveedor.Producto.Marca.Nombre,
                ProveedorNombre = productoProveedor.Proveedor.Nombre
            });
        }

        [HttpPost]
        [Authorize(Policy = "AdminRol")]
        public async Task<ActionResult<ProductoProveedor>> PostProductoProveedor(ProductoProveedor productoProveedor)
        {
            try
            {
                await _context.ProductosProveedores.AddAsync(productoProveedor);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetProductoProveedor", new { id = productoProveedor.ProductoProveedorId }, productoProveedor);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminRol")]
        public async Task<ActionResult> DeleteProductoProveedor(int id)
        {
            ProductoProveedor productoProveedorBD = await _context.ProductosProveedores.FindAsync(id);
            if (productoProveedorBD == null) return NotFound();
            _context.ProductosProveedores.Remove(productoProveedorBD);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}