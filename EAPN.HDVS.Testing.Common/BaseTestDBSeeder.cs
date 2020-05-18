using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Shared.Permissions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAPN.HDVS.Testing.Common
{
    public class BaseTestDBSeeder : ITestDBSeeder
    {
        private readonly string _defaultHash;

        public BaseTestDBSeeder(string _defaultUserHash)
        {
            _defaultHash = _defaultUserHash;
        }

        private Usuario CreateUsuario(int id, int asociacionId, int perfilId, bool superadmin, bool activo)
        {
            var permisosAdicionales = new List<UsuarioPermiso>();
            if (superadmin)
                permisosAdicionales.Add(new UsuarioPermiso { PermisoId = 6, UsuarioId = id });

            return new Usuario
            {
                Activo = activo,
                Apellidos = $"Apellidos",
                AsociacionId = asociacionId,
                Email = $"usuario{id}@test.com",
                Hash = _defaultHash,
                Id = id,
                Nombre = $"Nombre",
                Observaciones = $"asociacion {asociacionId} - activo {activo} - perfil {perfilId} - superadmin: {superadmin}",
                Perfiles = new List<UsuarioPerfil>(new[] { new UsuarioPerfil { PerfilId = perfilId, UsuarioId = id } }),
                PermisosAdicionales = permisosAdicionales
            };
        }

        public async Task Seed(HDVSContext context)
        {
            context.Permisos.Add(new Permiso { Id = 1, Clave = "--", Descripcion = "--" });
            context.Permisos.Add(new Permiso { Id = 2, Clave = Permissions.USERMANAGEMENT_READ, Descripcion = "Usuarios: Lectura" });
            context.Permisos.Add(new Permiso { Id = 3, Clave = Permissions.USERMANAGEMENT_WRITE, Descripcion = "Usuarios: Escritura" });
            context.Permisos.Add(new Permiso { Id = 4, Clave = Permissions.USERMANAGEMENT_DELETE, Descripcion = "Usuarios: Eliminar" });
            context.Permisos.Add(new Permiso { Id = 5, Clave = Permissions.USERMANAGEMENT_ACCESS, Descripcion = "Usuarios: Acceder" });
            context.Permisos.Add(new Permiso { Id = 6, Clave = Permissions.APP_SUPERADMIN, Descripcion = "Aplicación: Superadministrador" });
            context.Permisos.Add(new Permiso { Id = 7, Clave = Permissions.PESONALCARD_READ, Descripcion = "Fichas: Lectura" });
            context.Permisos.Add(new Permiso { Id = 8, Clave = Permissions.PESONALCARD_WRITE, Descripcion = "Fichas: Escritura" });
            context.Permisos.Add(new Permiso { Id = 9, Clave = Permissions.PESONALCARD_DELETE, Descripcion = "Fichas: Eliminar" });
            context.Permisos.Add(new Permiso { Id = 10, Clave = Permissions.PESONALCARD_ACCESS, Descripcion = "Fichas: Acceder" });

            context.Perfiles.Add(new Perfil { Id = 1, Descripcion = "Usuario" });
            context.Perfiles.Add(new Perfil { Id = 2, Descripcion = "Administrador" });

            context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 2, PermisoId = 2 });
            context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 2, PermisoId = 3 });
            context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 2, PermisoId = 4 });
            context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 2, PermisoId = 5 });
            context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 2, PermisoId = 7 });
            context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 2, PermisoId = 8 });
            context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 2, PermisoId = 9 });
            context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 2, PermisoId = 10 });

            context.Asociaciones.Add(new Asociacion { Activa = true, Id = 1, Nombre = $"Asociación activa 1", Observaciones = $"Observaciones Asociación activa 1" });
            context.Asociaciones.Add(new Asociacion { Activa = true, Id = 2, Nombre = $"Asociación activa 2", Observaciones = $"Observaciones Asociación activa 2" });
            context.Asociaciones.Add(new Asociacion { Activa = false, Id = 3, Nombre = $"Asociación inactiva 3", Observaciones = $"Observaciones Asociación inactiva 3" });

            // admins - activos
            context.Usuarios.Add(CreateUsuario(1, 1, 1, true, true));
            context.Usuarios.Add(CreateUsuario(2, 2, 1, true, true));
            context.Usuarios.Add(CreateUsuario(3, 3, 1, true, true)); // Asociacion Inactiva

            // usuarios - activos
            context.Usuarios.Add(CreateUsuario(4, 1, 1, false, true));
            context.Usuarios.Add(CreateUsuario(5, 2, 1, false, true));
            context.Usuarios.Add(CreateUsuario(6, 3, 1, false, true)); // Asociacion Inactiva

            // usuarios - inactivos
            context.Usuarios.Add(CreateUsuario(7, 1, 2, false, false));
            context.Usuarios.Add(CreateUsuario(8, 2, 2, false, false));
            context.Usuarios.Add(CreateUsuario(9, 3, 2, false, false)); // Asociacion Inactiva

            await context.SaveChangesAsync();
        }
    }
}