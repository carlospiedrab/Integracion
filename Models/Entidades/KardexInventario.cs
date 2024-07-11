using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Entidades
{
    [Table("KardexInventario")]
    public class KardexInventario
    {
        [Column("KardexInventarioId")]
        public int Id { get; set; }
        public int BodegaProductoId { get; set; }

        [ForeignKey("BodegaProductoId")]
        public BodegaProducto BodegaProducto { get; set; }
        public string Tipo { get; set; }
        public string Detalle { get; set; }
        public int StockAnterior { get; set; }
        public int Cantidad { get; set; }
        public decimal Costo { get; set; }
        public int Stock { get; set; }
        public decimal Total { get; set; }
        public string UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public AppUser AppUser { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
