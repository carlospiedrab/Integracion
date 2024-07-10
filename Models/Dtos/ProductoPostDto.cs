using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class ProductoPostDto
    {
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }

        //public bool Estado { get; set; }

        public int CategoriaId { get; set; }
        public int MarcaId { get; set; }

    }
}