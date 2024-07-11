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
    public class CompaniaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompaniaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Compania
        [HttpGet]
        [Authorize(Policy = "AdminVendedorRol")]
        public async Task<ActionResult<IEnumerable<CompaniaDto>>> GetCompanias()
        {
            var companias = await _context.Companias.ToListAsync();
            var companiaDtos = companias.Select(c => new CompaniaDto
            {
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Direccion = c.Direccion,
                Telefono = c.Telefono,
                Email = c.Email,
                Estado = c.Estado
            }).ToList();

            return Ok(companiaDtos);
        }

        // GET: api/Compania/5
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminVendedorRol")]
        public async Task<ActionResult<CompaniaDto>> GetCompania(int id)
        {
            var compania = await _context.Companias.FindAsync(id);

            if (compania == null)
            {
                return NotFound();
            }

            var companiaDto = new CompaniaDto
            {
                Nombre = compania.Nombre,
                Descripcion = compania.Descripcion,
                Direccion = compania.Direccion,
                Telefono = compania.Telefono,
                Email = compania.Email,
                Estado = compania.Estado
            };

            return Ok(companiaDto);
        }

        // POST: api/Compania
        [HttpPost]
        [Authorize(Policy = "AdminRol")]
        public async Task<ActionResult<Compania>> CreateCompania(Compania compania)
        {
            _context.Companias.Add(compania);
            await _context.SaveChangesAsync();

            var companiaDto = new CompaniaDto
            {
                Nombre = compania.Nombre,
                Descripcion = compania.Descripcion,
                Direccion = compania.Direccion,
                Telefono = compania.Telefono,
                Email = compania.Email,
                Estado = compania.Estado
            };

            return CreatedAtAction(nameof(GetCompania), new { id = compania.CompaniaId }, companiaDto);
        }

        // PUT: api/Compania/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminRol")]
        public async Task<IActionResult> UpdateCompania(int id, Compania compania)
        {
            if (id != compania.CompaniaId)
            {
                return BadRequest();
            }

            var companiaExistente = await _context.Companias.FindAsync(id);

            if (companiaExistente == null)
            {
                return NotFound();
            }

            companiaExistente.Nombre = compania.Nombre;
            companiaExistente.Descripcion = compania.Descripcion;
            companiaExistente.Direccion = compania.Direccion;
            companiaExistente.Telefono = compania.Telefono;
            companiaExistente.Email = compania.Email;
            companiaExistente.Estado = compania.Estado;

            _context.Entry(companiaExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompaniaExists(id))
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

        // DELETE: api/Compania/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminRol")]
        public async Task<IActionResult> DeleteCompania(int id)
        {
            var compania = await _context.Companias.FindAsync(id);

            if (compania == null)
            {
                return NotFound();
            }

            _context.Companias.Remove(compania);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompaniaExists(int id)
        {
            return _context.Companias.Any(e => e.CompaniaId == id);
        }
    }

}