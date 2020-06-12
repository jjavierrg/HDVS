using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Adjunto
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

        public Ficha Ficha { get; set; }
        public Ficha FotoFicha { get; set; }
        public Usuario FotoUsuario{ get; set; }
        public Organizacion FotoOrganizacion { get; set; }
        public TipoAdjunto Tipo { get; set; }
    }
}
