using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.DomainServices;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Interfaces;
using SIGEBI.Infrastructure.Data;
using SIGEBI.Infrastructure.Repositories;
using SIGEBI.Infrastructure.Services;

namespace SIGEBI.API.Desktop.Extensions
{
    public static class ServiciosExtension
    {
        public static IServiceCollection AgregarServicios(
            this IServiceCollection services, IConfiguration config)
        {
            var connStr = config.GetConnectionString("PostgreSQL")!;
            services.AddSingleton(new ConexionDb(connStr));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<INotificacionService, NotificacionEmailService>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRecursoRepository, RecursoRepository>();
            services.AddScoped<IPrestamoRepository, PrestamoRepository>();
            services.AddScoped<IPenalizacionRepository, PenalizacionRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
            services.AddScoped<IRepository<Rol>, RolRepository>();
            services.AddScoped<IRepository<Categoria>, CategoriaRepository>();
            services.AddScoped<PrestamoDomainService>();
            services.AddScoped<PenalizacionDomainService>();
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            services.AddScoped<IRecursoAppService, RecursoAppService>();
            services.AddScoped<IPrestamoAppService, PrestamoAppService>();
            services.AddScoped<IPenalizacionAppService, PenalizacionAppService>();
            services.AddScoped<IAuditoriaAppService, AuditoriaAppService>();
            return services;
        }
    }
}