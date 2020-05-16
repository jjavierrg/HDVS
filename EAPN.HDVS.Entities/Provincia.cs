using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Provincia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public IList<Municipio> Municipios { get; set; }
    }
}
