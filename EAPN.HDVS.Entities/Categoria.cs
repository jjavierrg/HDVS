using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public int Orden { get; set; }
        public int DimensionId { get; set; }
        public string Descripcion { get; set; }
        public bool RespuestaMultiple { get; set; }
        public bool Obligatorio { get; set; }
        public bool Activo { get; set; }

        public Dimension Dimension { get; set; }
        public IList<Indicador> Indicadores { get; set; }
    }
}
