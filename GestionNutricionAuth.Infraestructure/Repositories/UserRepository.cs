using GestionNutricionAuth.Core;
using GestionNutricionAuth.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionNutricionAuth.Api.Repositories
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
