using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class IndicadorFicha
    {
        public int IndicadorId { get; set; }
        public int FichaId { get; set; }
        public string Observaciones{ get; set; }

        public Indicador Indicador { get; set; }
        public Ficha Ficha { get; set; }
    }
}
