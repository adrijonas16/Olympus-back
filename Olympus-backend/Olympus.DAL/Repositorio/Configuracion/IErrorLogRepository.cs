using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{ 
    public interface IErrorLogRepository
    {
        bool Insertar(ErrorLog modelo);
    }
}
