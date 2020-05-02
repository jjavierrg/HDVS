using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Direccion
    {
        public int Id { get; set; }
        public int PersonaId { get; set; }
        public string Descripcion { get; set; }
        public string DireccionCompleta { get; set; }

        public virtual Persona Persona { get; set; }
    }
}
