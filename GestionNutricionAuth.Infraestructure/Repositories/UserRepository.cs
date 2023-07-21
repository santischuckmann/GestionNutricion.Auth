using GestionNutricionAuth.Core;
using GestionNutricionAuth.Core.Entities;
using GestionNutricionAuth.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionNutricionAuth.Infraestructure.Repositories
{
    public class UserRepository: RepositorioBase<User>, IUserRepository
    {
        private readonly GestionNutricionAuthContext _context;
        public UserRepository(GestionNutricionAuthContext context): base(context) 
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _entities.Where(u => u.Username == username).FirstOrDefaultAsync();
        }
    }
}
