using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Dtos.User;
using Models.Entidades;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenServicio _tokenServicio;
        private ApiResponse _response;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public AccountController(UserManager<AppUser> userManager, ITokenServicio tokenServicio,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _tokenServicio = tokenServicio;
            _response = new();
            _roleManager = roleManager;
            _db = db;
        }

        [Authorize(Policy = "AdminRol")]
        [HttpGet]  // api/usuario
        public async Task<ActionResult> GetUsuarios()
        {
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();
            var usuarios = await _userManager.Users.Select(u => new UsuarioDto()
            {
                UserId = u.Id.ToString(),
                Username = u.UserName,
                LastName = u.LastName,
                FirstName = u.FirstName,
                Email = u.Email,
                Token = "NA"
            }).ToListAsync();
            foreach (var usuario in usuarios)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.UserId).RoleId;
                usuario.Rol = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }

            _response.Resultado = usuarios;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "Listado de Usuarios";
            return Ok(_response);
        }

        [Authorize(Policy = "AdminRol")]
        [HttpGet("{id}")]  // api/Account/Perfil
        public async Task<ActionResult> Perfil(string id)
        {
            var userRoles = await _db.UserRoles.Where(ur => ur.UserId == id).ToListAsync();
            var roles = await _db.Roles.ToListAsync();
            var usuario = await _userManager.Users.FirstOrDefaultAsync(w => w.Id == id);

            if (usuario == null)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Mensaje = "Usuario no encontrado";
                return NotFound(_response);
            }

            var usuarioRol = new UsuarioDto
            {
                UserId = usuario.Id.ToString(),
                Username = usuario.UserName,
                LastName = usuario.LastName,
                FirstName = usuario.FirstName,
                Email = usuario.Email,
                Token = "NA"
            };

            var rolesUsuario = userRoles.Select(ur => roles.FirstOrDefault(r => r.Id == ur.RoleId)?.Name).ToList();
            usuarioRol.Rol = string.Join(", ", rolesUsuario);

            _response.Resultado = usuarioRol;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "Perfil del Usuario";
            return Ok(_response);
        }



        [Authorize(Policy = "AdminRol")]
        [HttpPost("registro")]   // POST: api/usuario/registro
        public async Task<ActionResult<UsuarioDto>> Registro(AddOrUpdateAppUserDto registroDto)
        {
            if (await UsuarioExiste(registroDto.UserName)) return BadRequest("UserName ya esta Registrado");


            var usuario = new AppUser
            {
                UserName = registroDto.UserName.ToLower(),
                Email = registroDto.Email,
                LastName = registroDto.LastName,
                FirstName = registroDto.FirstName
            };

            var resultado = await _userManager.CreateAsync(usuario, registroDto.Password);
            if (!resultado.Succeeded) return BadRequest(resultado.Errors);

            var rolResultado = await _userManager.AddToRoleAsync(usuario, registroDto.Role);
            if (!rolResultado.Succeeded) return BadRequest("Error al Agregar el Rol al Usuario");

            return new UsuarioDto
            {
                Username = usuario.UserName,
                Token = await _tokenServicio.CrearToken(usuario)
            };
        }

        [HttpPost("login")] // POST: api/usuario/login
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto loginDto)
        {
            var usuario = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (usuario == null) return Unauthorized("Usuario no Valido");

            var resultado = await _userManager.CheckPasswordAsync(usuario, loginDto.Password);

            if (!resultado) return Unauthorized("Password no valido");

            return new UsuarioDto
            {
                UserId = usuario.Id.ToString(),
                Username = usuario.UserName,
                Email = usuario.Email,
                LastName = usuario.LastName,
                FirstName = usuario.FirstName,
                Rol = string.Join(",", _userManager.GetRolesAsync(usuario).Result.ToArray()),
                Token = await _tokenServicio.CrearToken(usuario)
            };
        }

        [Authorize(Policy = "AdminRol")]
        [HttpGet("ListadoRoles")]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.Select(r => new { NombreRol = r.Name }).ToList();
            _response.Resultado = roles;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "Listado de Roles";

            return Ok(_response);
        }

        private async Task<bool> UsuarioExiste(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}