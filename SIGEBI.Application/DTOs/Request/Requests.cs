using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.DTOs.Request
{
    public record LoginRequest(string Email, string Password);

    public record CrearUsuarioRequest(
        int IdRol,
        string Nombre,
        string Apellido,
        string Email,
        string Password,
        string? Telefono,
        string? Direccion);

    public record ActualizarUsuarioRequest(
        string? Telefono,
        string? Direccion);

    public record CrearCategoriaRequest(
        string Nombre,
        string? Descripcion);

    public record ActualizarCategoriaRequest(
        string Nombre,
        string? Descripcion);

    public record CrearLibroRequest(
        int IdCategoria,
        string Codigo,
        string Titulo,
        string Autor,
        int Stock,
        string? ISBN,
        string? Editorial,
        int? NumeroPaginas);

    public record CrearRevistaRequest(
        int IdCategoria,
        string Codigo,
        string Titulo,
        string Autor,
        int Stock,
        string? ISBN,
        int? NumeroEdicion,
        string? Periodicidad);

    public record ActualizarRecursoRequest(
        string Titulo,
        string Autor,
        int IdCategoria,
        string? ISBN);

    public record AjustarStockRequest(int Cantidad);

    public record CrearPrestamoRequest(
        int IdUsuario,
        DateTime FechaLimite,
        List<DetallePrestamoRequest> Detalles);

    public record DetallePrestamoRequest(
        int IdRecurso,
        int Cantidad);

    public record CrearPenalizacionRequest(
        int IdUsuario,
        string Motivo,
        decimal Monto);
}
