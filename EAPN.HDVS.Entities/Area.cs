using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public int? IconoId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public Adjunto Icono { get; set; }
        public IList<Dimension> Dimensiones { get; set; }
    }
}
