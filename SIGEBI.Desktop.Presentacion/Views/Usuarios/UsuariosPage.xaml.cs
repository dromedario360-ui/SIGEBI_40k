using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace SIGEBI.Desktop.Presentacion.Views.Usuarios
{
    public partial class UsuariosPage : Page
    {
        private readonly IUsuarioAppService _svc;

        public UsuariosPage()
        {
            InitializeComponent();
            _svc = App.Services.GetRequiredService<IUsuarioAppService>();
            Loaded += async (s, e) => await CargarAsync();
        }

        private async System.Threading.Tasks.Task CargarAsync()
        {
            var r = await _svc.ObtenerTodosAsync();
            if (r.IsSuccess) DgUsuarios.ItemsSource = r.Value;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new NuevoUsuarioDialog();
            dlg.Owner = Window.GetWindow(this);
            if (dlg.ShowDialog() == true) _ = CargarAsync();
        }

        private async void BtnDesactivar_Click(object sender, RoutedEventArgs e)
        {
            if (DgUsuarios.SelectedItem == null) { MessageBox.Show("Seleccione un usuario."); return; }
            dynamic selected = DgUsuarios.SelectedItem;
            var r = await _svc.DesactivarAsync((int)selected.Id);
            if (r.IsSuccess) { MessageBox.Show("Usuario desactivado."); await CargarAsync(); }
            else MessageBox.Show(r.Error);
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e) => await CargarAsync();
    }
}