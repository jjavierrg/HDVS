using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Clave { get; set; }
        public bool Activo { get; set; }
        public string Observaciones { get; set; }
        public DateTime? FinBloqueo { get; set; }
        public IEnumerable<PerfilDto> Perfiles { get; set; }
        public IEnumerable<RolDto> RolesAdicionales { get; set; }
    }
}
