using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    [Table("OrdenCompra")]
    public class OrdenCompra
    {
        [Key]
        [Column("OrdenCompraId")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es Requerido")]
        public int ProveedorId { get; set; }

        [ForeignKey("ProveedorId")]
        public Proveedor Proveedor { get; set; }

        [Required(ErrorMessage = "{0} es Requerido")]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "{0} es Requerido")]
        [Column("Id",TypeName = "nvarchar(450)")]        
        public string UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public AppUser AppUser { get; set; }

        [Required(ErrorMessage = "{0} es Requerido")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalOrden { get; set; }
    }
}
