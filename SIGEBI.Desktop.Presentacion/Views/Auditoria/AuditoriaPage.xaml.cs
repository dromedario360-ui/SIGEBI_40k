using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using System.Windows.Controls;

namespace SIGEBI.Desktop.Presentacion.Views.Auditoria
{
    public partial class AuditoriaPage : Page
    {
        private readonly IAuditoriaAppService _svc;

        public AuditoriaPage()
        {
            InitializeComponent();
            _svc = App.Services.GetRequiredService<IAuditoriaAppService>();
            Loaded += async (s, e) => await CargarAsync();
        }

        private async System.Threading.Tasks.Task CargarAsync()
        {
            var r = await _svc.ObtenerTodasAsync();
            if (r.IsSuccess) DgAuditoria.ItemsSource = r.Value;
        }

        private async void BtnActualizar_Click(object sender, System.Windows.RoutedEventArgs e) => await CargarAsync();
    }
}