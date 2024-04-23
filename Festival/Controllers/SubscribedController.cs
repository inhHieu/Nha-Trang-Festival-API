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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public SubscribedController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        //get user subscribed
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Subscribed>>> GetSub(int id, [FromQuery] int offset, [FromQuery] int limit)
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
                if (conn.State != ConnectionState.Open) conn.Open();

                List<Subscribed> subs = new List<Subscribed>();
                SqlCommand cmd = new SqlCommand("sp_users", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@stmttype", "getUserSubscribed");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    Subscribed sub = new Subscribed();
                    sub.SubscribedId = Convert.ToInt32(row["Subscribed_ID"]);
                    sub.UserId = Convert.ToInt32(row["User_ID"]);
                    sub.Event = new Events
                    {
                        EventId = (int)row["Event_ID"],
                        EventName = row["EventName"].ToString(),
                        EventDescription = row["EventDescription"].ToString(),
                        TakePlace = row["TakePlace"].ToString(),
                        DateStart = Convert.ToDateTime(row["DateStart"]),
                        ImageUrl = row["imageUrl"].ToString(),
                        Category = new Categories
                        {
                            CategoryName = row["CategoryName"].ToString()
                        }
                    };
                    subs.Add(sub);
                }

                return subs;
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


        [HttpGet, Route("subid/{userID}")]
        public ActionResult<List<Subscribed>> GetSubID(int userID)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            int jwtUserId = Convert.ToInt32(userIdClaim.Value);

            if (jwtUserId != userID)
            {
                return Unauthorized();
            }
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<Subscribed> _listSub = new List<Subscribed>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_users", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", userID);
                cmd.Parameters.AddWithValue("@stmttype", "getUserSubscribedID");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    Subscribed sub = new Subscribed();
                    sub.SubscribedId = Convert.ToInt32(_Reader["Subscribed_ID"]);
                    sub.UserId = Convert.ToInt32(_Reader["User_ID"]);
                    sub.EventId = Convert.ToInt32(_Reader["Event_ID"]);
                    _listSub.Add(sub);
                }

                return _listSub;
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

        //Subscribe
        [HttpPost]
        public ActionResult<int> Subscribe(int userID, int eventID)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            int jwtUserId = Convert.ToInt32(userIdClaim.Value);

            if (jwtUserId != userID)
            {
                return Unauthorized();
            }
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_users", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", userID);
                cmd.Parameters.AddWithValue("@eventid", eventID);
                cmd.Parameters.AddWithValue("@stmttype", "Subscribe");

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

        //-----------------------------------------------------------------------------------------
        // DELETE: api/News/5
        [HttpDelete]
        public ActionResult<int> UnSubscribe(int userID, int eventID)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            int jwtUserId = Convert.ToInt32(userIdClaim.Value);

            if (jwtUserId != userID)
            {
                return Unauthorized();
            }
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                 
                SqlCommand cmd = new SqlCommand("sp_users", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", userID);
                cmd.Parameters.AddWithValue("@eventid", eventID);
                cmd.Parameters.AddWithValue("@stmttype", "unSubscribe");

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

    }
}
