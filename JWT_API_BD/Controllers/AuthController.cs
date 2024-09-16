using JWT_API_BD.Models;
using JWT_API_BD.Models.custom;
using JWT_API_BD.Resources;
using JWT_API_BD.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace JWT_API_BD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService, IUserService userService) 
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> register([FromBody] User userModel)
        {
            userModel.Password = Utilities.encryptPassword(userModel.Password);
            User userCreated = await _userService.SaveUserAsync(userModel);

            var authorizationResult = await _authService.ReturnToken(userCreated);
            if (authorizationResult == null)
            {
                return Unauthorized();
            }

            return Ok(authorizationResult);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login([FromBody] AuthorizationRequest authorization)
        {
            authorization.password = Utilities.encryptPassword(authorization.password);
            User foundUser = await _userService.GetUserAsync(authorization);
            var authorizationResult = await _authService.ReturnToken(foundUser);
            if (authorizationResult == null)
            {
                return Unauthorized();
            }

            return Ok(authorizationResult);
        }
        
        [HttpPost]
        [Route("refresh_token")]
        public async Task<IActionResult> refreshToken([FromBody] RefreshTokenRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var supposedlyExpiredToken = tokenHandler.ReadJwtToken(request.ExpiredToken);

            if(supposedlyExpiredToken.ValidTo > DateTime.Now)
            {
                return BadRequest(new AuthorizationResponse { Success = false, MSG = "Token hasn't expired yet"});
            }

            string idUser = supposedlyExpiredToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

            var authorizationResponse = await _authService.ReturnRefreshToken(request, long.Parse(idUser));

            if(authorizationResponse.Success)
            {
                return Ok(authorizationResponse);
            }
            else
            {
                return BadRequest(authorizationResponse);
            }

        }
    }
}
