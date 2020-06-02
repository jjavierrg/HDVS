using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;

namespace EAPN.HDVS.Infrastructure.Context
{
    public class HDVSContext : DbContext
    {
        public virtual DbSet<Log> Logs { get; set; }

        public virtual DbSet<TipoAdjunto> TiposAdjunto { get; set; }
        public virtual DbSet<Adjunto> Adjuntos { get; set; }

        public virtual DbSet<Dimension> Dimensiones { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Indicador> Indicadores { get; set; }
        public virtual DbSet<IndicadorSeguimiento> IndicadoresSeguimientos { get; set; }
        public virtual DbSet<Seguimiento> Seguimientos { get; set; }

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuariosPerfiles { get; set; }
        public virtual DbSet<UsuarioPermiso> UsuariosPermisos { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet<PerfilPermiso> PerfilesPermisos { get; set; }
        public virtual DbSet<Permiso> Permisos { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public virtual DbSet<Organizacion> Organizaciones { get; set; }
        public virtual DbSet<Ficha> Fichas { get; set; }
        public virtual DbSet<SituacionAdministrativa> SituacionesAdministrativas { get; set; }
        public virtual DbSet<Empadronamiento> Empadronamientos { get; set; }
        public virtual DbSet<Sexo> Sexos { get; set; }
        public virtual DbSet<Pais> Paises { get; set; }
        public virtual DbSet<Provincia> Provincias { get; set; }
        public virtual DbSet<Municipio> Municipios { get; set; }

        public HDVSContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.LogConfiguration());

            modelBuilder.ApplyConfiguration(new Configurations.AdjuntoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TipoAdjuntoConfiguration());

            modelBuilder.ApplyConfiguration(new Configurations.DimensionConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.IndicadorConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.IndicadorSeguimientoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SeguimientoConfiguration());

            modelBuilder.ApplyConfiguration(new Configurations.UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UsuarioPerfilConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UsuarioPermisoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PerfilConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PerfilPermisoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PermisoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RefreshTokenConfiguration());

            modelBuilder.ApplyConfiguration(new Configurations.OrganizacionConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.EmpadronamientoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ProvinciaConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.MunicipioConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PaisConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.FichaConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SexoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SituacionAdministrativaConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
