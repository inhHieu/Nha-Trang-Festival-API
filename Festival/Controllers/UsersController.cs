using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Festival.Data;
using Festival.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Security.Claims;

namespace Festival.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public UsersController(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }


        //[HttpGet]
        //[Authorize(Roles = "1,2")]
        //public List<Users> GetUsers()
        //{
        //    SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
        //    conn.Open();

        //    try
        //    {
        //        List<Users> _listUsers = new List<Users>();

        //        if (conn.State != ConnectionState.Open) conn.Open();

        //        SqlCommand cmd = new SqlCommand("sp_users", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@stmttype", "getUsers");
        //        SqlDataReader _Reader = cmd.ExecuteReader();

        //        while (_Reader.Read())
        //        {
        //            Users users = new Users();
        //            users.User_ID = Convert.ToInt32(_Reader["User_ID"]);
        //            users.FirstName = _Reader["FirstName"].ToString();
        //            users.LastName = _Reader["LastName"].ToString();
        //            users.Phone = _Reader["Phone"].ToString();
        //            users.Address = _Reader["Address"].ToString();
        //            users.Email = _Reader["Email"].ToString();
        //            _listUsers.Add(users);
        //        }

        //        return _listUsers;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (conn != null)
        //        {
        //            if (conn.State == ConnectionState.Open)
        //            {
        //                conn.Close();
        //                conn.Dispose();
        //            }
        //        }
        //    }


        //}

        /*// GET: api/Users/phoneNumber
        [Route("phone")]
        [HttpGet("{phone}")]
        public async Task<ActionResult<Users>> GetUserByPhone(int phone)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();

            Users users = new Users();


            SqlCommand cmd = new SqlCommand("sp_getUserByPhone", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@stmttype", "getUsers");
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                users.UserName = dt.Rows[0]["UserName"].ToString();
                users.FullName = dt.Rows[0]["FullName"].ToString();
                users.Address = dt.Rows[0]["Address"].ToString();
                users.Email = dt.Rows[0]["Email"].ToString();
                users.Password = dt.Rows[0]["Password"].ToString();
                users.User_ID = (int)dt.Rows[0]["User_ID"];
                users.Phone = (int)dt.Rows[0]["Phone"];

            }
            else
            {
                return NotFound();
            }
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return users;
        } */


        //GET: api/Users/5


        [Authorize]//GET current user information
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            int jwtUserId = Convert.ToInt32(userIdClaim.Value);

            if (jwtUserId != id)
            {
                return Unauthorized();
            }
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();

                Users users = new Users();
                SqlCommand cmd = new SqlCommand("sp_users", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stmttype", "getUsersByID");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    users.User_ID = (int)dt.Rows[0]["User_ID"];
                    users.FirstName = dt.Rows[0]["FirstName"].ToString();
                    users.LastName = dt.Rows[0]["LastName"].ToString();
                    users.Address = dt.Rows[0]["Address"].ToString();
                    users.Email = dt.Rows[0]["Email"].ToString();
                    users.Password = dt.Rows[0]["Password"].ToString();
                    users.Phone = dt.Rows[0]["Phone"].ToString();
                    users.Sex = (bool)dt.Rows[0]["Sex"];
                    users.Age = (DateTime)dt.Rows[0]["Age"];
                    users.Avatar = dt.Rows[0]["Avatar"].ToString();
                }
                else
                {
                    return NotFound();
                }
                return users;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }

        }

        //[Authorize(Roles ="1,2")]
        //[Route("detail")]
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Users>> GetUsersDetail(int id)
        //{
        //    SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
        //    conn.Open();

        //    try
        //    {
        //        if (conn.State != System.Data.ConnectionState.Open) conn.Open();

        //        Users users = new Users();
        //        SqlCommand cmd = new SqlCommand("sp_users", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@id", id);
        //        cmd.Parameters.AddWithValue("@stmttype", "getUsersByID");
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        adapter.Fill(dt);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            users.User_ID = (int)dt.Rows[0]["User_ID"];
        //            users.FirstName = dt.Rows[0]["FirstName"].ToString();
        //            users.LastName = dt.Rows[0]["LastName"].ToString();
        //            users.Address = dt.Rows[0]["Address"].ToString();
        //            users.Email = dt.Rows[0]["Email"].ToString();
        //            users.Phone = dt.Rows[0]["Phone"].ToString();
        //            users.Password = dt.Rows[0]["Password"].ToString();
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //        return users;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (conn != null)
        //        {
        //            if (conn.State == ConnectionState.Open)
        //            {
        //                conn.Close();
        //                conn.Dispose();
        //            }
        //        }
        //    }

        //}

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [Authorize]//UPDATE current user information include sensitive info
        [HttpPut("{id}")]
        public ActionResult<int> PutUsers(int id, Users users)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            int jwtUserId = Convert.ToInt32(userIdClaim.Value);

            if (jwtUserId != id)
            {
                return StatusCode(401);
            }
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                if (UsersExists(id) == 0)
                {
                    return NotFound();
                }
                SqlCommand cmd = new SqlCommand("sp_users", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", users.User_ID);
                cmd.Parameters.AddWithValue("@firstname", users.FirstName);
                cmd.Parameters.AddWithValue("@lastname", users.LastName);
                cmd.Parameters.AddWithValue("@address", users.Address);
                cmd.Parameters.AddWithValue("@phone", users.Phone);
                cmd.Parameters.AddWithValue("@password", users.Password);
                cmd.Parameters.AddWithValue("@email", users.Email);
                cmd.Parameters.AddWithValue("@sex", users.Sex);
                cmd.Parameters.AddWithValue("@age", users.Age);
                cmd.Parameters.AddWithValue("@avatar", users.Avatar);
                cmd.Parameters.AddWithValue("@stmttype", "updateUsersByID");

                int result = Convert.ToInt32(cmd.ExecuteScalar());
                if (result == 1)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        //Register new user with defaul roleID (3-User)
        [HttpPost]
        public ActionResult<int> PostUsers(Users users)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_users", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@roleID", 3);
                cmd.Parameters.AddWithValue("@firstname", users.FirstName);
                cmd.Parameters.AddWithValue("@lastname", users.LastName);
                cmd.Parameters.AddWithValue("@address", users.Address);
                cmd.Parameters.AddWithValue("@phone", users.Phone);
                cmd.Parameters.AddWithValue("@password", users.Password);
                cmd.Parameters.AddWithValue("@email", users.Email);
                cmd.Parameters.AddWithValue("@sex", users.Sex);
                cmd.Parameters.AddWithValue("@age", users.Age);
                cmd.Parameters.AddWithValue("@avatar", users.Avatar);
                cmd.Parameters.AddWithValue("@stmttype", "createUsers");

                int result = Convert.ToInt32(cmd.ExecuteScalar());
                if (result > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            } 
        }


        private int UsersExists(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());


            if (conn.State != ConnectionState.Open) conn.Open();

            SqlCommand cmd = new SqlCommand("sp_users", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@stmttype", "isExist");
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }
    }
}
