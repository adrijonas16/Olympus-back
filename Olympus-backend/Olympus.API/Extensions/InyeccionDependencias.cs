using CapaDatos.Repositorio.Configuracion;
using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using CapaNegocio.Servicio.Venta;

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
            services.AddScoped<IVTAModVentaPersonaService, VTAModVentaPersonaService>();
            services.AddScoped<IVTAModVentaAsesorService, VTAModVentaAsesorService>();
            services.AddScoped<IVTAModVentaEstadoService, VTAModVentaEstadoService>();
            services.AddScoped<IVTAModVentaControlOportunidadService, VTAModVentaControlOportunidadService>();
            services.AddScoped<IVTAModVentaOportunidadService, VTAModVentaOportunidadService>();
            services.AddScoped<IVTAModVentaHistorialEstadoService, VTAModVentaHistorialEstadoService>();
            services.AddScoped<IVTAModVentaHistorialInteraccionService, VTAModVentaHistorialInteraccionService>();
            services.AddScoped<IVTAModVentaPaisService, VTAModVentaPaisService>();
            services.AddScoped<IVTAModVentaLanzamientoService, VTAModVentaLanzamientoService>();
            services.AddScoped<IVTAModVentaEstadoTransicionService,VTAModVentaEstadoTransicionService>();
            services.AddScoped<IVTAModVentaCobranzaService, VTAModVentaCobranzaService>();
            services.AddScoped<IVTAModVentaPotencialClienteService, VTAModVentaPotencialClienteService>();
            return services;
        }
    }
}
