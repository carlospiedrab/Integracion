using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class ProveedorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ApiResponse _response;

        public ProveedorController(ApplicationDbContext context)
        {
            _response = new();
            _context = context;
        }

        [Authorize(Policy = "AdminVendedorRol")]
        [HttpGet]
        public async Task<ActionResult<List<ProveedorDto>>> GetProveedores()
        {
            List<ProveedorDto> lista = await _context
                .Proveedores.Select(p => new ProveedorDto
                {
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Direccion = p.Direccion,
                    Telefono = p.Telefono,
                    Email = p.Email,
                    Estado = p.Estado == true ? 1 : 0
                })
                .ToListAsync();
            _response.Resultado = lista;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "Listado de proveedores";
            return Ok(_response);
        }

        [Authorize(Policy = "AdminVendedorRol")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDto>> GetProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null)
                return NotFound();

            _response.Resultado = new ProveedorDto
            {
                Nombre = proveedor.Nombre,
                Descripcion = proveedor.Descripcion,
                Direccion = proveedor.Direccion,
                Telefono = proveedor.Telefono,
                Email = proveedor.Email,
                Estado = proveedor.Estado == true ? 1 : 0
            };

            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "";

            return Ok(_response);
        }

        [Authorize(Policy = "AdminVendedorRol")]
        [HttpPost]
        public async Task<ActionResult<Proveedor>> PostProveedor(ProveedorDto proveedorDto)
        {
            try
            {
                Proveedor proveedor = new Proveedor()
                {
                    Nombre = proveedorDto.Nombre,
                    Descripcion = proveedorDto.Descripcion,
                    Direccion = proveedorDto.Direccion,
                    Telefono = proveedorDto.Telefono,
                    Email = proveedorDto.Email,
                    Estado = proveedorDto.Estado == 1 ? true : false
                };

                await _context.Proveedores.AddAsync(proveedor);
                await _context.SaveChangesAsync();

                _response.Resultado = proveedor;
                _response.IsExitoso = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Mensaje = "Proveedor ingresado con éxito";

                return Ok(_response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Policy = "AdminVendedorRol")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProveedor(int id, ProveedorDto proveedorDto)
        {
            var proveedorBd = await _context.Proveedores.FindAsync(id);

            if (proveedorBd == null)
                return NotFound();

            proveedorBd.Nombre = proveedorDto.Nombre;
            proveedorBd.Descripcion = proveedorDto.Descripcion;
            proveedorBd.Direccion = proveedorDto.Direccion;
            proveedorBd.Telefono = proveedorDto.Telefono;
            proveedorBd.Email = proveedorDto.Email;
            proveedorBd.Estado = proveedorDto.Estado == 1 ? true : false;

            await _context.SaveChangesAsync();

            _response.Resultado = proveedorBd;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "Proveedor actualizado con éxito";

            return Ok(_response);
        }

        [Authorize(Policy = "AdminVendedorRol")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProveedor(int id)
        {
            var proveedorBd = await _context.Proveedores.FindAsync(id);

            if (proveedorBd == null)
                return NotFound();

            _context.Proveedores.Remove(proveedorBd);

            await _context.SaveChangesAsync();

            _response.Resultado = null;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "Proveedor eliminado con éxito";
            return Ok(_response);
        }
    }
}
