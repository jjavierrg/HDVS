using System.Collections.Generic;

namespace EAPN.HDVS.Entities
{
    public class Perfil
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public IList<PerfilPermiso> Permisos { get; set; }
        public IList<UsuarioPerfil> Usuarios { get; set; }
    }
}
