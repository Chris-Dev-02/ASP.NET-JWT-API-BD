using JWT_API_BD.Models;
using JWT_API_BD.Models.custom;
using JWT_API_BD.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWT_API_BD.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly BasicUserAuthContext _basicUserAuthContext;
        private readonly IConfiguration _configuration;

        public AuthService(BasicUserAuthContext basicUserAuthContext, IConfiguration configuration)
        {
            _basicUserAuthContext = basicUserAuthContext;
            _configuration = configuration;
        }

        private string GenerateToken(string idUser)
        {
            var key = _configuration.GetValue<string>("JwtSettings:Secret_Key");
            var byteKey = Encoding.UTF8.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUser));

            var tokenCredentials = new SigningCredentials(new SymmetricSecurityKey(byteKey), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = tokenCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokenCreated = tokenHandler.WriteToken(tokenConfig);

            return tokenCreated;
        }

        private string GenerateRefreshToken()
        {
            var byteArray = new byte[64];
            var refreshToken = "";

            using(var mg = RandomNumberGenerator.Create())
            {
                mg.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }
        
        private async Task<AuthorizationResponse> SaveRefreshTokenHistory(long idUser, string token, string refreshToken)
        {
            var refreshTokenHistory = new RefreshTokenHistory
            {
                IdUser = idUser,
                Token = token,
                RefreshToken = refreshToken,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(2)
            };

            await _basicUserAuthContext.RefreshTokenHistories.AddAsync(refreshTokenHistory);
            await _basicUserAuthContext.SaveChangesAsync();

            return new AuthorizationResponse { JWT = token, RefreshToken = refreshToken, Success = true, MSG = "OK" };
        }

        public async Task<AuthorizationResponse> ReturnToken(User foundUser)
        {
            if (foundUser == null)
            {
                return await Task.FromResult<AuthorizationResponse>(null);
            }

            string tokenCreated = GenerateToken(foundUser.IdUser.ToString());
            string refreshTokenCreated = GenerateRefreshToken();

            return await SaveRefreshTokenHistory(foundUser.IdUser, tokenCreated, refreshTokenCreated);
            //return new AuthorizationResponse() { JWT = tokenCreated, Success = true, MSG = "OK" };
            //throw new NotImplementedException();
        }

        public async Task<AuthorizationResponse> ReturnRefreshToken(RefreshTokenRequest refreshRequest, long idUser)
        {
            var foundRefreshToken = _basicUserAuthContext.RefreshTokenHistories.FirstOrDefault(x =>
            x.Token == refreshRequest.ExpiredToken &&
            x.RefreshToken == refreshRequest.RefreshToken &&
            x.IdUser == idUser);

            if (foundRefreshToken != null)
            {
                return new AuthorizationResponse { Success = false, MSG = "Refresh Token doesn't exist" };
            }

            var tokenCreated = GenerateToken(idUser.ToString());
            var refreshTokenCreated = GenerateRefreshToken();

            return await SaveRefreshTokenHistory(idUser, tokenCreated, refreshTokenCreated);
        }
    }
}
