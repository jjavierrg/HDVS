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
                .ForMember(d => d.Organizacion, opt => opt.MapFrom((src, dest) => src.Organizacion?.Nombre))
                .ForMember(d => d.OrganizacionFotoId, opt => opt.MapFrom((src, dest) => src.Organizacion?.FotoId))
                .ForMember(d => d.ClaveActual, o => o.Ignore());

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

            CreateMap<Ficha, FichaBusquedaDto>()
                .ForMember(d => d.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido1} {src.Apellido2}".Trim()))
                .ForMember(d => d.Organizacion, opt => opt.MapFrom((src, dest) => src.Organizacion?.Nombre))
                .ForMember(d => d.Tecnico, opt => opt.MapFrom((src, dest) => src.Tecnico?.NombreCompleto))
                .ForMember(d => d.UltimoSeguimientoId, opt => opt.MapFrom((src, dest) => src.Seguimientos?.OrderByDescending(x => x.Fecha).FirstOrDefault()?.Id))
                .ForMember(d => d.FechaUltimoSeguimiento, opt => opt.MapFrom((src, dest) => src.Seguimientos?.OrderByDescending(x => x.Fecha).FirstOrDefault()?.Fecha))
                .ForMember(d => d.PuntuacionUltimoSeguimiento, opt => opt.MapFrom((src, dest) => src.Seguimientos?.OrderByDescending(x => x.Fecha).FirstOrDefault()?.Indicadores?.Where(x => x.Indicador?.Activo ?? false).Sum(x => x.Indicador?.Puntuacion)));

            CreateMap<Ficha, VistaPreviaFichaDto>()
                .ForMember(d => d.FichaId, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.NombreOrganizacion, opt => opt.MapFrom((src, dest) => src.Organizacion?.Nombre))
                .ForMember(d => d.EmailOrganizacion, opt => opt.MapFrom((src, dest) => src.Organizacion?.Email))
                .ForMember(d => d.TelefonoOrganizacion, opt => opt.MapFrom((src, dest) => src.Organizacion?.Telefono))
                .ForMember(d => d.NombreTecnico, opt => opt.MapFrom((src, dest) => src.Tecnico?.Nombre));

            CreateMap<Ficha, DatosFichaDto>()
                .ForMember(d => d.Sexo, opt => opt.MapFrom((src, dest) => src.Sexo?.Descripcion))
                .ForMember(d => d.Genero, opt => opt.MapFrom((src, dest) => src.Genero?.Descripcion))
                .ForMember(d => d.Provincia, opt => opt.MapFrom((src, dest) => src.Provincia?.Nombre))
                .ForMember(d => d.Municipio, opt => opt.MapFrom((src, dest) => src.Municipio?.Nombre))
                .ForMember(d => d.Padron, opt => opt.MapFrom((src, dest) => src.Origen?.Descripcion))
                .ForMember(d => d.Origen, opt => opt.MapFrom((src, dest) => src.Origen?.Descripcion))
                .ForMember(d => d.Organizacion, opt => opt.MapFrom((src, dest) => src.Organizacion?.Nombre))
                .ForMember(d => d.Tecnico, opt => opt.MapFrom((src, dest) => src.Tecnico?.NombreCompleto))
                .ForMember(d => d.Nacionalidad, opt => opt.MapFrom((src, dest) => src.Nacionalidad?.Descripcion));

            CreateMap<Permiso, PermisoDto>().ReverseMap();
            CreateMap<UserToken, UserTokenDto>();

            CreateMap<Configuracion, ConfiguracionDto>().ReverseMap();
            CreateMap<Adjunto, AdjuntoDto>().ReverseMap();

            CreateMap<Dimension, DimensionDto>().ReverseMap();
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Indicador, IndicadorDto>().ReverseMap();
            CreateMap<Seguimiento, SeguimientoDto>()
                .ForMember(d => d.OrganizacionId, opt => opt.MapFrom((src, dest) => src.Ficha?.OrganizacionId ?? 0))
                .ReverseMap();

            CreateMap<IndicadorSeguimiento, IndicadorSeguimientoDto>().ReverseMap()
                .ForMember(d => d.Seguimiento, opt => opt.Ignore())
                .ForMember(d => d.Indicador, opt => opt.Ignore());

            CreateMap<Seguimiento, SeguimientoViewDto>()
                .ForMember(d => d.NombreTecnico, opt => opt.MapFrom((src, dest) => src.Tecnico?.NombreCompleto))
                .ForMember(d => d.Puntuacion, opt => opt.MapFrom((src, dest) => src.Indicadores?.Where(x => x.Indicador?.Activo ?? false).Sum(x => x.Indicador?.Puntuacion) ?? 0));

            CreateMap<Ficha, FichaDto>()
                .AfterMap((ficha, dto) => dto.Seguimientos = dto.Seguimientos.OrderByDescending(x => x.Fecha).ToList())
                .ReverseMap()
                .ForMember(x => x.Seguimientos, opt => opt.Ignore())
                .ForMember(x => x.Organizacion, opt => opt.Ignore())
                .ForMember(x => x.Tecnico, opt => opt.Ignore());
            CreateMap<Municipio, MunicipioDto>().ReverseMap();
            CreateMap<Provincia, ProvinciaDto>().ReverseMap();
            CreateMap<Pais, PaisDto>().ReverseMap();
            CreateMap<Empadronamiento, EmpadronamientoDto>().ReverseMap();
            CreateMap<Sexo, SexoDto>().ReverseMap();
            CreateMap<SituacionAdministrativa, SituacionAdministrativaDto>().ReverseMap();
            CreateMap<Rango, RangoDto>().ReverseMap();

            CreateMap<Municipio, MasterDataDto>()
                .ForMember(d => d.Descripcion, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<Provincia, MasterDataDto>()
                .ForMember(d => d.Descripcion, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<Empadronamiento, MasterDataDto>();
            CreateMap<Sexo, MasterDataDto>();
            CreateMap<Pais, MasterDataDto>();
            CreateMap<Perfil, MasterDataDto>();
            CreateMap<Permiso, MasterDataDto>();
            CreateMap<SituacionAdministrativa, MasterDataDto>();
            CreateMap<Usuario, MasterDataDto>()
                .ForMember(d => d.Descripcion, opt => opt.MapFrom(src => src.NombreCompleto));
            CreateMap<Organizacion, MasterDataDto>()
                .ForMember(d => d.Descripcion, opt => opt.MapFrom(src => src.Nombre));
        }
    }
}
