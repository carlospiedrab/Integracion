using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException : ApiErrorResponse
    {
        public ApiException(int statusCode, string mensaje = null, string detalle = null) : base(statusCode, mensaje)
        {
            Detalle = detalle;
        }

        public string Detalle { get; set; }
    }
}