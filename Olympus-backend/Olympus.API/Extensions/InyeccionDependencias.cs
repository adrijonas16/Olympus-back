using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Servicio.Configuracion;

namespace Olympus.API.Extensions
{
    public static class InyeccionDependencias
    {
        public static IServiceCollection AgregarServiciosAplicacion(this IServiceCollection services)
        {
            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Servicios de negocio
            services.AddScoped<ISEGModLoginService, SEGModLoginService>();
            // Aquí agregas todos los demás servicios

            return services;
        }
    }
}
