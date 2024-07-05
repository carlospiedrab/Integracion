using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Entidades;

namespace Data.Interfaces
{
    public interface ITokenServicio
    {
        Task<string> CrearToken(AppUser usuario);
    }
}