using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsultorioApi.Core;
using ConsultorioApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;


namespace ConsultorioApi.Web.Controllers
{
    /// <summary>
    /// Clase relacionado a los accesos del api
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Clase relacionado a los accesos del api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IAlmacenadorDeArchivos _almacenadorDeArchivos;
        private readonly ICuentas _cuentas;

        /// <summary>
        /// Controlador de Cuentas
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
        /// <param name="almacenadorDeArchivos"></param>
        /// <param name="cuentas"></param>
        public CuentasController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IAlmacenadorDeArchivos almacenadorDeArchivos,
            ICuentas cuentas)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _almacenadorDeArchivos = almacenadorDeArchivos;
            _cuentas = cuentas;
        }

        /// <summary>
        /// Permite crear un usuario
        /// </summary>
        /// <param name="model">Objeto de tipo <see cref="UserInfo"/></param>
        /// <returns>Objeto de tipo <see cref="UserToken"/></returns>
        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            if (!string.IsNullOrWhiteSpace(model.Imagen))
            {
                var fotoPersona = Convert.FromBase64String(model.Imagen);
                model.Imagen = await _almacenadorDeArchivos.GuardarArchivo(fotoPersona, "jpg", "personas");
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                IsEnabled = model.Estatus,
                Name = model.Nombre,
                JobTitle = model.Puesto,
                PhoneNumber = model.Telefono,
                Imagen = model.Imagen
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                try
                {
                    var usuario = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(usuario, model.RolId);
                    await _cuentas.InsertaRelacionEmpresaCuenta(user.Id, model.EmpresasRelacionadas);
                    return _cuentas.BuildToken(model, new List<string>(), model.Email, _configuration["JWT:key"]);
                } 
                catch (Exception ex)
                {
                    return BadRequest("Usuario creado exitosamente, pero se encontro un detalle: " + ex.Message);
                }
            }
            else
            {
                return BadRequest("Username or password invalid");
            }

        }

        /// <summary>
        /// Permite editar un usuario
        /// </summary>
        /// <param name="model">Objeto de tipo <see cref="UserInfo"/></param>
        /// <returns>Objeto de tipo <see cref="UserToken"/></returns>
        [HttpPut("Editar")]
        public async Task<ActionResult<UserToken>> EditarUser([FromBody] UserInfo model)
        {
            if (!string.IsNullOrWhiteSpace(model.Imagen))
            {
                var fotoPersona = Convert.FromBase64String(model.Imagen);
                model.Imagen = await _almacenadorDeArchivos.GuardarArchivo(fotoPersona, "jpg", "personas");
            }
            var usuario = await _userManager.FindByEmailAsync(model.Email);
            usuario.IsEnabled = model.Estatus;
            usuario.Name = model.Nombre;
            usuario.JobTitle = model.Puesto;
            usuario.PhoneNumber = model.Telefono;
            usuario.Imagen = model.Imagen == "" ? null : model.Imagen;
            
            var result = await _userManager.UpdateAsync(usuario);
            if (result.Succeeded)
            {
                try
                {
                    var roles = await _userManager.GetRolesAsync(usuario);

                    if (!String.IsNullOrWhiteSpace(model.Password))
                    {
                        await _userManager.RemovePasswordAsync(usuario);
                        await _userManager.AddPasswordAsync(usuario, model.Password);
                    }

                    if (String.IsNullOrWhiteSpace(model.RolId) && !roles.Contains(model.RolId))
                    {
                        await _userManager.RemoveFromRolesAsync(usuario, roles);
                        await _userManager.AddToRoleAsync(usuario, model.RolId);
                    }
                    
                    await _cuentas.InsertaRelacionEmpresaCuenta(usuario.Id, model.EmpresasRelacionadas);
                    
                    return _cuentas.BuildToken(model, new List<string>(), model.Email, _configuration["JWT:key"]);
                }
                catch (Exception ex)
                {
                    return BadRequest("Usuario editado exitosamente, pero se encontro un detalle: " + ex.Message);
                }
            }
            else
            {
                return BadRequest("Error al editar el usuario");
            }

        }

        /// <summary>
        /// Login para obtener el token mas reciente
        /// </summary>
        /// <param name="userInfo">Objeto de tipo <see cref="UserAccess"/></param>
        /// <returns>Objeto de tipo <see cref="UserToken"/></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserAccess userInfo)
        {
            UserInfo user = new UserInfo() { Email = userInfo.Email, Password = userInfo.Password};
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var usuario = await _userManager.FindByEmailAsync(userInfo.Email);
                var roles = await _userManager.GetRolesAsync(usuario);
                return _cuentas.BuildToken(user, roles, usuario.UserName, _configuration["JWT:key"]);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Invalido, revisar su usuario y contraseña.");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Muestra el listado de Usuarios de Accesos
        /// </summary>
        /// <param name="userFilter">Objeto de tipo <see cref="UserFiltro"/></param>
        /// <returns>Objeto de tipo <see cref="UserToken"/></returns>
        [HttpPost("UsersList")]
        public async Task<ActionResult<UserList>> UserList([FromBody] UserFiltro userFilter)
        {
            var result = _cuentas.FilterUser(_userManager.Users, userFilter);
            if (result.Count() > 0)
                return Ok(result);
            else
                return NoContent();
        }

        /// <summary>
        /// Muestra información del usuario
        /// </summary>
        /// <param name="id">Identificador del Usuario</param>
        /// <returns>Objeto de tipo <see cref="UserToken"/></returns>
        [HttpGet("User")]
        [AllowAnonymous]
        public async Task<ActionResult<UserList>> GetUser(string id)
        {
            var result = await _cuentas.User(id);
            return Ok(result);
        }

        /// <summary>
        /// Muestra el listado de los Roles
        /// </summary>
        /// <returns>Listado de objetos tipo <see cref="UserList"/></returns>
        [HttpGet("RolList")]
        public async Task<ActionResult<RolList>> RolList()
        {
            var result = await _cuentas.GetRol();
            if (result.Count() > 0)
                return Ok(result);
            else
                return NoContent();
        }
    }
}