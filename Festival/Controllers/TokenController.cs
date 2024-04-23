using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Festival.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Festival.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Festival.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public TokenController(IConfiguration config, ApplicationDbContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Users _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.Email, _userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.User_ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("User_ID", user.User_ID.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    DateTime expires;
                    if (user.RoleId == 3)
                    {
                        expires = DateTime.UtcNow.AddDays(1);
                    }
                    else
                    {
                        expires = DateTime.UtcNow.AddHours(1);
                    }
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: expires,
                        signingCredentials: signIn);

                    // create a new object that contains both the token and the user information
                    var response = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        user = new
                        {
                            user.User_ID,
                            user.FirstName,
                            user.LastName,
                            user.Avatar
                        }
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest("");
            }
        }

        private async Task<Users> GetUser(string email, string password)
        {
            Users user = new Users();
            return user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }


    }
}
