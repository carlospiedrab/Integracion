using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiValidacionErrorResponse : ApiErrorResponse
    {
        public ApiValidacionErrorResponse() : base(400)
        {

        }

        public IEnumerable<string> Errores { get; set; }
    }
}