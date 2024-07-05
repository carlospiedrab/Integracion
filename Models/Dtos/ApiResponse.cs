using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class ApiResponse
    {
         public HttpStatusCode StatusCode { get; set; }  //  200, 400, 500
        public bool IsExitoso { get; set; }

        public string Mensaje { get; set; }
        public object Resultado { get; set; }  // List, Entidad
    }
}