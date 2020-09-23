using ConsultorioApi.DataAccess;
using ConsultorioApi.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ConsultorioApi.Core
{
    public class Cuentas : ICuentas
    {
        private readonly ICuentasRepositorio _cuentasRepositorio;

        public Cuentas(ICuentasRepositorio cuentasRepositorio) 
        {
            _cuentasRepositorio = cuentasRepositorio;
        }

        /// <summary>
        /// Inserta la relacion de la cuenta con las empresas seleccionadas
        /// </summary>
        /// <param name="userId">Identificador de Usuario</param>
        /// <param name="empresas">Lista de Empresas relacionadas <see cref="CompaniaInsert"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        public async Task<StatusProcess> InsertaRelacionEmpresaCuenta(string userId, List<CompaniaLista> empresas) 
        {
            StatusProcess statusProcess;

            try
            {
                statusProcess = await _cuentasRepositorio.InsertaRelacionEmpresaCuenta(userId, empresas);
            }
            catch (Exception ex)
            {
                statusProcess = new StatusProcess()
                {
                    Estatus = false,
                    Mensaje = ex.Message
                };
            }
            return statusProcess;
        }

        /// <summary>
        /// COnstruye el token en base a la informacion de usuario
        /// </summary>
        /// <param name="userInfo">Modelo tipo <see cref="UserInfo"/></param>
        /// <param name="roles">Listado de roles del usuario</param>
        /// <param name="username">Username del usuario</param>
        /// <param name="jwtKey">Llave del token</param>
        /// <returns>Modelo tipo <see cref="UserToken"/></returns>
        public UserToken BuildToken(UserInfo userInfo, IList<string> roles, string username, string jwtKey)
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
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

        /// <summary>
        /// Filtra la información del usuario
        /// </summary>
        /// <param name="applicationUsers">Listado del objeto <see cref="ApplicationUser"/></param>
        /// <param name="userFilter">Modelo tipo <see cref="UserFiltro"/></param>
        /// <returns>Listado del objeto <see cref="UserList"/></returns>
        public List<UserList> FilterUser(IQueryable<ApplicationUser> applicationUsers, UserFiltro userFilter)
        {
            List<UserList> userList = new List<UserList>();
            applicationUsers = applicationUsers.Where(x => x.Name.Contains(userFilter.Name));
            applicationUsers = applicationUsers.Where(x => x.Email.Contains(userFilter.Email));

            if (userFilter.EstatusId != 2 && userFilter.EstatusId == 1)
            {
                applicationUsers = applicationUsers.Where(x => x.IsEnabled == true);
            }
            else if (userFilter.EstatusId != 2 && userFilter.EstatusId == 0)
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

        /// <summary>
        /// Obtiene los Roles existentes
        /// </summary>
        /// <returns>Listado del objeto <see cref="RolList"/></returns>
        public async Task<List<RolList>> GetRol()
        {
            List<RolList> rolLists;

            try
            {
                rolLists = await _cuentasRepositorio.GetRol();
            }
            catch
            {
                rolLists = new List<RolList>();
            }
            return rolLists;
        }

        /// <summary>
        /// Obtiene información de un usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Objeto tipo <see cref="Entities.User"/></returns>
        public async Task<User> User(string id)
        {
            User user;

            try
            {
                user = await _cuentasRepositorio.GetUser(id);
                user.EmpresasRelacionadas = await _cuentasRepositorio.GetUserEmpresa(id);
            }
            catch
            {
                user = new User();
            }
            return user;
        }
    }
}
