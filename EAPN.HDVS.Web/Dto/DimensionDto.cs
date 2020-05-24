using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class DimensionDto
    {

        public int Id { get; set; }
        public int Orden { get; set; }
        public int? IconoId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public IList<CategoriaDto> Categorias { get; set; }
    }
}
