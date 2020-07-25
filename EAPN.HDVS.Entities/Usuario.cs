using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public int? FotoId { get; set; }
        public int OrganizacionId { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Hash { get; set; }
        public int IntentosLogin { get; set; }
        public bool Activo { get; set; }
        public string Observaciones { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellidos}".Trim();
        public DateTime? UltimoAcceso { get; set; }
        public DateTime? FinBloqueo { get; set; }
        public IList<UsuarioPermiso> PermisosAdicionales { get; set; }
        public IList<UsuarioPerfil> Perfiles { get; set; }
        public Organizacion Organizacion { get; set; }
        public DateTime FechaAlta { get; set; }
        public Adjunto Foto { get; set; }
    }
}
