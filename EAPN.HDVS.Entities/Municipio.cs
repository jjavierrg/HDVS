using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Municipio
    {
        public int Id { get; set; }
        public int ProvinciaId { get; set; }
        public string Nombre { get; set; }
        
        public Provincia Provincia { get; set; }
    }
}
