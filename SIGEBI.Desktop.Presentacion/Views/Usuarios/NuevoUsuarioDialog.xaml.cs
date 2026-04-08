using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace SIGEBI.Desktop.Presentacion.Views.Usuarios
{
    public partial class NuevoUsuarioDialog : Window
    {
        private readonly IUsuarioAppService _svc;

        public NuevoUsuarioDialog()
        {
            InitializeComponent();
            _svc = App.Services.GetRequiredService<IUsuarioAppService>();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Visibility = Visibility.Collapsed;
            if (string.IsNullOrWhiteSpace(TxtNombre.Text) || string.IsNullOrWhiteSpace(TxtApellido.Text) ||
                string.IsNullOrWhiteSpace(TxtEmail.Text) || string.IsNullOrWhiteSpace(TxtPassword.Password))
            {
                TxtError.Text = "Todos los campos son obligatorios.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }
            var rolId = int.Parse(((ComboBoxItem)CmbRol.SelectedItem).Tag.ToString()!);
            var req = new CrearUsuarioRequest(rolId, TxtNombre.Text, TxtApellido.Text, TxtEmail.Text, TxtPassword.Password, null, null);
            var r = await _svc.CrearAsync(req);
            if (r.IsSuccess) { DialogResult = true; Close(); }
            else { TxtError.Text = r.Error; TxtError.Visibility = Visibility.Visible; }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}