using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// Controlador de Cuentas
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
        /// <param name="almacenadorDeArchivos"></param>
        /// <param name="cuentas"></param>
        /// <param name="mapper"></param>
        public CuentasController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IAlmacenadorDeArchivos almacenadorDeArchivos,
            ICuentas cuentas,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _almacenadorDeArchivos = almacenadorDeArchivos;
            _cuentas = cuentas;
            _mapper = mapper;
        }

        /// <summary>
        /// Permite crear un usuario
        /// </summary>
        /// <param name="model">Objeto de tipo <see cref="UserInfo"/></param>
        /// <returns>Objeto de tipo <see cref="UserToken"/></returns>
        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            Persona persona = new Persona();
            var user = _mapper.Map(model, applicationUser);
            persona = _mapper.Map(model, persona);

            if (!string.IsNullOrWhiteSpace(model.Imagen))
            {
                var fotoPersona = Convert.FromBase64String(model.Imagen);
                persona.Imagen = await _almacenadorDeArchivos.GuardarArchivo(fotoPersona, "jpg", "personas");
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                try
                {
                    var userId = await _cuentas.SaveUser(user.Id, model);
                    var usuario = await _userManager.FindByEmailAsync(model.Email);
                    usuario.Persona_Id = userId.IdentificadoConfirmado;
                    await _userManager.UpdateAsync(usuario);
                    await _userManager.AddToRoleAsync(usuario, model.RolId);
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
            usuario = _mapper.Map(model, usuario);

            var result = await _userManager.UpdateAsync(usuario);
            if (result.Succeeded)
            {
                try
                {
                    var userId = await _cuentas.ModificarUsuario(usuario.Id, model, usuario.Persona_Id);
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

                    return _cuentas.BuildToken(model, new List<string>(), model.Email, _configuration["JWT:key"]);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Usuario editado exitosamente, pero se encontro un detalle: " + ex.Message);
                    return BadRequest(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error al editar el usuario");
                return BadRequest(ModelState);
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
            UserInfo user = new UserInfo();
            user = _mapper.Map(userInfo, user);
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
        /// <returns>Objeto de tipo <see cref="UserList"/></returns>
        [HttpPost("UsersList")]
        public async Task<ActionResult<UserList>> UserList([FromBody] UserFiltro userFilter)
        {
            var result = await _cuentas.FilterUser(_userManager.Users, userFilter);
            if (result.Count() > 0)
                return Ok(result);
            else
                return NoContent();
        }

        /// <summary>
        /// Muestra información del usuario
        /// </summary>
        /// <param name="id">Identificador del Usuario</param>
        /// <returns>Objeto de tipo <see cref="User"/></returns>
        [HttpGet("User")]
        public async Task<ActionResult<User>> GetUser(string id)
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