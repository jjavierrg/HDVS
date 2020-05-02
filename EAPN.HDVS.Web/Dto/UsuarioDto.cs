using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public IEnumerable<RolDto> Roles { get; set; }
        public IEnumerable<PermisoDto> PermisosAdicionales { get; set; }
    }
}
