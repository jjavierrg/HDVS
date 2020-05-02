using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Persona
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string DNI { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int SexoId { get; set; }
        public int NacionalidadId { get; set; }
        public bool DocumentacionEmpadronamiento { get; set; }

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

        public virtual IEnumerable<Direccion> Direcciones { get; set; }
        public virtual Sexo Sexo { get; set; }
        public virtual Pais Nacionalidad { get; set; }
    }
}
