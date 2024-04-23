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

namespace Festival.Controllers.AdminController
{
    [Authorize(Roles = "1,2")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public AdminUsersController(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        //----------------------------------------------------------------
        //Get All User
        [HttpGet]
        public List<UserDetail> GetUsersDetail(int offset, int limit)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<UserDetail> _listUsers = new List<UserDetail>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_users", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stmttype", "getUsers");
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    UserDetail users = new UserDetail();
                    users.User_ID = Convert.ToInt32(_Reader["User_ID"]);
                    users.RoleId = Convert.ToInt32(_Reader["RoleID"]);
                    users.FirstName = _Reader["FirstName"].ToString();
                    users.LastName = _Reader["LastName"].ToString();
                    users.Phone = _Reader["Phone"].ToString();
                    users.Address = _Reader["Address"].ToString();
                    users.Email = _Reader["Email"].ToString();
                    users.Password = _Reader["Password"].ToString();
                    users.total_subscriptions = Convert.ToInt32(_Reader["total_subscriptions"]);
                    users.top_category_1 = _Reader["top_category_1"].ToString();
                    users.top_category_2 = _Reader["top_category_2"].ToString();
                    users.top_category_3 = _Reader["top_category_3"].ToString();
                    users.Sex = Convert.ToBoolean(_Reader["Sex"]);
                    if (_Reader["Age"] != DBNull.Value)
                    {
                        users.Age = Convert.ToDateTime(_Reader["Age"]);
                    }
                    else
                    {
                        users.Age = DateTime.Today; // or some other default value that makes sense in yourcontext
                    }
                    users.Avatar = _Reader["Avatar"].ToString();
                    _listUsers.Add(users);
                }

                return _listUsers;
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
        //----------------------------------------------------------------
        //Find User
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetail>> GetUsers(int id)
        {

            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();

                UserDetail users = new UserDetail();
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
                    users.RoleId = (int)dt.Rows[0]["RoleID"];
                    users.FirstName = dt.Rows[0]["FirstName"].ToString();
                    users.LastName = dt.Rows[0]["LastName"].ToString();
                    users.Address = dt.Rows[0]["Address"].ToString();
                    users.Email = dt.Rows[0]["Email"].ToString();
                    users.Phone = dt.Rows[0]["Phone"].ToString();
                    users.Password = dt.Rows[0]["Password"].ToString();
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
        //----------------------------------------------------------------
        //Update User
        [HttpPut("{id}")]
        public ActionResult<int> PutUsers(int id, Users users)
        {
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
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@roleID", users.RoleId);
                cmd.Parameters.AddWithValue("@firstname", users.FirstName);
                cmd.Parameters.AddWithValue("@lastname", users.LastName);
                cmd.Parameters.AddWithValue("@address", users.Address);
                cmd.Parameters.AddWithValue("@phone", users.Phone);
                cmd.Parameters.AddWithValue("@password", users.Password);
                cmd.Parameters.AddWithValue("@email", users.Email);
                cmd.Parameters.AddWithValue("@sex", users.Sex);
                cmd.Parameters.AddWithValue("@age", users.Age);
                cmd.Parameters.AddWithValue("@avatar", users.Avatar);
                cmd.Parameters.AddWithValue("@stmttype", "updateUsersByID-admin");

                int result = Convert.ToInt32(cmd.ExecuteScalar());
                if (result == 1)
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
        //----------------------------------------------------------------
        //Create User
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
                cmd.Parameters.AddWithValue("@roleID", users.RoleId);
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
                if (result == -1)
                {
                    return BadRequest(result);
                }
                else
                {
                    return Ok(result);
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
        //----------------------------------------------------------------
        //DELETE User 
        // DELETE: api/admin/adusers/5

        [HttpDelete("{id}")]
        public ActionResult<int> DeleteUsers(int id)
        {
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
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stmttype", "deleteUsers");

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
