using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Dto
{
    public class AsociacionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activa { get; set; }
        public string Observaciones { get; set; }
        public int NumeroUsuarios { get; set; }
    }
}
