using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime FinValidez { get; set; } 

        public virtual Usuario Usuario { get; set; }
    }
}
