namespace EAPN.HDVS.Web.Dto
{
    public class DatosUsuarioDto
    {
        public int Id { get; set; }
        public int? FotoId { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string ClaveActual { get; set; }
        public string NuevaClave { get; set; }
        public int? OrganizacionFotoId { get; set; }
        public string Organizacion { get; set; }
    }
}
