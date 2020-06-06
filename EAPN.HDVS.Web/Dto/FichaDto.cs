using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Web.Dto
{
    public class FichaDto
    {
        public int Id { get; set; }
        public int? FotoId { get; set; }
        public int OrganizacionId{ get; set; }
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

        public OrganizacionDto Organizacion { get; set; }
        public UsuarioDto Tecnico { get; set; }
        public SexoDto Sexo { get; set; }
        public SexoDto Genero { get; set; }
        public MunicipioDto Municipio { get; set; }
        public ProvinciaDto Provincia { get; set; }
        public EmpadronamientoDto Padron { get; set; }
        public PaisDto Nacionalidad { get; set; }
        public PaisDto Origen { get; set; }

        public IList<SeguimientoViewDto> Seguimientos { get; set; }
        public IList<AdjuntoDto> Adjuntos { get; set; }
    }
}
