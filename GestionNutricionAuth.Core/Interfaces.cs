using GestionNutricionAuth.Core.Entities;
using System.Collections.Generic;

namespace GestionNutricionAuth.Core
{
    public abstract class EntidadBase
    {
        public int Id { get; set; }
    }
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        void Save();
        Task SaveAsync();
    }
    public interface IUserRepository: IRepositorio<User>
    {
        public Task<User> GetUserByUsername(string username);
    }
    public interface IRepositorio<T> where T : EntidadBase
    {
        Task<IEnumerable<T>> ObtenerTodos();

        Task<T> ObtenerPorId(int id);

        Task Agregar(T entity);

        void Actualizar(T entity);

        Task Eliminar(int id);

        Task AgregarLista(IEnumerable<T> entities);

        void ActualizarLista(IEnumerable<T> entity);
        System.Linq.IQueryable<T> ObtenerTodosQuery();
    }
}