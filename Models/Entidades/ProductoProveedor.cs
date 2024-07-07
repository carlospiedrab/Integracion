using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class ProductoProveedor
    {
        [Key]
        public int ProductoProveedorId { get; set; }
        public int ProveedorId { get; set; }
        public int ProductoId { get; set; }

        [ForeignKey("ProveedorId")]
        public Proveedor Proveedor { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

    }
}