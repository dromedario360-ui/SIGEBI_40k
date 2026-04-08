using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIGEBI.Desktop.Presentacion.Views.Recursos
{
    public partial class RecursosPage : Page
    {
        private readonly IRecursoAppService _svc;
        private List<object> _todos = new();

        public RecursosPage()
        {
            InitializeComponent();
            _svc = App.Services.GetRequiredService<IRecursoAppService>();
            Loaded += async (s, e) => await CargarAsync();
        }

        private async System.Threading.Tasks.Task CargarAsync()
        {
            var r = await _svc.ObtenerTodosAsync();
            if (r.IsSuccess)
            {
                _todos = r.Value!.Cast<object>().ToList();
                DgRecursos.ItemsSource = _todos;
            }
        }

        private void TxtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = TxtBusqueda.Text.ToLower();
            if (string.IsNullOrEmpty(texto))
                DgRecursos.ItemsSource = _todos;
            else
                DgRecursos.ItemsSource = _todos.Where(x => {
                    dynamic d = x;
                    return ((string)d.Titulo).ToLower().Contains(texto) ||
                           ((string)d.Autor).ToLower().Contains(texto) ||
                           ((string)d.Codigo).ToLower().Contains(texto);
                }).ToList();
        }

        private void BtnAgregarLibro_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AgregarLibroDialog();
            dlg.Owner = Window.GetWindow(this);
            if (dlg.ShowDialog() == true) _ = CargarAsync();
        }

        private void BtnAgregarRevista_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AgregarRevistaDialog();
            dlg.Owner = Window.GetWindow(this);
            if (dlg.ShowDialog() == true) _ = CargarAsync();
        }

        private async void BtnIncrementar_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            await _svc.AjustarStockAsync(id, new AjustarStockRequest(1), true);
            await CargarAsync();
        }

        private async void BtnReducir_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            await _svc.AjustarStockAsync(id, new AjustarStockRequest(1), false);
            await CargarAsync();
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e) => await CargarAsync();
    }
}