using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.DomainServices;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Interfaces;
using SIGEBI.Infrastructure.Data;
using SIGEBI.Infrastructure.Repositories;

namespace SIGEBI.API.Desktop.Extensions
{
    public static class ServiciosExtension
    {
        public static IServiceCollection AgregarServicios(
            this IServiceCollection services, IConfiguration config)
        {
            // Conexión
            var connStr = config.GetConnectionString("PostgreSQL")!;
            services.AddSingleton(new ConexionDb(connStr));

            // Password Hasher
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Repositorios
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRecursoRepository, RecursoRepository>();
            services.AddScoped<IPrestamoRepository, PrestamoRepository>();
            services.AddScoped<IPenalizacionRepository, PenalizacionRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
            services.AddScoped<IRepository<Rol>, RolRepository>();
            services.AddScoped<IRepository<Categoria>, CategoriaRepository>();

            // Domain Services
            services.AddScoped<PrestamoDomainService>();
            services.AddScoped<PenalizacionDomainService>();

            // App Services
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            services.AddScoped<IRecursoAppService, RecursoAppService>();
            services.AddScoped<IPrestamoAppService, PrestamoAppService>();
            services.AddScoped<IPenalizacionAppService, PenalizacionAppService>();
            services.AddScoped<IAuditoriaAppService, AuditoriaAppService>();

            return services;
        }
    }
}
