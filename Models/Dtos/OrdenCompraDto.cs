using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class OrdenCompraDto
    {
        public int OrdenCompraId { get; set; }

        public int Proveedor { get; set; }        

        public DateTime FechaIngreso { get; set; }

        public string Usuario { get; set; }        

        public decimal TotalOrden { get; set; }
    }
}