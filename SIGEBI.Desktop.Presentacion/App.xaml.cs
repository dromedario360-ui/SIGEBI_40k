using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.DomainServices;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Interfaces;
using SIGEBI.Infrastructure.Data;
using SIGEBI.Infrastructure.Repositories;
using SIGEBI.Infrastructure.Services;
using System;
using System.Windows;

namespace SIGEBI.Desktop.Presentacion
{
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var collection = new ServiceCollection();
            ConfigureServices(collection);
            Services = collection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var connStr = "Host=localhost;Port=5432;Database=SIGEBI_DB;Username=postgres;Password=febrero13";
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
        }
    }
}