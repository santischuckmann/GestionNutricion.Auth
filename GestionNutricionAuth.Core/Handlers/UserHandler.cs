using GestionNutricionAuth.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionNutricionAuth.Core.Handlers
{
    public interface IUserHandler
    {
        public Task CreateUser(User user);
        public Task UpdateUser(User user);
    }
    public class UserHandler: IUserHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        } 

        public async Task CreateUser(User user)
        {
            await _unitOfWork.UserRepository.Agregar(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateUser(User user)
        {
            _unitOfWork.UserRepository.Actualizar(user);
            await _unitOfWork.SaveAsync();
        }
    }
}
