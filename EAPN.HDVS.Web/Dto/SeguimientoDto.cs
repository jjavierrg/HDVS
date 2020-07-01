using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class SeguimientoDto
    {
        public int Id { get; set; }
        public int OrganizacionId { get; set; }
        public int UsuarioId { get; set; }
        public int FichaId { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public bool Completo { get; set; }
        public bool? ForzadoIncompleto { get; set; }

        public IList<IndicadorSeguimientoDto> Indicadores { get; set; }
    }
}
