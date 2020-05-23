using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Dimension
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public Area Area { get; set; }
        public IList<Indicador> Indicadores { get; set; }
    }
}
