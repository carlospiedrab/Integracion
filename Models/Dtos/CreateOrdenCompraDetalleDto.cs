using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class CreateOrdenCompraDetalleDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Costo { get; set; }
    }
}
