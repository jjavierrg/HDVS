using AutoMapper;
using EAPN.HDVS.Application.Models;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Web.Dto.Auth;

namespace EAPN.HDVS.Web.Dto
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Usuario, UsuarioDto>().ForMember(d => d.Clave, o => o.Ignore()).ReverseMap().ForMember(x => x.Hash, o => o.Ignore());
            CreateMap<Perfil, RolDto>().ReverseMap();
            CreateMap<Rol, PermisoDto>().ReverseMap();
            CreateMap<UserToken, UserTokenDto>();
        }
    }
}
