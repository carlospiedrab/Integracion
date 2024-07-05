using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;

namespace Data.Inicializador
{
    public class DbInicializador : IDbInicializador
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public DbInicializador(ApplicationDbContext db, UserManager<AppUser> userManager,
         RoleManager<IdentityRole> rolManager)
        {
            _db = db;
            _userManager = userManager;
            _rolManager = rolManager;
        }

        public async void Inicializar()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate(); // Cuando se ejecuta por primera vez nuestra app y hay migraciones pendientes
                }
            }
            catch (Exception)
            {

                throw;
            }

            // Datos Iniciales
            // Crear Roles
            if (_db.Roles.Any(r => r.Name == "Admin")) return;

            _rolManager.CreateAsync(new IdentityRole { Name = "Admin" }).GetAwaiter().GetResult();
            _rolManager.CreateAsync(new IdentityRole { Name = "Vendedor" }).GetAwaiter().GetResult();
            _rolManager.CreateAsync(new IdentityRole { Name = "Cliente" }).GetAwaiter().GetResult();

            // Crear Usuario Administrador
            var usuario = new AppUser
            {
                UserName = "administrador",
                Email = "support@baezstone.com",
                LastName = "Piedra",
                FirstName = "Carlos"
            };
            try
            {
             var result =_userManager.CreateAsync(usuario, "Admin123").GetAwaiter().GetResult();
             if(!result.Succeeded){
                  throw (new Exception("Error " + result));
             }
            }
            catch (System.Exception ex)
            {

                throw (new Exception("Error " + ex.Message));
            }

            // Asignar el Rol Admin al usuario
            AppUser usuarioAdmin = _db.AppUser.Where(u => u.UserName == "administrador").FirstOrDefault();
             _userManager.AddToRoleAsync(usuarioAdmin, "Admin").GetAwaiter().GetResult();
             
        }
    }
}