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
                .ForMember(d => d.PermisosAdicionales, opt => opt.MapFrom((src, dest) => src.PermisosAdicionales?.Select(x => x.Permiso).Distinct()))
                .ForMember(d => d.Perfiles, opt => opt.MapFrom((src, dest) => src.Perfiles?.Select(x => x.Perfil).Distinct()))
                .ReverseMap()
                .ForMember(x => x.Hash, o => o.MapFrom(src => src.Clave))
                .ForMember(d => d.Perfiles, opt => opt.MapFrom((src, dest) => src.Perfiles?.Select(x => new UsuarioPerfil { PerfilId = x.Id, UsuarioId = src.Id })))
                .ForMember(d => d.PermisosAdicionales, opt => opt.MapFrom((src, dest) => src.PermisosAdicionales?.Select(x => new UsuarioPermiso { PermisoId = x.Id, UsuarioId = src.Id })));

            CreateMap<Usuario, DatosUsuarioDto>()
                .ForMember(d => d.ClaveActual, o => o.Ignore())
                .ForMember(d => d.NuevaClave, o => o.Ignore());

            CreateMap<Perfil, PerfilDto>()
                .ForMember(d => d.Permisos, opt => opt.MapFrom((src, dest) => src.Permisos.Select(x => x.Permiso)))
                .ForMember(d => d.NumeroUsuarios, opt => opt.MapFrom((src, dest) => src.Usuarios?.Count ?? 0))
                .ReverseMap()
                .ForMember(d => d.Permisos, opt => opt.MapFrom((src, dest) => src.Permisos?.Select(x => new PerfilPermiso { PerfilId = src.Id, PermisoId = x.Id })));

            CreateMap<UsuarioPerfil, PerfilDto>()
                .ForMember(d => d.Descripcion, opt => opt.MapFrom((src, dest) => src.Perfil?.Descripcion))
                .ForMember(d => d.Id, opt => opt.MapFrom((src, dest) => src.Perfil?.Id))
                .ForMember(d => d.NumeroUsuarios, opt => opt.MapFrom(srr => 0))
                .ForMember(d => d.Permisos, opt => opt.MapFrom(srr => new Permiso[] { }));

            CreateMap<Organizacion, OrganizacionDto>()
                .ForMember(d => d.NumeroUsuarios, opt => opt.MapFrom((src, dest) => src.Usuarios?.Count() ?? 0))
                .ReverseMap();

            CreateMap<Ficha, VistaPreviaFichaDto>()
                .ForMember(d => d.FichaId, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.NombreOrganizacion, opt => opt.MapFrom((src, dest) => src.Organizacion?.Nombre))
                .ForMember(d => d.EmailOrganizacion, opt => opt.MapFrom((src, dest) => src.Organizacion?.Email))
                .ForMember(d => d.TelefonoOrganizacion, opt => opt.MapFrom((src, dest) => src.Organizacion?.Telefono))
                .ForMember(d => d.NombreTecnico, opt => opt.MapFrom((src, dest) => src.Tecnico?.Nombre));

            CreateMap<Permiso, PermisoDto>().ReverseMap();
            CreateMap<UserToken, UserTokenDto>();

            CreateMap<Adjunto, Adjunto>().ReverseMap().ForMember(d => d.FullPath, o => o.Ignore());

            CreateMap<Dimension, DimensionDto>().ReverseMap();
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Indicador, IndicadorDto>().ReverseMap();
            CreateMap<IndicadorFicha, IndicadorFichaDto>().ReverseMap();

            CreateMap<Ficha, FichaDto>().ReverseMap();
            CreateMap<Municipio, MunicipioDto>().ReverseMap();
            CreateMap<Provincia, ProvinciaDto>().ReverseMap();
            CreateMap<Pais, PaisDto>().ReverseMap();
            CreateMap<Sexo, SexoDto>().ReverseMap();

            CreateMap<Municipio, MasterDataDto>()
                .ForMember(d => d.Descripcion, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<Provincia, MasterDataDto>()
                .ForMember(d => d.Descripcion, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<Sexo, MasterDataDto>();
            CreateMap<Pais, MasterDataDto>();
            CreateMap<Perfil, MasterDataDto>();
            CreateMap<Permiso, MasterDataDto>();
            CreateMap<Organizacion, MasterDataDto>()
                .ForMember(d => d.Descripcion, opt => opt.MapFrom(src => src.Nombre));
        }
    }
}
