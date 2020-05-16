using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class ProvinciaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public IList<MunicipioDto> Municipios { get; set; }
    }
}
