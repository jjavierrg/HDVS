
using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Rango
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int Minimo { get; set; }
        public int? Maximo { get; set; }
    }
}
