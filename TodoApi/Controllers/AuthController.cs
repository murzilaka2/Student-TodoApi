using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApi.Helpers;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Utils;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private readonly IUser _users;

        public AuthController(IUser users)
        {
            _users = users;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("/api/checkAuthorization")]
        public IActionResult CheckAuthorization()
        {
            // Пользователь авторизован, так как он прошел проверку JwtBearer
            return Ok(); // Возвращает статус 200 OK, если пользователь авторизован
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            if (!await _users.IsUserExistsAsync(userDTO.Email))
            {
                return NotFound("User email not found");
            }
            string? salt = await _users.GetUserSaltAsync(userDTO.Email);
            if (salt == null)
            {
                return NotFound();
            }
            string hashedPassword = SecurityHelper.HashPassword(userDTO.Password, salt, 10101, 70);
            if (await _users.VerifyUserAsync(new User
            {
                Email = userDTO.Email,
                HashedPasssword = hashedPassword
            }))
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Email, userDTO.Email) };
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                return Json(new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            else
            {
                return NotFound("Wrong password");
            }
        }
    }
}
