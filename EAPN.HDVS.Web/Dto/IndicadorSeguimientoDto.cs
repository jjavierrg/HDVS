using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class IndicadorSeguimientoDto
    {
        public int IndicadorId { get; set; }
        public int SeguimientoId { get; set; }
        public string Observaciones { get; set; }
        public bool? Verificado { get; set; }

        public IndicadorDto Indicador { get; set; }
    }
}
