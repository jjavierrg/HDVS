using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Entities
{
    public class Seguimiento
    {
        public int Id { get; set; }
        public int? UsuarioId { get; set; }
        public int FichaId { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public bool Completo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }

        public Ficha Ficha { get; set; }
        public Usuario Tecnico { get; set; }
        public IList<IndicadorSeguimiento> Indicadores { get; set; }
    }
}
