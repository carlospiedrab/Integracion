using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class OrdenCompraDto
    {
        public int ProveedorId { get; set; }
        public int BodegaId { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string UsuarioId { get; set; }
        public decimal TotalOrden { get; set; }
        public List<OrdenCompraDetalleDto> Detalles { get; set; }
    }
}
