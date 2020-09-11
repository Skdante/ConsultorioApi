using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ConsultorioApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Controlador de Cuentas
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
        public CuentasController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Permite Crear un usuario nuevo
        /// </summary>
        /// <param name="model">Objeto de tipo <see cref="UserInfo"/></param>
        /// <returns>Objeto de tipo <see cref="UserToken"/></returns>
        [HttpPost("Crear")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser {
                UserName = model.Email,
                Email = model.Email,
                IsEnabled = true,
                Name = model.Name,
                JobTitle = model.JobTitle
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return BuildToken(model, new List<string>(), model.Email);
            }
            else
            {
                return BadRequest("Username or password invalid");
            }

        }

        /// <summary>
        /// Login para obtener el token mas reciente
        /// </summary>
        /// <param name="userInfo">Objeto de tipo <see cref="UserAccess"/></param>
        /// <returns>Objeto de tipo <see cref="UserToken"/></returns>
        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserAccess userInfo)
        {
            UserInfo user = new UserInfo() { Email = userInfo.Email, Password = userInfo.Password};
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var usuario = await _userManager.FindByEmailAsync(userInfo.Email);
                var roles = await _userManager.GetRolesAsync(usuario);
                return BuildToken(user, roles, usuario.UserName);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserList>> UserList([FromBody] UserFiltro userFilter)
        {
            var result = FilterUser(_userManager.Users, userFilter);
            if (result.Count() > 0)
                return Ok(result);
            else
                return NoContent();
        }

        private UserToken BuildToken(UserInfo userInfo, IList<string> roles, string username)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de una año.
            var expiration = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        private List<UserList> FilterUser(IQueryable<ApplicationUser> applicationUsers, UserFiltro userFilter)
        {
            List<UserList> userList = new List<UserList>();
            applicationUsers = applicationUsers.Where(x => x.Name.Contains(userFilter.Name));
            applicationUsers = applicationUsers.Where(x => x.Email.Contains(userFilter.Email));

            if (userFilter.EstatusId != 2 && userFilter.EstatusId == 1) {
                applicationUsers = applicationUsers.Where(x => x.IsEnabled == true);
            } else if (userFilter.EstatusId != 2 && userFilter.EstatusId == 0)
            {
                applicationUsers = applicationUsers.Where(x => x.IsEnabled == false);
            }

            foreach (var user in applicationUsers)
            {
                UserList userOnly = new UserList()
                {
                    Id = user.Id,
                    Name = user.Name,
                    JobTitle = user.JobTitle,
                    IsEnabled = user.IsEnabled,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber
                };

                userList.Add(userOnly);
            }
            return userList;
        }
    }
}