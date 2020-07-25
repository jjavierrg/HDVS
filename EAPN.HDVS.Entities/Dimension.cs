using System.Collections.Generic;

namespace EAPN.HDVS.Entities
{
    public class Dimension
    {
        public int Id { get; set; }
        public int Orden { get; set; }
        public int? IconoId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public Adjunto Icono { get; set; }
        public IList<Categoria> Categorias { get; set; }
    }
}
