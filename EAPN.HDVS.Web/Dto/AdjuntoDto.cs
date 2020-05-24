using System;

namespace EAPN.HDVS.Web.Dto
{
    public class AdjuntoDto
    {
        public int Id { get; set; }
        public int TipoId { get; set; }
        public int? UsuarioId { get; set; }
        public int? FichaId { get; set; }
        public int? OrganizacionId { get; set; }
        public string Alias { get; set; }
        public string NombreOriginal { get; set; }
        public long Tamano { get; set; }
        public DateTime FechaAlta { get; set; }

        public string FullPath { get; set; }
    }
}
