using Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class OrdenCompraDetalleDto
    {
        public int OrdenCompraDetalleId { get; set; }
        
        public int OrdenCompraId { get; set; }

        public int Producto { get; set; }

        public int Cantidad { get; set; }

        public decimal Costo { get; set; }

        public decimal Subtotal { get; set; }
    }
}