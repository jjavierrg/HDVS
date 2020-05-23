using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Indicador
    {
        public int Id { get; set; }
        public int DimensionId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public int Puntuacion { get; set; }
        public string Sugerencias { get; set; }

        public Dimension Dimension { get; set; }
        public IList<IndicadorFicha> Fichas { get; set; }
    }
}
