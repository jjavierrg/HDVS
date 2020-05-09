using AutoMapper;
using EAPN.HDVS.Application.Models;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Web.Dto.Auth;
using System.Collections.Generic;
using System.Linq;

namespace EAPN.HDVS.Web.Dto
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(d => d.Clave, o => o.Ignore())
                .ForMember(d => d.Perfiles, opt => opt.MapFrom((src, dest) => src.Perfiles?.Select(x => x.Perfil).Distinct()))
                .ForMember(d => d.RolesAdicionales, opt => opt.MapFrom((src, dest) => src.RolesAdicionales?.Select(x => x.Rol).Distinct()))
                .ReverseMap()
                .ForMember(x => x.Hash, o => o.Ignore())
                .ForMember(d => d.Perfiles, opt => opt.MapFrom((src, dest) => src.Perfiles?.Select(x => new UsuarioPerfil { PerfilId = x.Id, UsuarioId = src.Id, Perfil = new Perfil { Id = x.Id, Descripcion = x.Descripcion } })))
                .ForMember(d => d.RolesAdicionales, opt => opt.MapFrom((src, dest) => src.RolesAdicionales?.Select(x => new UsuarioRol { RolId = x.Id, UsuarioId = src.Id, Rol = new Rol { Id = x.Id, Descripcion = x.Descripcion } })));

            CreateMap<Rol, RolDto>().ReverseMap();
            CreateMap<Perfil, PerfilDto>().ReverseMap();
            CreateMap<UserToken, UserTokenDto>();
        }
    }
}
