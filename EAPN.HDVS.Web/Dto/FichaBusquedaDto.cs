using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class FichaBusquedaDto
    {
        public int Id { get; set; }
        public int UltimoSeguimientoId { get; set; }
        public int? FotoId { get; set; }
        public string Codigo { get; set; }
        public string NombreCompleto { get; set; }
        public string Organizacion { get; set; }
        public string Tecnico { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaUltimoSeguimiento{ get; set; }
        public int? PuntuacionUltimoSeguimiento { get; set; }
        public bool Competada { get; set; }
    }
}
