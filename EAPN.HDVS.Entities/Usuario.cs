using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Hash { get; set; }
        public int IntentosLogin { get; set; }
        public bool Activo { get; set; }
        public string Observaciones { get; set; }
        public DateTime? FinBloqueo { get; set; }
        public IEnumerable<UsuarioRol> RolesAdicionales { get; set; }
        public IEnumerable<UsuarioPerfil> Perfiles { get; set; }
        public IEnumerable<RefreshToken> Tokens { get; set; }

    }
}
