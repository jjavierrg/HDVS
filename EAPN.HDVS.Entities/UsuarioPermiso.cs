namespace EAPN.HDVS.Entities
{
    public class UsuarioPermiso
    {
        public int UsuarioId { get; set; }
        public int PermisoId { get; set; }
        public Usuario Usuario { get; set; }
        public Permiso Permiso { get; set; }
    }
}
