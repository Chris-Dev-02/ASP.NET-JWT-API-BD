using Microsoft.EntityFrameworkCore;
using JWT_API_BD.Models;
using JWT_API_BD.Models.custom;

namespace JWT_API_BD.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(AuthorizationRequest authorization);

        Task<User> SaveUserAsync(User userModel);
    }
}
