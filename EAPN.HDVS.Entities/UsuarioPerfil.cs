﻿namespace EAPN.HDVS.Entities
{
    public class UsuarioPerfil
    {
        public int UsuarioId { get; set; }
        public int PerfilId { get; set; }
        public Usuario Usuario { get; set; }
        public Perfil Perfil { get; set; }
    }
}
