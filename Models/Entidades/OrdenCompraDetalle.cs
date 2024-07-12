using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    [Table("OrdenCompraDetalle")]
    public class OrdenCompraDetalle
    {
        [Key]
        [Column("OrdenCompraDetalleId")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es Requerido")]
        public int OrdenCompraId { get; set; }

        [ForeignKey("OrdenCompraId")]
        public OrdenCompra OrdenCompra { get; set; }

        [Required(ErrorMessage = "{0} es Requerido")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public ProductoProveedor ProductoProveedor { get; set; }

        [Required(ErrorMessage = "{0} es Requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor o igual a 0.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "{0} es Requerido")]
        [Column(TypeName = "decimal(18,0)")]
        public decimal Costo { get; set; }

        [Required(ErrorMessage = "{0} es Requerido")]
        [Column(TypeName = "decimal(18,0)")]
        public decimal Subtotal { get; set; }
    }
}
