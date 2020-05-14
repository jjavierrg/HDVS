using EAPN.HDVS.Entities;
using Microsoft.EntityFrameworkCore;

namespace EAPN.HDVS.Infrastructure.Context
{
    public class HDVSContext : DbContext
    {
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuariosPerfiles{ get; set; }
        public virtual DbSet<UsuarioPermiso> UsuariosPermisos{ get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet<PerfilPermiso> PerfilesPermisos { get; set; }
        public virtual DbSet<Permiso> Permisos { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public virtual DbSet<Asociacion> Asociaciones { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<Direccion> Direcciones { get; set; }
        public virtual DbSet<Pais> Paises { get; set; }
        public virtual DbSet<Sexo> Sexos { get; set; }

        public HDVSContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UsuarioPerfilConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UsuarioPermisoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PerfilConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PerfilPermisoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PermisoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RefreshTokenConfiguration());

            modelBuilder.ApplyConfiguration(new Configurations.AsociacionConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.DireccionConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PaisConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PersonaConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SexoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
