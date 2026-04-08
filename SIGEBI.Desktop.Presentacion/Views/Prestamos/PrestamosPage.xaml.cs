using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace SIGEBI.Desktop.Presentacion.Views.Prestamos
{
    public partial class PrestamosPage : Page
    {
        private readonly IPrestamoAppService _svc;

        public PrestamosPage()
        {
            InitializeComponent();
            _svc = App.Services.GetRequiredService<IPrestamoAppService>();
            Loaded += async (s, e) => await CargarAsync();
        }

        private async System.Threading.Tasks.Task CargarAsync()
        {
            var r = await _svc.ObtenerTodosAsync();
            if (r.IsSuccess) DgPrestamos.ItemsSource = r.Value;
        }

        private async void BtnDevolucion_Click(object sender, RoutedEventArgs e)
        {
            if (DgPrestamos.SelectedItem == null) { MessageBox.Show("Seleccione un prestamo."); return; }
            dynamic selected = DgPrestamos.SelectedItem;
            var r = await _svc.ProcesarDevolucionAsync((int)selected.Id);
            if (r.IsSuccess) { MessageBox.Show("Devolucion procesada correctamente."); await CargarAsync(); }
            else MessageBox.Show(r.Error);
        }

        private async void BtnMarcarVencidos_Click(object sender, RoutedEventArgs e)
        {
            await _svc.MarcarVencidosAsync();
            await CargarAsync();
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e) => await CargarAsync();
    }
}