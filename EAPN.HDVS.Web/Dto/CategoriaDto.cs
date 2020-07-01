using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class CategoriaDto
    {

        public int Id { get; set; }
        public int Orden { get; set; }
        public int DimensionId { get; set; }
        public string Descripcion { get; set; }
        public bool Obligatorio { get; set; }
        public bool Activo { get; set; }

        public IList<IndicadorDto> Indicadores { get; set; }
    }
}
