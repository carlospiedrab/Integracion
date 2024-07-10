using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class ProductoNuevoDto
    {
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }
        public int CategoriaId { get; set; }
        public int MarcaId { get; set; }
        public bool Estado { get; set; }
    }
}