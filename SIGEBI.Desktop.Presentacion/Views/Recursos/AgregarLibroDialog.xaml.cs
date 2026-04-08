using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces;
using System.Windows;

namespace SIGEBI.Desktop.Presentacion.Views.Recursos
{
    public partial class AgregarLibroDialog : Window
    {
        private readonly IRecursoAppService _svc;

        public AgregarLibroDialog()
        {
            InitializeComponent();
            _svc = App.Services.GetRequiredService<IRecursoAppService>();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Visibility = Visibility.Collapsed;
            if (string.IsNullOrWhiteSpace(TxtCodigo.Text) || string.IsNullOrWhiteSpace(TxtTitulo.Text) || string.IsNullOrWhiteSpace(TxtAutor.Text))
            {
                TxtError.Text = "Codigo, Titulo y Autor son obligatorios.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }
            var idCat = int.TryParse(TxtCategoria.Text, out var c) ? c : 1;
            var stock = int.TryParse(TxtStock.Text, out var s) ? s : 1;
            var req = new CrearLibroRequest(idCat, TxtCodigo.Text, TxtTitulo.Text, TxtAutor.Text, stock, TxtISBN.Text, null, null);
            var r = await _svc.CrearLibroAsync(req);
            if (r.IsSuccess) { DialogResult = true; Close(); }
            else { TxtError.Text = r.Error; TxtError.Visibility = Visibility.Visible; }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}