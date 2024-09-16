using JWT_API_BD.Models;
using JWT_API_BD.Models.custom;

namespace JWT_API_BD.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthorizationResponse> ReturnToken(User foundUser);
        Task<AuthorizationResponse> ReturnRefreshToken(RefreshTokenRequest refreshRequest, long idUser);
    }
}
