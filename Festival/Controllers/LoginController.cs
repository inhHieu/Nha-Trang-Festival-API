using Festival.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Festival.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }

        [HttpPost, Route("login")]
        public async Task<ActionResult<int>> Login(Login login)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand("sp_login", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", login.Email);
            cmd.Parameters.AddWithValue("@password", login.Password);
            cmd.Parameters.AddWithValue("@stmttype", "userlogin");

            var userId = (int?)await cmd.ExecuteScalarAsync();

            if (userId.HasValue)
            {
                return Ok(userId);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}