using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class Indicador
    {
        public int Id { get; set; }
        public int Orden { get; set; }
        public int CategoriaId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public int Puntuacion { get; set; }
        public string Sugerencias { get; set; }

        public Categoria Categoria { get; set; }
        public IList<IndicadorSeguimiento> Seguimientos { get; set; }
    }
}
