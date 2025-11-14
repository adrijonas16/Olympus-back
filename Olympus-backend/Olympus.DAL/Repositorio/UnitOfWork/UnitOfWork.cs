using CapaDatos.DataContext;
using CapaDatos.Repositorio.Configuracion;
using CapaDatos.Repositorio.Seguridad;
using CapaDatos.Repositorio.Venta;
using Microsoft.EntityFrameworkCore.Storage;

namespace CapaDatos.Repositorio.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OlympusContext _context;

        private IDbContextTransaction? _transaction;

        private UsuarioRepository _usuarioRepository;
        private ErrorLogRepository _errorLogRepository;
        private AreaRepository _areaRepository;
        private ModuloRepository _moduloRepository;
        private FormularioRepository _formularioRepository;
        private UserTokenRepository _userTokenRepository;
        private PersonaRepository _personaRepository;
        private AsesorRepository _asesorRepository;
        private EstadoRepository _estadoRepository;
        private OportunidadRepository _oportunidadRepository;
        private ControlOportunidadRepository _controlOportunidadRepository;
        private HistorialEstadoRepository _historialEstadoRepository;
        private HistorialInteraccionRepository _historialInteraccionRepository;
        private PaisRepository _paisRepository;
        private LanzamientoRepository _lanzamientoRepository;
        private BeneficioRepository _beneficioRepository;
        private CertificadoRepository _certificadoRepository;
        private CobranzaRepository _cobranzaRepository;
        private ConvertidoRepository _convertidoRepository;
        private CorporativoRepository _corporativoRepository;
        private DocenteRepository _docenteRepository;
        private HistorialEstadoTipoRepository _historialEstadoTipoRepository;
        private HorarioRepository _horarioRepository;
        private InversionDescuentoRepository _inversionDescuentoRepository;
        private ProductoRepository _productoRepository;
        private TipoRepository _tipoRepository;
        private OcurrenciaRepository _ocurrenciaRepository;
        private InversionRepository _inversionRepository;
        private MetodoPagoRepository _metodoPagoRepository;
        private MetodoPagoProductoRepository _metodoPagoProductoRepository;
        private ProductoDocenteRepository _productoDocenteRepository;
        private ProductoCertificadoRepository _productoCertificadoRepository;
        private VentaCruzadaRepository _ventaCruzadaRepository;
        private PotencialClienteRepository _potencialClienteRepository;
        private EstadoTransicionRepository _estadoTransicionRepository;
        public IUsuarioRepository UsuarioRepository
            => _usuarioRepository ??= new UsuarioRepository(_context);
        public IErrorLogRepository ErrorLogRepository
            => _errorLogRepository ??= new ErrorLogRepository(_context);
        public IAreaRepository AreaRepository
            => _areaRepository ??= new AreaRepository(_context);
        public IModuloRepository ModuloRepository
            => _moduloRepository ??= new ModuloRepository(_context);
        public IFormularioRepository FormularioRepository
            => _formularioRepository ??= new FormularioRepository(_context);
        public IUserTokenRepository UserTokenRepository
            => _userTokenRepository ??= new UserTokenRepository(_context);
        public IPersonaRepository PersonaRepository
            => _personaRepository ??= new PersonaRepository(_context);
        public IAsesorRepository AsesorRepository
            => _asesorRepository ??= new AsesorRepository(_context);
        public IEstadoRepository EstadoRepository
         => _estadoRepository ??= new EstadoRepository(_context);
        public IOportunidadRepository OportunidadRepository
            => _oportunidadRepository ??= new OportunidadRepository(_context);
        public IControlOportunidadRepository ControlOportunidadRepository
            => _controlOportunidadRepository ??= new ControlOportunidadRepository(_context);
        public IHistorialEstadoRepository HistorialEstadoRepository
            => _historialEstadoRepository ??= new HistorialEstadoRepository(_context);
        public IHistorialInteraccionRepository HistorialInteraccionRepository
            => _historialInteraccionRepository ??= new HistorialInteraccionRepository(_context);
        public IPaisRepository PaisRepository
            => _paisRepository ??= new PaisRepository(_context);
        public ILanzamientoRepository LanzamientoRepository
            => _lanzamientoRepository ??= new LanzamientoRepository(_context);
        public IBeneficioRepository BeneficioRepository
            => _beneficioRepository ??= new BeneficioRepository(_context);
        public ICertificadoRepository CertificadoRepository
            => _certificadoRepository ??= new CertificadoRepository(_context);
        public ICobranzaRepository CobranzaRepository
            => _cobranzaRepository ??= new CobranzaRepository(_context);
        public IConvertidoRepository ConvertidoRepository
            => _convertidoRepository ??= new ConvertidoRepository(_context);
        public ICorporativoRepository CorporativoRepository
            => _corporativoRepository ??= new CorporativoRepository(_context);
        public IDocenteRepository DocenteRepository
            => _docenteRepository ??= new DocenteRepository(_context);
        public IHistorialEstadoTipoRepository HistorialEstadoTipoRepository
            => _historialEstadoTipoRepository ??= new HistorialEstadoTipoRepository(_context);
        public IHorarioRepository HorarioRepository
            => _horarioRepository ??= new HorarioRepository(_context);
        public IInversionDescuentoRepository InversionDescuentoRepository
            => _inversionDescuentoRepository ??= new InversionDescuentoRepository(_context);
        public IProductoRepository ProductoRepository
            => _productoRepository ??= new ProductoRepository(_context);
        public ITipoRepository TipoRepository
            => _tipoRepository ??= new TipoRepository(_context);
        public IOcurrenciaRepository OcurrenciaRepository
            => _ocurrenciaRepository ??= new OcurrenciaRepository(_context);
        public IInversionRepository InversionRepository
            => _inversionRepository ??= new InversionRepository(_context);
        public IMetodoPagoRepository MetodoPagoRepository
            => _metodoPagoRepository ??= new MetodoPagoRepository(_context);
        public IMetodoPagoProductoRepository MetodoPagoProductoRepository
            => _metodoPagoProductoRepository ??= new MetodoPagoProductoRepository(_context);
        public IProductoDocenteRepository ProductoDocenteRepository
            => _productoDocenteRepository ??= new ProductoDocenteRepository(_context);
        public IProductoCertificadoRepository ProductoCertificadoRepository
            => _productoCertificadoRepository ??= new ProductoCertificadoRepository(_context);
        public IVentaCruzadaRepository VentaCruzadaRepository
            => _ventaCruzadaRepository ??= new VentaCruzadaRepository(_context);
        public IPotencialClienteRepository PotencialClienteRepository
            => _potencialClienteRepository ??= new PotencialClienteRepository(_context);
        public IEstadoTransicionRepository EstadoTransicionRepository
            => _estadoTransicionRepository ??= new EstadoTransicionRepository(_context);

        public UnitOfWork(OlympusContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_transaction != null)
                return _transaction;

            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null) return;

            try
            {
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null) return;

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

