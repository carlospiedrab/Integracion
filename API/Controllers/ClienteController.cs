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
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = "AdminVendedorRol")]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();
            var clienteDtos = clientes.Select(c => new ClienteDto
            {
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,
                Direccion = c.Direccion,
                Telefono = c.Telefono,
                Email = c.Email,
                Estado = c.Estado
            }).ToList();

            return Ok(clienteDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminVendedorRol")]
        public async Task<ActionResult<ClienteDto>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            var clienteDto = new ClienteDto
            {
                Nombres = cliente.Nombres,
                Apellidos = cliente.Apellidos,
                Direccion = cliente.Direccion,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Estado = cliente.Estado
            };

            return Ok(clienteDto);
        }

        [HttpPost]
        [Authorize(Policy = "AdminRol")]
        public async Task<ActionResult<Cliente>> CreateCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            var clienteDto = new ClienteDto
            {
                Nombres = cliente.Nombres,
                Apellidos = cliente.Apellidos,
                Direccion = cliente.Direccion,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Estado = cliente.Estado
            };

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteId }, clienteDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminRol")]
        public async Task<IActionResult> UpdateCliente(int id, Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return BadRequest();
            }

            var clienteExistente = await _context.Clientes.FindAsync(id);

            if (clienteExistente == null)
            {
                return NotFound();
            }

            clienteExistente.Nombres = cliente.Nombres;
            clienteExistente.Apellidos = cliente.Apellidos;
            clienteExistente.Direccion = cliente.Direccion;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Estado = cliente.Estado;

            _context.Entry(clienteExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clientes.Any(e => e.ClienteId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminRol")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}