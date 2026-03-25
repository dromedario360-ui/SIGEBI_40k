using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.DTOs.Response
{
    public record LoginResponse(
        string Token,
        string NombreCompleto,
        string Email,
        string Rol);

    public record UsuarioResponse(
        int Id,
        string NombreCompleto,
        string Email,
        string Rol,
        string? Telefono,
        string? Direccion,
        bool Activo,
        DateTime FechaRegistro);

    public record RolResponse(
        int Id,
        string Nombre,
        string? Descripcion,
        bool Activo);

    public record CategoriaResponse(
        int Id,
        string Nombre,
        string? Descripcion,
        bool Activa);

    public record RecursoResponse(
        int Id,
        string Codigo,
        string Titulo,
        string Autor,
        string? ISBN,
        int Stock,
        bool Disponible,
        string Tipo,
        string Categoria,
        DateTime FechaRegistro);

    public record PrestamoResponse(
        int Id,
        int IdUsuario,
        string NombreUsuario,
        DateTime FechaPrestamo,
        DateTime FechaLimite,
        string Estado,
        decimal TotalMulta,
        List<DetallePrestamoResponse> Detalles);

    public record DetallePrestamoResponse(
        int IdRecurso,
        string TituloRecurso,
        int Cantidad);

    public record PenalizacionResponse(
        int Id,
        int IdUsuario,
        string NombreUsuario,
        string Motivo,
        decimal Monto,
        DateTime FechaInicio,
        DateTime? FechaFin,
        bool Activa);

    public record AuditoriaResponse(
        int Id,
        int? IdUsuario,
        string? NombreUsuario,
        string Accion,
        string? Descripcion,
        DateTime Fecha);
}
