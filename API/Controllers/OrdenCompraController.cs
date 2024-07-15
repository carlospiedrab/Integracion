using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Data;
using Data.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Entidades;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenCompraController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IKardexInventarioServicio _kardexServicio;
        private ApiResponse _response;

        public OrdenCompraController(
            ApplicationDbContext context,
            IKardexInventarioServicio kardexServicio
        )
        {
            _context = context;
            _kardexServicio = kardexServicio;
            _response = new();
        }

        [HttpPost]
        public async Task<ActionResult> CrearOrdenCompra(CreateOrdenCompraDto createOrdenCompraDto)
        {
            try
            {
                var proveedorProductos = await _context
                    .ProductosProveedores.Where(pp =>
                        pp.ProveedorId == createOrdenCompraDto.ProveedorId
                    )
                    .Select(pp => pp.ProductoId)
                    .ToListAsync();

                foreach (var detalle in createOrdenCompraDto.Detalles)
                {
                    if (!proveedorProductos.Contains(detalle.ProductoId))
                        return BadRequest(
                            $"El producto con ID {detalle.ProductoId} no pertenece al proveedor seleccionado."
                        );

                    if (detalle.Cantidad < 0)
                        return BadRequest("La cantidad no puede ser negativa.");
                }

                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var appUser = await _context.AppUser.FirstOrDefaultAsync(u => u.UserName == claim.Value);

                var ordenCompra = new OrdenCompra
                {
                    ProveedorId = createOrdenCompraDto.ProveedorId,
                    BodegaId = createOrdenCompraDto.BodegaId,
                    FechaIngreso = DateTime.Now,
                    UsuarioId = appUser.Id, //User.FindFirstValue("UserId"),
                    TotalOrden = 0
                };

                await _context.OrdenCompras.AddAsync(ordenCompra);
                await _context.SaveChangesAsync();

                foreach (var detalle in createOrdenCompraDto.Detalles)
                {
                    var ordenCompraDetalle = new OrdenCompraDetalle
                    {
                        OrdenCompraId = ordenCompra.Id,
                        ProductoId = detalle.ProductoId,
                        Cantidad = detalle.Cantidad,
                        Costo = detalle.Costo,
                    };

                    await _context.OrdenCompraDetalles.AddAsync(ordenCompraDetalle);

                    //Afectar el inventario
                    var bodegaProductoBd = await _context.BodegaProductos.FirstOrDefaultAsync(bp =>
                        bp.BodegaId == ordenCompra.BodegaId && bp.ProductoId == detalle.ProductoId
                    );

                    var stockAnterior = bodegaProductoBd?.Cantidad ?? 0;
                    if (bodegaProductoBd == null)
                    {
                        bodegaProductoBd = new BodegaProducto
                        {
                            BodegaId = ordenCompra.BodegaId,
                            ProductoId = detalle.ProductoId,
                            Cantidad = detalle.Cantidad
                        };

                        await _context.BodegaProductos.AddAsync(bodegaProductoBd);
                    }
                    else
                    {
                        bodegaProductoBd.Cantidad += detalle.Cantidad;
                    }
                    await _context.SaveChangesAsync();

                    //registrar kardex
                    await _kardexServicio.RegistrarKardex(
                        bodegaProductoBd.Id,
                        "Entrada",
                        "Ingreso por orden de compra",
                        stockAnterior,
                        detalle.Cantidad,
                        ordenCompra.UsuarioId
                    );
                }
                _response.Resultado = null;
                _response.IsExitoso = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Mensaje = "Se ha registrado la orden de compra";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                // await transaction.RollbackAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenCompraDto>> GetOrdenCompraById(int id)
        {
            var ordenCompra = await _context
                .OrdenCompras.Include(oc => oc.OrdenCompraDetalles)
                .FirstOrDefaultAsync(oc => oc.Id == id);

            if (ordenCompra == null)
                return NotFound();

            var ordenCompraDto = new OrdenCompraDto
            {
                ProveedorId = ordenCompra.ProveedorId,
                BodegaId = ordenCompra.BodegaId,
                FechaIngreso = ordenCompra.FechaIngreso,
                UsuarioId = ordenCompra.UsuarioId,
                TotalOrden = ordenCompra.TotalOrden,
                Detalles = ordenCompra
                    .OrdenCompraDetalles.Select(d => new OrdenCompraDetalleDto
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad,
                        Costo = d.Costo,
                        Subtotal = d.Subtotal
                    })
                    .ToList()
            };

            _response.Resultado = ordenCompraDto;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "Orden compra";
            return Ok(_response);
        }
    }
}
