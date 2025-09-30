using CapaDatos.Repositorio.Configuracion;
using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;

namespace Olympus.API.Extensions
{
    public static class InyeccionDependencias
    {
        public static IServiceCollection AgregarServiciosAplicacion(this IServiceCollection services)
        {
            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<IModuloRepository, ModuloRepository>();
            services.AddScoped<IFormularioRepository, FormularioRepository>();

            // Servicios de negocio
            services.AddScoped<ISEGModLoginService, SEGModLoginService>();
            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<ICFGModPermisosService, CFGModPermisosService>();

            // Aquí agregas todos los demás servicios

            return services;
        }
    }
}
