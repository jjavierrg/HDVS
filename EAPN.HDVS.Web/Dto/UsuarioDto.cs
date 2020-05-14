using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public int AsociacionId { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Clave { get; set; }
        public bool Activo { get; set; }
        public string Observaciones { get; set; }
        public DateTime? FinBloqueo { get; set; }
        public IList<MasterDataDto> Perfiles { get; set; }
        public IList<MasterDataDto> PermisosAdicionales { get; set; }
        public AsociacionDto Asociacion { get; set; }
    }
}
