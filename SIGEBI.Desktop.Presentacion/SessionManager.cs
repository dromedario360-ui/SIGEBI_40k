namespace SIGEBI.Desktop.Presentacion
{
    public static class SessionManager
    {
        public static int UsuarioId { get; set; }
        public static string NombreUsuario { get; set; } = string.Empty;
        public static int RolId { get; set; }
        public static bool EsAdmin => RolId == 1;
        public static bool EsBibliotecario => RolId == 2;
    }
}