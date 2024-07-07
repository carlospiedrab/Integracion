using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entidades
{

    [Table("Bodega")]
    public class Bodega
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("BodegaId")]
        public int Id { get; set; }
        
        public bool Estado { get; set; }

        [MaxLength(120)]
        public string NombreBodega { get; set; }

        [MaxLength(160)]
        public string Descripcion { get; set; }

    }

}