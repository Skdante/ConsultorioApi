using AutoMapper;
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
using Serilog;

namespace ConsultorioApi.Core
{
    public class Cuentas : ICuentas
    {
        private readonly ICuentasRepositorio _cuentasRepositorio;
        private readonly IMapper _mapper;

        public Cuentas(ICuentasRepositorio cuentasRepositorio,
               IMapper mapper) 
        {
            _cuentasRepositorio = cuentasRepositorio;
            _mapper = mapper;
        }

        /// <summary>
        /// Guarda el usuario
        /// </summary>
        /// <param name="string">Identificador del usuario</param>
        /// <param name="usuario">Objeto tipo <see cref="UserInfo"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        public async Task<StatusProcessDB> SaveUser(string userId, UserInfo usuario)
        {
            StatusProcessDB statusProcess = new StatusProcessDB();
            StatusProcessDB status = new StatusProcessDB();
            Persona persona = new Persona();
            Doctor doctor = new Doctor();

            try
            {
                persona = _mapper.Map(usuario, persona);
                status = await _cuentasRepositorio.SaveUser(userId, persona);

                if (status.Estatus) 
                {
                    doctor.Persona_id = status.IdentificadoConfirmado;
                    doctor.Descripcion = usuario.Descripcion;
                    statusProcess = await _cuentasRepositorio.GuardarMedico(doctor);

                    if (statusProcess.Estatus)
                    {
                        statusProcess = await _cuentasRepositorio.GuardarUsuarioEspecialidad(statusProcess.IdentificadoConfirmado, usuario.Especialidades);
                    }
                }

                await InsertaRelacionEmpresaCuenta(userId, usuario.EmpresasRelacionadas);

            }
            catch (Exception ex)
            {
                Log.Error("Metodo SaveUser: {@usuario}", usuario, statusProcess);
                status.IdentificadoConfirmado = 0;
                status.Estatus = false;
                status.Mensaje = ex.Message;
            }
            return status;
        }

        /// <summary>
        /// Modifica el usuario
        /// </summary>
        /// <param name="string">Identificador del usuario</param>
        /// <param name="usuario">Objeto tipo <see cref="UserInfo"/></param>
        /// <param name="int">Identificador del la persona</param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        public async Task<StatusProcess> ModificarUsuario(string userId, UserInfo usuario, int personaId)
        {
            StatusProcessDB statusProcess = new StatusProcessDB();
            StatusProcessDB status = new StatusProcessDB();
            Persona persona = new Persona();
            Doctor doctor = new Doctor();

            try
            {
                persona = _mapper.Map(usuario, persona);
                persona.Persona_Id = personaId;
                status = await _cuentasRepositorio.ModificaUsuario(userId, persona);

                if (status.Estatus && usuario.RolId == "Medico")
                {
                    doctor.Persona_id = personaId;
                    doctor.Descripcion = usuario.Descripcion;
                    statusProcess = await _cuentasRepositorio.GuardarMedico(doctor);

                    if (statusProcess.Estatus)
                    {
                        statusProcess = await _cuentasRepositorio.GuardarUsuarioEspecialidad(statusProcess.IdentificadoConfirmado, usuario.Especialidades);
                    }
                }

                await InsertaRelacionEmpresaCuenta(userId, usuario.EmpresasRelacionadas);

            }
            catch (Exception ex)
            {
                Log.Error("Metodo ModificarUsuario: {@usuario}", usuario, statusProcess);
                status.IdentificadoConfirmado = 0;
                status.Estatus = false;
                status.Mensaje = ex.Message;
            }
            return status;
        }

        /// <summary>
        /// Inserta la relacion de la cuenta con las empresas seleccionadas
        /// </summary>
        /// <param name="userId">Identificador de Usuario</param>
        /// <param name="empresas">Lista de Empresas relacionadas <see cref="CompaniaInsert"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        public async Task<StatusProcess> InsertaRelacionEmpresaCuenta(string userId, List<CompaniaLista> empresas) 
        {
            StatusProcess statusProcess = new StatusProcess();

            try
            {
                statusProcess = await _cuentasRepositorio.InsertaRelacionEmpresaCuenta(userId, empresas);
            }
            catch (Exception ex)
            {
                Log.Error("Metodo InsertaRelacionEmpresaCuenta: {@empresas}", empresas, statusProcess);
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
        public async Task<List<UserList>> FilterUser(IQueryable<ApplicationUser> applicationUsers, UserFiltro userFilter)
        {
            List<User> userList = await _cuentasRepositorio.GetUser(userFilter);
            return _mapper.Map<List<UserList>>(userList);
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
                Log.Error("Metodo GetRol: {@rolLists}", rolLists);
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

                if (user.RolId != "Admin")
                {
                    user.EmpresasRelacionadas = await _cuentasRepositorio.GetUserEmpresa(id);
                    user.Especialidades = await _cuentasRepositorio.GetUserEspecialidad(user.Persona_Id);
                }
            }
            catch
            {
                user = new User();
                Log.Error("Metodo User: {@user}", user);
            }
            return user;
        }
    }
}
