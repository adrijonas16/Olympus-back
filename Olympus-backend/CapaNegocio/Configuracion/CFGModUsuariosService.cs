using CapaDatos.DataContext;
using CapaDatos.Repositorio.Configuracion;
using CapaDatos.Repositorio.Venta;
using Modelos.DTO.Configuracion;
using Modelos.Entidades;

namespace CapaNegocio.Configuracion
{
    public class CFGModUsuariosService : ICFGModUsuariosService
    {
        private readonly OlympusContext _context;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPersonaRepository _personaRepo;
        private readonly IAsesorRepository _asesorRepo;
        private readonly IRolRepository _rolRepo;

        public CFGModUsuariosService(
            OlympusContext context,
            IUsuarioRepository usuarioRepo,
            IPersonaRepository personaRepo,
            IAsesorRepository asesorRepo,
            IRolRepository rolRepo)
        {
            _context = context;
            _usuarioRepo = usuarioRepo;
            _personaRepo = personaRepo;
            _rolRepo = rolRepo;
            _asesorRepo = asesorRepo;
        }

        public CFGModUsuariosTUsuarioDTORPT RegistrarUsuarioYPersona(CFGModUsuariosTUsuarioDTO modelo)
        {
            var respuesta = new CFGModUsuariosTUsuarioDTORPT();

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1. Crear usuario
                var usuario = new Usuario
                {
                    Nombre = modelo.NombreUsuario,
                    Correo = modelo.CorreoUsuario,
                    Password = modelo.Password,
                    IdRol = modelo.IdRol,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = 1, // ← ajustar según login real
                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = 1
                };

                _usuarioRepo.Insertar(usuario);
                _context.SaveChanges();

                int idUsuarioNuevo = usuario.Id;

                // 2. Crear persona
                var persona = new Persona
                {
                    IdPais = modelo.IdPais,
                    Nombres = modelo.Nombres,
                    Apellidos = modelo.Apellidos,
                    Correo = modelo.CorreoUsuario,
                    Celular = modelo.Celular,
                    PrefijoPaisCelular = modelo.PrefijoPaisCelular,
                    AreaTrabajo = modelo.AreaTrabajo,
                    Industria = modelo.Industria,
                    Estado = true,
                    IdUsuario = idUsuarioNuevo,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = "SYSTEM"
                };

                _personaRepo.Insertar(persona);
                _context.SaveChanges();

                int idPersonaNueva = persona.Id;

                if (modelo.IdRol == 1)
                {
                    var asesor = new Asesor
                    {
                        IdPersona = idPersonaNueva,
                        IdPais = modelo.IdPais,
                        Nombres = modelo.Nombres,
                        Apellidos = modelo.Apellidos,
                        Correo = modelo.CorreoUsuario,
                        Celular = modelo.Celular,
                        PrefijoPaisCelular = modelo.PrefijoPaisCelular,
                        AreaTrabajo = modelo.AreaTrabajo,
                        Estado = true,
                        Cesado = false,
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = "SYSTEM",
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = "SYSTEM"
                    };

                    _asesorRepo.Insertar(asesor);
                    _context.SaveChanges();
                }

                transaction.Commit();

                // Respuesta genérica OK
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
                respuesta.IdUsuario = idUsuarioNuevo;
                respuesta.IdPersona = persona.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = "Error al registrar: " + (ex.InnerException?.Message ?? ex.Message);
            }

            return respuesta;
        }

        public CFGModUsuariosListadoDTORPT ListarConUsuario()
        {
            var respuesta = new CFGModUsuariosListadoDTORPT();

            try
            {
                var lista = (from u in _context.Usuario
                             join p in _context.Persona on u.Id equals p.IdUsuario
                             join r in _context.Rol on u.IdRol equals r.Id
                             orderby u.FechaCreacion descending
                             select new CFGModUsuariosListadoDTO
                             {
                                 IdUsuario = u.Id,
                                 IdPersona = p.Id,
                                 Nombres = p.Nombres,
                                 Apellidos = p.Apellidos,
                                 Correo = u.Correo,
                                 Rol = r.NombreRol,
                                 IdRol = r.Id,
                                 IdPais = p.IdPais,
                                 Celular = p.Celular,
                                 PrefijoPaisCelular = p.PrefijoPaisCelular,
                                 AreaTrabajo = p.AreaTrabajo,
                                 Industria = p.Industria,
                                 Activo = u.Activo
                             }).ToList();

                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = "";
                respuesta.Usuarios = lista;
            }
            catch (Exception ex)
            {
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }

        public CFGModUsuariosTUsuarioDTORPT EditarUsuarioYPersona(CFGModUsuariosTUsuarioDTO modelo, int idUsuario)
        {
            var respuesta = new CFGModUsuariosTUsuarioDTORPT();

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1. Obtener usuario y persona
                var usuario = _usuarioRepo.ObtenerPorId(idUsuario);
                if (usuario == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Usuario no encontrado";
                    return respuesta;
                }

                var persona = _personaRepo.ObtenerPorIdUsuario(idUsuario);
                if (persona == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Persona no encontrada";
                    return respuesta;
                }

                // 2. Actualizar usuario
                usuario.Nombre = modelo.NombreUsuario;
                usuario.Correo = modelo.CorreoUsuario;
                if (!string.IsNullOrEmpty(modelo.Password))
                    usuario.Password = modelo.Password;
                usuario.IdRol = modelo.IdRol;
                usuario.FechaModificacion = DateTime.Now;
                usuario.UsuarioModificacion = 1; // ajustar según login real
                _usuarioRepo.Actualizar(usuario);

                // 3. Actualizar persona
                persona.Nombres = modelo.Nombres;
                persona.Apellidos = modelo.Apellidos;
                persona.IdPais = modelo.IdPais;
                persona.Celular = modelo.Celular;
                persona.PrefijoPaisCelular = modelo.PrefijoPaisCelular;
                persona.AreaTrabajo = modelo.AreaTrabajo;
                persona.Industria = modelo.Industria;
                persona.FechaModificacion = DateTime.Now;
                persona.UsuarioModificacion = "SYSTEM";
                _personaRepo.Actualizar(persona);

                _context.SaveChanges();
                transaction.Commit();

                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = "Usuario y persona actualizados correctamente";
                respuesta.IdUsuario = idUsuario;
                respuesta.IdPersona = persona.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = "Error al actualizar: " + ex.Message;
            }

            return respuesta;
        }

        public CFGModUsuariosTUsuarioDTORPT EliminarUsuarioYPersona(int idUsuario)
        {
            var respuesta = new CFGModUsuariosTUsuarioDTORPT();

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1. Obtener usuario
                var usuario = _usuarioRepo.ObtenerPorId(idUsuario);
                if (usuario == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Usuario no encontrado";
                    return respuesta;
                }

                var persona = _personaRepo.ObtenerPorIdUsuario(idUsuario);
                if (persona != null)
                {
                    _personaRepo.Eliminar(persona.Id);
                    _context.SaveChanges();
                }

                if (usuario.IdRol == 1)
                {
                    var asesor = _context.Asesor
                        .FirstOrDefault(a => a.IdPersona == persona.Id);

                    if (asesor != null)
                    {
                        _context.Asesor.Remove(asesor);
                        _context.SaveChanges();
                    }
                }

                _usuarioRepo.Eliminar(idUsuario);
                _context.SaveChanges();

                transaction.Commit();

                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = "Usuario, persona y asesor eliminados correctamente";
                respuesta.IdUsuario = idUsuario;
                respuesta.IdPersona = persona?.Id ?? 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = "Error al eliminar: " + ex.Message;
            }

            return respuesta;
        }


        public CFGModUsuariosTRolDTORPT ObtenerRoles()
        {
            var respuesta = new CFGModUsuariosTRolDTORPT();
            try
            {
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = "";
                respuesta.Rol =  _rolRepo.ObtenerTodas();

            }
            catch (Exception ex)
            {
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }
        public CFGModUsuarioPorRolDTORPT ObtenerUsuariosPorRol(int idRol)
        {
            var respuesta = new CFGModUsuarioPorRolDTORPT();
            try
            {
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = "";

                // Obtenemos los usuarios por rol y traemos a memoria
                var usuarios = _usuarioRepo.ObtenerPorRol(idRol).ToList();

                // Ahora hacemos el Select en memoria
                var listaUsuarios = usuarios
                    .Select(u =>
                    {
                        var persona = _personaRepo.ObtenerPorIdUsuario(u.Id);
                        return new CFGModUsuarioPorRolDTO
                        {
                            Id = u.Id,                     // id del usuario
                            IdPersona = persona?.Id ?? 0,  // regresamos IdPersona
                            Nombre = u.Nombre,
                            IdRol = u.IdRol
                        };
                    })
                    .ToList();

                respuesta.Usuarios = listaUsuarios;
            }
            catch (Exception ex)
            {
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }
    }
}
