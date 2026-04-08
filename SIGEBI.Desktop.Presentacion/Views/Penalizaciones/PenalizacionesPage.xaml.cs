using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace SIGEBI.Desktop.Presentacion.Views.Penalizaciones
{
    public partial class PenalizacionesPage : Page
    {
        private readonly IPenalizacionAppService _svc;

        public PenalizacionesPage()
        {
            InitializeComponent();
            _svc = App.Services.GetRequiredService<IPenalizacionAppService>();
            Loaded += async (s, e) => await CargarAsync();
        }

        private async System.Threading.Tasks.Task CargarAsync()
        {
            var r = await _svc.ObtenerTodasAsync();
            if (r.IsSuccess) DgPenalizaciones.ItemsSource = r.Value;
        }

        private async void BtnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            if (DgPenalizaciones.SelectedItem == null) { MessageBox.Show("Seleccione una penalizacion."); return; }
            dynamic selected = DgPenalizaciones.SelectedItem;
            var r = await _svc.FinalizarAsync((int)selected.Id);
            if (r.IsSuccess) { MessageBox.Show("Penalizacion finalizada."); await CargarAsync(); }
            else MessageBox.Show(r.Error);
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e) => await CargarAsync();
    }
}