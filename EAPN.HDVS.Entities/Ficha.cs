using System;

namespace EAPN.HDVS.Entities
{
    public class Ficha
    {
        public int Id { get; set; }
        public int AsociacionId { get; set; }
        public int UsuarioId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string DNI { get; set; }
        public bool FotocopiaDNI { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? SexoId { get; set; }
        public int? GeneroId { get; set; }
        public string Domicilio { get; set; }
        public string CP{ get; set; }
        public int? MunicipioId { get; set; }
        public int? ProvinciaId { get; set; }
        public int? PadronId { get; set; }
        public bool DocumentacionEmpadronamiento { get; set; }
        public int? NacionalidadId { get; set; }
        public int? OrigenId { get; set; }
        public int? SituacionAdministrativaId{ get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string MotivoAlta { get; set; }
        public string Observaciones { get; set; }
        public bool PoliticaFirmada { get; set; }
        public bool Completa { get; set; }

        public int? Edad
        {
            get
            {
                if (!FechaNacimiento.HasValue) return null;

                var today = DateTime.Now;
                var age = today.Year - FechaNacimiento.Value.Year;
                if (FechaNacimiento.Value.Date > today.AddYears(-age)) age--;

                return age;
            }
        }

        public Asociacion Asociacion { get; set; }
        public Usuario Tecnico { get; set; }
        public Sexo Sexo { get; set; }
        public Sexo Genero { get; set; }
        public Municipio Municipio { get; set; }
        public Provincia Provincia { get; set; }
        public Municipio Padron { get; set; }
        public Pais Nacionalidad { get; set; }
        public Pais Origen { get; set; }
    }
}
