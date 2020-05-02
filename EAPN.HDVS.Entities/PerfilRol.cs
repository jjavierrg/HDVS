using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class PerfilRol
    {
        public int PerfilId { get; set; }
        public int RolId { get; set; }
        public Perfil Perfil { get; set; }
        public Rol Rol{ get; set; }
    }
}
