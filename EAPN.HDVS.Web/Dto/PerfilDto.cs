using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class PerfilDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int NumeroUsuarios { get; set; }
        public IList<MasterDataDto> Permisos{ get; set; }
    }
}
