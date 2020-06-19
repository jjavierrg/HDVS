namespace EAPN.HDVS.Web.Dto
{
    public class IndicadorDto
    {

        public int Id { get; set; }
        public int Orden { get; set; }
        public int CategoriaId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public int Puntuacion { get; set; }
        public string Verificacion { get; set; }
    }
}
