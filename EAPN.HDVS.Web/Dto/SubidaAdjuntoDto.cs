using Microsoft.AspNetCore.Http;
using System;

namespace EAPN.HDVS.Web.Dto
{
    public class SubidaAdjuntoDto
    {
        public int TipoId { get; set; }
        public int? FichaId { get; set; }
        public int? OrganizacionId { get; set; }
        public IFormFile File { get; set; }
    }
}
