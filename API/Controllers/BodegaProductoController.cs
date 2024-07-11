using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Data;
using Data.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Entidades;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BodegaProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IKardexInventarioServicio _kardexServicio;
        private ApiResponse _response;

        public BodegaProductoController(ApplicationDbContext context, 
        IKardexInventarioServicio kardexServicio)
        {
            _context = context;
            _kardexServicio = kardexServicio;
            _response = new();
        }

        [Authorize(Policy = "AdminVendedorRol")]
        [HttpPost("IncrementarCantidades")]
        public async Task<ActionResult> IncrementarCantidades(
            int bodegaId,
            int productoId,
            int cantidad
        )
        {
            if (cantidad < 0)
                return BadRequest("La cantidad debe ser mayor a cero");

            if (!await BodegaExiste(bodegaId))
                return BadRequest("La bodega no existe");

            if (!await ProductoExiste(productoId))
                return BadRequest("El producto no existe");

            var bodegaProductoBd = await _context
                .BodegaProductos.Where(bp => bp.BodegaId == bodegaId && bp.ProductoId == productoId)
                .FirstOrDefaultAsync();
            
            int stockAnterior = 0;
            BodegaProducto bodegaProducto;

            if (bodegaProductoBd == null)
            {
                bodegaProducto = new BodegaProducto
                {
                    BodegaId = bodegaId,
                    ProductoId = productoId,
                    Cantidad = cantidad
                };

                await _context.AddAsync(bodegaProducto);
            }
            else
            {
                stockAnterior = bodegaProductoBd.Cantidad;
                bodegaProductoBd.Cantidad += cantidad;
                bodegaProducto = bodegaProductoBd;
            }
            await _context.SaveChangesAsync();
            _response.Resultado = bodegaProducto;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "Se ha actualizado la cantidad";

            //REGISTRAR KARDEX
            var userId = User.FindFirstValue("UserId");
            await _kardexServicio.RegistrarKardex(bodegaProducto.Id, "Entrada",
            "Incremento de cantidades", stockAnterior,cantidad, userId);

            return Ok(_response);
        }

        [Authorize(Policy = "AdminVendedorRol")]
        [HttpPost("DisminuirCantidades")]
        public async Task<ActionResult> DisminuirCantidades(int bodegaId, int productoId, int cantidad
        )
        {
            if (cantidad < 0)
                return BadRequest("La cantidad debe ser mayor a cero");

            if (!await BodegaExiste(bodegaId))
                return BadRequest("La bodega no existe");

            if (!await ProductoExiste(productoId))
                return BadRequest("El producto no existe");

            var bodegaProductoBd = await _context
                .BodegaProductos.Where(b => b.Bodega.Id == bodegaId && b.ProductoId == productoId)
                .FirstOrDefaultAsync();

            if (bodegaProductoBd == null)
                return BadRequest("No se encontró el registro");

            if (bodegaProductoBd.Cantidad < cantidad)
                return BadRequest(
                    $"La cantidad a disminuir es mayor a la existente: {bodegaProductoBd.Cantidad}"
                );

            int stockAnterior = bodegaProductoBd.Cantidad;
            bodegaProductoBd.Cantidad -= cantidad;

            if (bodegaProductoBd.Cantidad < 0)
                bodegaProductoBd.Cantidad = 0;

            await _context.SaveChangesAsync();

            _response.Resultado = bodegaProductoBd;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "Se ha actualizado la cantidad";

            var userId = User.FindFirstValue("UserId");
            await _kardexServicio.RegistrarKardex(
                bodegaProductoBd.Id,
                "Salida",
                "Disminución de cantidades",
                stockAnterior,
                cantidad,
                userId
            );

            return Ok(_response);
        }

        private async Task<bool> BodegaExiste(int bodegaId)
        {
            return await _context.Bodegas.AnyAsync(b => b.Id == bodegaId);
        }

        private async Task<bool> ProductoExiste(int productoId)
        {
            return await _context.Productos.AnyAsync(p => p.Id == productoId);
        }
    }
}
