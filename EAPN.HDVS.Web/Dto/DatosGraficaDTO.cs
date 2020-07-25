using System;

namespace EAPN.HDVS.Web.Dto
{
    public class DatosGraficaDTO
    {
        public int OrganizacionId { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? SexoId { get; set; }
        public int? GeneroId { get; set; }
        public int? MunicipioId { get; set; }
        public int? ProvinciaId { get; set; }
        public int? PadronId { get; set; }
        public int? NacionalidadId { get; set; }
        public int? OrigenId { get; set; }
        public int? SituacionAdministrativaId { get; set; }
        public string Sexo { get; set; }
        public string Genero { get; set; }
        public string Municipio { get; set; }
        public string Provincia { get; set; }
        public string Padron { get; set; }
        public string Nacionalidad { get; set; }
        public string Origen { get; set; }
        public string SituacionAdministrativa { get; set; }
        public int? RangoId { get; set; }
        public string Rango { get; set; }
    }
}
