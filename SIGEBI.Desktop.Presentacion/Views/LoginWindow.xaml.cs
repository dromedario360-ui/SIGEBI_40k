using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Interfaces;
using System.Windows;

namespace SIGEBI.Desktop.Presentacion.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e) => System.Windows.Application.Current.Shutdown();

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Visibility = Visibility.Collapsed;
            var email = TxtEmail.Text.Trim();
            var password = TxtPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TxtError.Text = "Por favor complete todos los campos.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            var usuarioRepo = App.Services.GetRequiredService<IUsuarioRepository>();
            var hasher = App.Services.GetRequiredService<IPasswordHasher>();

            var usuario = await usuarioRepo.ObtenerPorEmailAsync(email);
            if (usuario is null)
            {
                TxtError.Text = "Credenciales invalidas.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            if (!usuario.Activo)
            {
                TxtError.Text = "Usuario inactivo.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            if (usuario.IdRol == 3)
            {
                TxtError.Text = "Acceso no permitido para estudiantes.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            if (!hasher.Verify(password, usuario.PasswordHash))
            {
                TxtError.Text = "Credenciales invalidas.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            SessionManager.UsuarioId = usuario.Id;
            SessionManager.NombreUsuario = usuario.Nombre.NombreCompleto;
            SessionManager.RolId = usuario.IdRol;

            var main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}