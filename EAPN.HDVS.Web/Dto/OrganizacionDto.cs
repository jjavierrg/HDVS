namespace EAPN.HDVS.Web.Dto
{
    public class OrganizacionDto
    {
        public int Id { get; set; }
        public int? FotoId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Web { get; set; }
        public bool Activa { get; set; }
        public string Observaciones { get; set; }
        public int NumeroUsuarios { get; set; }
    }
}
