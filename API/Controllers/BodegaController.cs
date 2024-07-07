using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Entidades;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BodegaController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public BodegaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Bodega>> GetBodegas()
        {

            return await _context.Bodegas.ToListAsync();

        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<DtoBodega>> GetBodega( int Id )
        {

            Bodega bodega = await _context.Bodegas.FirstOrDefaultAsync( Bga => Bga.Id == Id );

            if( bodega == null )
                return NotFound();

            return Ok( new DtoBodega { Estado = Convert.ToBoolean(bodega.Estado) == false ? "Inactiva" : "Activa", NombreBodega = bodega.NombreBodega, Descripcion = bodega.Descripcion } );

        }

        [HttpPost]
        public async Task<ActionResult<Bodega>> NewBodega( Bodega bodega )
        {

            try
            {

                await _context.Bodegas.AddAsync( bodega );
                await _context.SaveChangesAsync();
                return CreatedAtAction( "GetBodega", new { Id = bodega.Id }, bodega );

            }
            catch (System.Exception)
            {

                throw;

            }

        }

        [HttpPut]
        public async Task<ActionResult<Bodega>> ModBodega( Bodega bodega )
        {

            try
            {

                if( bodega.Id == 0 ) return BadRequest();


                Bodega BDBga = await _context.Bodegas.FindAsync( bodega.Id );

                if( BDBga == null) return NotFound();

                BDBga.Estado = bodega.Estado;
                BDBga.NombreBodega = bodega.NombreBodega;
                BDBga.Descripcion = bodega.Descripcion;
                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (System.Exception)
            {

                throw;

            }

        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Bodega>> DelBodega( int Id )
        {

            try
            {

                if( Id == 0 ) return BadRequest();


                Bodega BDBga = await _context.Bodegas.FindAsync( Id );

                if( BDBga == null) return NotFound();
                
                _context.Bodegas.Remove( BDBga );
                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (System.Exception)
            {

                throw;

            }

        }

    }

}