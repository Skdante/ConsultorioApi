using AutoMapper;
using ConsultorioApi.Entities;

namespace ConsultorioApi.Web.Helper
{
    /// <summary>
    /// Clase AutoMapperPerfiles
    /// </summary>
    public class AutoMapperPerfiles : Profile
    {
        /// <summary>
        /// Constructor AutoMapperPerfiles
        /// </summary>
        public AutoMapperPerfiles() 
        {
            CreateMap<UserAccess, UserInfo>();
            CreateMap<ApplicationUser, UserList>();

            CreateMap<User, UserList>()
                .ForMember(x => x.IsEnabled, opt => opt.MapFrom(c => c.Estatus))
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(c => c.Puesto))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Nombre + " " + c.Apellido_paterno))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(c => c.Telefono))
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Email));

            CreateMap<User, UserList>()
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Nombre + " " + c.Apellido_paterno))
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(c => c.Puesto))
                .ForMember(x => x.IsEnabled, opt => opt.MapFrom(c => c.Estatus))
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Email))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(c => c.Telefono));

            CreateMap<UserInfo, ApplicationUser>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Email))
                .ForMember(x => x.IsEnabled, opt => opt.MapFrom(c => c.Estatus))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(c => c.Telefono))
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(c => c.Puesto));

            CreateMap<UserInfo, Persona>()
                .ForMember(x => x.Telefono_Contacto, opt => opt.MapFrom(c => c.Telefono))
                .ForMember(x => x.Imagen, opt => opt.Ignore());

            CreateMap<EstadoDB, Estado>()
                .ForMember(x => x.Estado_id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Estado_nombre, opt => opt.MapFrom(c => c.Nombre));
        }
    }
}
