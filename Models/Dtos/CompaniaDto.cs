using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class CompaniaDto
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public bool Estado { get; set; }
}
}