using GestionNutricionAuth.Core;
using GestionNutricionAuth.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionNutricionAuth.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GestionNutricionAuthContext _context;
        private readonly IUserRepository _userRepository;

        public UnitOfWork(GestionNutricionAuthContext gestionNutricionAuthContext)
        {
            _context = gestionNutricionAuthContext;
        }

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

    public class RepositorioBase<T> : IRepositorio<T> where T : EntidadBase
    {
        #region Variables
        protected readonly GestionNutricionAuthContext _context;
        protected readonly DbSet<T> _entities;
        #endregion

        #region Constructor
        public RepositorioBase(GestionNutricionAuthContext context)
        {
            this._context = context ?? throw new System.ArgumentNullException(nameof(context));
            _entities = context.Set<T>();
        }
        #endregion

        public IQueryable<T> ObtenerTodosQuery()
        {
            return _entities;
        }

        public async Task<IEnumerable<T>> ObtenerTodos()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> ObtenerPorId(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Agregar(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task AgregarLista(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public void Actualizar(T entity)
        {
            _entities.Update(entity);
        }

        public void ActualizarLista(IEnumerable<T> entity)
        {
            _entities.UpdateRange(entity);
        }

        public async Task Eliminar(int id)
        {
            T entity = await ObtenerPorId(id);
            _entities.Remove(entity);
        }
    }
}
