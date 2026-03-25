using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Interfaces;
using SIGEBI.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IRepository<Rol> _rolRepo;
        private readonly IPasswordHasher _hasher;
        private readonly IAuditoriaRepository _auditoriaRepo;

        public UsuarioAppService(IUsuarioRepository usuarioRepo, IRepository<Rol> rolRepo,
                                  IPasswordHasher hasher, IAuditoriaRepository auditoriaRepo)
        {
            _usuarioRepo = usuarioRepo;
            _rolRepo = rolRepo;
            _hasher = hasher;
            _auditoriaRepo = auditoriaRepo;
        }

        public async Task<Result<UsuarioResponse>> CrearAsync(CrearUsuarioRequest req)
        {
            var existe = await _usuarioRepo.ExisteEmailAsync(req.Email);
            if (existe)
                return Result<UsuarioResponse>.Failure("El email ya está registrado.");

            Email email;
            NombrePersona nombre;
            try
            {
                email = new Email(req.Email);
                nombre = new NombrePersona(req.Nombre, req.Apellido);
            }
            catch (DomainException ex)
            {
                return Result<UsuarioResponse>.Failure(ex.Message);
            }

            var usuario = Usuario.Crear(req.IdRol, nombre, email, _hasher.Hash(req.Password));
            usuario.ActualizarPerfil(req.Telefono, req.Direccion);

            await _usuarioRepo.AgregarAsync(usuario);
            await _auditoriaRepo.AgregarAsync(
                Auditoria.Crear(null, "CREAR_USUARIO", $"Usuario creado: {email.Value}"));

            return Result<UsuarioResponse>.Success(await MapearAsync(usuario));
        }

        public async Task<Result<UsuarioResponse>> ObtenerPorIdAsync(int id)
        {
            var usuario = await _usuarioRepo.ObtenerPorIdAsync(id);
            if (usuario is null)
                return Result<UsuarioResponse>.Failure("Usuario no encontrado.");
            return Result<UsuarioResponse>.Success(await MapearAsync(usuario));
        }

        public async Task<Result<IEnumerable<UsuarioResponse>>> ObtenerTodosAsync()
        {
            var lista = await _usuarioRepo.ObtenerTodosAsync();
            var mapped = new List<UsuarioResponse>();
            foreach (var u in lista) mapped.Add(await MapearAsync(u));
            return Result<IEnumerable<UsuarioResponse>>.Success(mapped);
        }

        public async Task<Result> DesactivarAsync(int id)
        {
            var usuario = await _usuarioRepo.ObtenerPorIdAsync(id);
            if (usuario is null) return Result.Failure("Usuario no encontrado.");
            if (!usuario.Activo) return Result.Failure("El usuario ya está desactivado.");

            usuario.Desactivar();
            await _usuarioRepo.ActualizarAsync(usuario);
            await _auditoriaRepo.AgregarAsync(
                Auditoria.Crear(id, "DESACTIVAR_USUARIO", $"Usuario desactivado: {usuario.Email.Value}"));

            return Result.Success();
        }

        public async Task<Result> ActualizarPerfilAsync(int id, ActualizarUsuarioRequest req)
        {
            var usuario = await _usuarioRepo.ObtenerPorIdAsync(id);
            if (usuario is null) return Result.Failure("Usuario no encontrado.");

            usuario.ActualizarPerfil(req.Telefono, req.Direccion);
            await _usuarioRepo.ActualizarAsync(usuario);
            return Result.Success();
        }

        private async Task<UsuarioResponse> MapearAsync(Usuario u)
        {
            var rol = await _rolRepo.ObtenerPorIdAsync(u.IdRol);
            return new UsuarioResponse(u.Id, u.Nombre.NombreCompleto, u.Email.Value,
                                        rol?.Nombre ?? "", u.Telefono, u.Direccion,
                                        u.Activo, u.FechaRegistro);
        }
    }
}
