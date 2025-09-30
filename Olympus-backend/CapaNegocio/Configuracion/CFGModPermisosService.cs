using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Servicio.Configuracion;
using Modelos.DTO.Configuracion;
using Modelos.Entidades;
using System.Collections.Generic;

namespace CapaNegocio.Configuracion
{
    public class CFGModPermisosService : ICFGModPermisosService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IErrorLogService _errorLogService;

        public CFGModPermisosService(IUnitOfWork unitOfWork, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _errorLogService = errorLogService;
        }

        /// <summary>
        /// Obtiene todas las áreas activas.
        /// </summary>
        public CFGModPermisosTAreaDTORPT ObtenerTodas()
        {
            var respuesta = new CFGModPermisosTAreaDTORPT();
            try
            {
                var datos = _unitOfWork.AreaRepository.ObtenerTodas();

                if (datos.Count() == 0)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_AREAS_PARA_MOSTRAR;
                    return respuesta;
                }

                respuesta.Area = datos.ToList();
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Obtiene todas las los modulos por Area.
        /// </summary>
        public CFGModPermisosTModuloDTORPT ObtenerPorAreas(List<int> idsAreas)
        {
            var respuesta = new CFGModPermisosTModuloDTORPT();
            try
            {
                var datos = _unitOfWork.ModuloRepository.ObtenerPorAreas(idsAreas);

                if (datos.Count() == 0)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_MODULOS_PARA_MOSTRAR;
                    return respuesta;
                }

                respuesta.Modulo = datos.ToList();
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Obtiene los formularios por Modulo.
        /// </summary>
        public CFGModPermisosTFormularioDTORPT ObtenerFormularioPorModulo(int idModulo)
        {
            var respuesta = new CFGModPermisosTFormularioDTORPT();
            try
            {
                var datos = _unitOfWork.FormularioRepository.ObtenerFormularioPorModulo(idModulo);

                if (datos.Count() == 0)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_MODULOS_PARA_MOSTRAR;
                    return respuesta;
                }

                respuesta.Formulario = datos.ToList();
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
