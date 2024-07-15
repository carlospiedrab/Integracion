using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class CreateOrdenCompraDto
    {
        public int ProveedorId { get; set; }
        public int BodegaId { get; set; }
        public List<CreateOrdenCompraDetalleDto> Detalles { get; set; }
    }
}
