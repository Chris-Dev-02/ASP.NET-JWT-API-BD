using Microsoft.EntityFrameworkCore;
using JWT_API_BD.Models;
using JWT_API_BD.Services.Interfaces;
using JWT_API_BD.Models.custom;

namespace JWT_API_BD.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly BasicUserAuthContext _authContext;
        public UserService(BasicUserAuthContext authContext)
        {
            _authContext = authContext;
        }
        public async Task<User> GetUserAsync(AuthorizationRequest authorization)
        {
            User foundUser = null;
            if (authorization.username is null)
            {
                foundUser = await _authContext.Users.Where(u => u.Email == authorization.email && u.Password == authorization.password).FirstOrDefaultAsync();
            }
            else
            {
                foundUser = await _authContext.Users.Where(u => u.UserName == authorization.username && u.Password == authorization.password).FirstOrDefaultAsync();
            }
            return foundUser;

            //throw new NotImplementedException();
        }

        public async Task<User> SaveUserAsync(User userModel)
        {
            _authContext.Add(userModel);
            await _authContext.SaveChangesAsync();
            return userModel;

            //throw new NotImplementedException();
        }
    }
}
