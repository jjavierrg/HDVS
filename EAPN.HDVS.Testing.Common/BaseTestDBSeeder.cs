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

        private Usuario CreateUsuario(int id, int organizacionId, int perfilId, bool superadmin, bool activo)
        {
            var permisosAdicionales = new List<UsuarioPermiso>();
            if (superadmin)
            {
                permisosAdicionales.Add(new UsuarioPermiso { PermisoId = 6, UsuarioId = id });
            }

            return new Usuario
            {
                Activo = activo,
                Apellidos = $"Apellidos",
                OrganizacionId = organizacionId,
                Email = $"usuario{id}@test.com",
                Hash = _defaultHash,
                Id = id,
                Nombre = $"Nombre",
                Observaciones = $"organización {organizacionId} - activo {activo} - perfil {perfilId} - superadmin: {superadmin}",
                Perfiles = new List<UsuarioPerfil>(new[] { new UsuarioPerfil { PerfilId = perfilId, UsuarioId = id } }),
                PermisosAdicionales = permisosAdicionales
            };
        }

        public async Task Seed(HDVSContext context)
        {
            context.Configuraciones.Add(new Configuracion { Id = 1, Enlaces = "", MostrarEnlaces = true });

            context.Permisos.Add(new Permiso { Id = 1, Clave = "--", Descripcion = "--" });
            context.Permisos.Add(new Permiso { Id = 2, Clave = Permissions.USERMANAGEMENT_READ, Descripcion = "Usuarios: Lectura" });
            context.Permisos.Add(new Permiso { Id = 3, Clave = Permissions.USERMANAGEMENT_WRITE, Descripcion = "Usuarios: Escritura" });
            context.Permisos.Add(new Permiso { Id = 4, Clave = Permissions.USERMANAGEMENT_DELETE, Descripcion = "Usuarios: Eliminar" });
            context.Permisos.Add(new Permiso { Id = 5, Clave = Permissions.USERMANAGEMENT_ACCESS, Descripcion = "Usuarios: Acceder" });
            context.Permisos.Add(new Permiso { Id = 6, Clave = Permissions.APP_SUPERADMIN, Descripcion = "Aplicación: Superadministrador" });
            context.Permisos.Add(new Permiso { Id = 7, Clave = Permissions.PERSONALCARD_READ, Descripcion = "Fichas: Lectura" });
            context.Permisos.Add(new Permiso { Id = 8, Clave = Permissions.PERSONALCARD_WRITE, Descripcion = "Fichas: Escritura" });
            context.Permisos.Add(new Permiso { Id = 9, Clave = Permissions.PERSONALCARD_DELETE, Descripcion = "Fichas: Eliminar" });
            context.Permisos.Add(new Permiso { Id = 10, Clave = Permissions.PERSONALCARD_ACCESS, Descripcion = "Fichas: Acceder" });
            context.Permisos.Add(new Permiso { Id = 11, Clave = Permissions.PERSONALATTACHMENTS_READ, Descripcion = "Adjuntos: Lectura" });
            context.Permisos.Add(new Permiso { Id = 12, Clave = Permissions.PERSONALATTACHMENTS_WRITE, Descripcion = "Adjuntos: Escritura" });
            context.Permisos.Add(new Permiso { Id = 13, Clave = Permissions.PERSONALATTACHMENTS_DELETE, Descripcion = "Adjuntos: Eliminar" });
            context.Permisos.Add(new Permiso { Id = 14, Clave = Permissions.PERSONALATTACHMENTS_ACCESS, Descripcion = "Adjuntos: Acceder" });
            context.Permisos.Add(new Permiso { Id = 15, Clave = Permissions.PERSONALINDICATORS_READ, Descripcion = "Indicadores: Lectura" });
            context.Permisos.Add(new Permiso { Id = 16, Clave = Permissions.PERSONALINDICATORS_WRITE, Descripcion = "Indicadores: Escritura" });
            context.Permisos.Add(new Permiso { Id = 17, Clave = Permissions.PERSONALINDICATORS_DELETE, Descripcion = "Indicadores: Eliminar" });
            context.Permisos.Add(new Permiso { Id = 18, Clave = Permissions.PERSONALINDICATORS_ACCESS, Descripcion = "Indicadores: Acceder" });

            context.Perfiles.Add(new Perfil { Id = 1, Descripcion = "Usuario" });
            context.Perfiles.Add(new Perfil { Id = 2, Descripcion = "Administrador" });
            context.Perfiles.Add(new Perfil { Id = 3, Descripcion = "UserManagement" });

            for (int i = 2; i <= 18; i++)
            {
                if (i != 6)
                {
                    context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 2, PermisoId = i });
                }
            }


            for (int i = 2; i <= 5; i++)
            {
                context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 3, PermisoId = i });
            }

            context.Organizaciones.Add(new Organizacion { Activa = true, Id = 1, Nombre = $"Organización activa 1", Observaciones = $"Observaciones Organización activa 1" });
            context.Organizaciones.Add(new Organizacion { Activa = true, Id = 2, Nombre = $"Organización activa 2", Observaciones = $"Observaciones Organización activa 2" });
            context.Organizaciones.Add(new Organizacion { Activa = false, Id = 3, Nombre = $"Organización inactiva 3", Observaciones = $"Observaciones Organización inactiva 3" });

            // usuarios admins - activos
            context.Usuarios.Add(CreateUsuario(1, 1, 2, true, true));
            context.Usuarios.Add(CreateUsuario(2, 2, 1, true, true));
            context.Usuarios.Add(CreateUsuario(3, 3, 1, true, true)); // Organización Inactiva

            // usuarios - activos
            context.Usuarios.Add(CreateUsuario(4, 1, 1, false, true));
            context.Usuarios.Add(CreateUsuario(5, 2, 1, false, true));
            context.Usuarios.Add(CreateUsuario(6, 3, 1, false, true)); // Organización Inactiva

            // usuarios - inactivos
            context.Usuarios.Add(CreateUsuario(7, 1, 2, false, false));
            context.Usuarios.Add(CreateUsuario(8, 2, 2, false, false));
            context.Usuarios.Add(CreateUsuario(9, 3, 2, false, false)); // Organización Inactiva

            // user management
            context.Usuarios.Add(CreateUsuario(10, 1, 3, false, true));
            context.Usuarios.Add(CreateUsuario(11, 1, 3, true, true));

            // usuarios fichas
            context.Usuarios.Add(CreateUsuario(12, 1, 2, false, true));

            context.TiposAdjunto.Add(new TipoAdjunto { Id = 1, Carpeta = "Imagenes", Descripcion = "images" });
            context.TiposAdjunto.Add(new TipoAdjunto { Id = 2, Carpeta = "Adjuntos", Descripcion = "attachments" });
            context.TiposAdjunto.Add(new TipoAdjunto { Id = 3, Carpeta = "Documentacion", Descripcion = "docs" });
            context.TiposAdjunto.Add(new TipoAdjunto { Id = 4, Carpeta = "Personales Ficha", Descripcion = "personal" });

            for (int i = 1; i < 11; i++)
            {
                // Dimension 5 y 10 no están activas
                context.Dimensiones.Add(new Dimension { Id = i, Activo = i % 5 > 0, Descripcion = $"Dimensión {i}", Orden = i });

                for (int j = 1; j < 11; j++)
                {
                    // Categorias 3, 6, 9 no están activas
                    context.Categorias.Add(new Categoria { Id = ((i - 1) * 10) + j, Activo = j % 3 > 0, Descripcion = $"Categoria {j}", Orden = j, DimensionId = i });

                    for (int k = 1; k < 11; k++)
                    {
                        // Indicadores 4 y 8 no están activos
                        context.Indicadores.Add(new Indicador { Id = (((i - 1) * 10) + j - 1) * 10 + k, Activo = k % 4 > 0, Descripcion = $"Indicador {k}", Orden = k, CategoriaId = j, Puntuacion = k });
                    }
                }
            }

            context.Empadronamientos.Add(new Empadronamiento { Id = 1, Descripcion = "Si" });
            context.Empadronamientos.Add(new Empadronamiento { Id = 2, Descripcion = "No" });
            context.Empadronamientos.Add(new Empadronamiento { Id = 3, Descripcion = "En Trámite" });

            context.Sexos.Add(new Sexo { Id = 1, Descripcion = "Hombre" });
            context.Sexos.Add(new Sexo { Id = 2, Descripcion = "Mujer" });
            context.Sexos.Add(new Sexo { Id = 3, Descripcion = "Otros" });

            context.SituacionesAdministrativas.Add(new SituacionAdministrativa { Id = 1, Descripcion = "Permiso de trabajo y residencia" });
            context.SituacionesAdministrativas.Add(new SituacionAdministrativa { Id = 2, Descripcion = "Permiso de residencia" });
            context.SituacionesAdministrativas.Add(new SituacionAdministrativa { Id = 3, Descripcion = "Situación irregular" });
            context.SituacionesAdministrativas.Add(new SituacionAdministrativa { Id = 4, Descripcion = "Protección internacional" });
            context.SituacionesAdministrativas.Add(new SituacionAdministrativa { Id = 5, Descripcion = "Solicitante de Protección internacional" });
            context.SituacionesAdministrativas.Add(new SituacionAdministrativa { Id = 6, Descripcion = "NIE ciudadanos comunitarios" });
            context.SituacionesAdministrativas.Add(new SituacionAdministrativa { Id = 7, Descripcion = "DNI" });

            for (int i = 1; i < 11; i++)
            {
                context.Paises.Add(new Pais { Id = i, Descripcion = $"Pais {i}" });
            }

            for (int i = 1; i < 11; i++)
            {
                context.Provincias.Add(new Provincia { Id = i, Nombre = $"Provincia {i}" });

                for (int j = 1; j < 11; j++)
                {
                    context.Municipios.Add(new Municipio { Id = ((i - 1) * 10) + j, Nombre = $"Municipio {j}", ProvinciaId = i });
                }
            }


            await context.SaveChangesAsync();
        }
    }
}