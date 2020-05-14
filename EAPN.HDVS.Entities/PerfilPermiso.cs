using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class PerfilPermiso
    {
        public int PerfilId { get; set; }
        public int PermisoId { get; set; }
        public Perfil Perfil { get; set; }
        public Permiso Permiso{ get; set; }
    }
}
