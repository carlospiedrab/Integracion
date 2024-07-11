using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Entidades
{
    [Table("Companias")]
    public class Compania
    {
        [Key]
        public int CompaniaId {get;set;}
        public string Nombre {get;set;}
        public string Descripcion {get;set;}
        public string Direccion {get;set;}
        public string Telefono {get;set;}
        public string Email {get;set;}
        public bool Estado {get;set;}
    }
}