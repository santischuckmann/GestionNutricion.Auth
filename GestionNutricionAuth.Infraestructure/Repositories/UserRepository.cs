using GestionNutricionAuth.Core;
using GestionNutricionAuth.Core.Entities;

namespace GestionNutricionAuth.Api.Repositories
{
    public class UserRepository: RepositorioBase<User>, IUserRepository
    {
        private readonly GestionNutricionAuthContext _context;
        public UserRepository(GestionNutricionAuthContext context): base(context) 
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }
    }
}
