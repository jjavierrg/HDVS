﻿namespace EAPN.HDVS.Entities
{
    public class IndicadorSeguimiento
    {
        public int IndicadorId { get; set; }
        public int SeguimientoId { get; set; }
        public string Observaciones { get; set; }
        public bool? Verificado { get; set; }

        public Indicador Indicador { get; set; }
        public Seguimiento Seguimiento { get; set; }
    }
}
