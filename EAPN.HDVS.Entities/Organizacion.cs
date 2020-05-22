using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Entities
{
    public class Organizacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Web { get; set; }
        public bool Activa { get; set; }
        public string Observaciones { get; set; }
        public IList<Usuario> Usuarios { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
