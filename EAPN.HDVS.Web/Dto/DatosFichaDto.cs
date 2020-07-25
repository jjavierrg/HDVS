using System;

namespace EAPN.HDVS.Web.Dto
{
    public class DatosFichaDto
    {
        public int Id { get; set; }
        public int? FotoId { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string DNI { get; set; }
        public string Codigo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Edad { get; set; }
        public string Sexo { get; set; }
        public string Genero { get; set; }
        public string Domicilio { get; set; }
        public string Municipio { get; set; }
        public string Provincia { get; set; }
        public string CP { get; set; }
        public string Padron { get; set; }
        public string Nacionalidad { get; set; }
        public string Origen { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Organizacion { get; set; }
        public string Tecnico { get; set; }
    }
}
