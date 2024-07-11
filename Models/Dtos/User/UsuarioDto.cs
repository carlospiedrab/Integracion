using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Dtos.User
{
    public class UsuarioDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }
    }
}