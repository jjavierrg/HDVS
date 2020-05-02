using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Perfil
    {
        public int Id { get; set; }
        public string Descripcion{ get; set; }
        public IEnumerable<PerfilRol> Roles { get; set; }
    }
}
