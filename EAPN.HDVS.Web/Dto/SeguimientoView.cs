using System;

namespace EAPN.HDVS.Web.Dto
{
    public class SeguimientoViewDto
    {
        public int Id { get; set; }
        public string NombreTecnico { get; set; }
        public DateTime Fecha { get; set; }
        public int Puntuacion { get; set; }
        public bool Completo { get; set; }
    }
}
