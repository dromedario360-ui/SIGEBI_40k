using SIGEBI.Desktop.Presentacion.Views.Recursos;
using SIGEBI.Desktop.Presentacion.Views.Prestamos;
using SIGEBI.Desktop.Presentacion.Views.Penalizaciones;
using SIGEBI.Desktop.Presentacion.Views.Usuarios;
using SIGEBI.Desktop.Presentacion.Views.Auditoria;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIGEBI.Desktop.Presentacion.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TxtUsuario.Text = SessionManager.NombreUsuario;
            TxtRol.Text = SessionManager.EsAdmin ? "Administrador" : "Bibliotecario";
            if (!SessionManager.EsAdmin)
                BtnAuditoria.Visibility = Visibility.Collapsed;
            WindowState = WindowState.Maximized;
            MainFrame.Navigate(new RecursosPage());
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag.ToString();
            TxtPaginaActual.Text = tag;
            switch (tag)
            {
                case "Recursos": MainFrame.Navigate(new RecursosPage()); break;
                case "Prestamos": MainFrame.Navigate(new PrestamosPage()); break;
                case "Penalizaciones": MainFrame.Navigate(new PenalizacionesPage()); break;
                case "Usuarios": MainFrame.Navigate(new UsuariosPage()); break;
                case "Auditoria": MainFrame.Navigate(new AuditoriaPage()); break;
            }
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}