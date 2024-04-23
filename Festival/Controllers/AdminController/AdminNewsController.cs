using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Festival.Data;
using Festival.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Festival.Controllers.AdminController
{
    [Authorize(Roles = "1,2")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminNewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public AdminNewsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }




        //-----------------------------------------------------------------------------------------
        // PUT: api/News/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult<int> PutNews(int id, NewsPut news)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                if (NewsExists(id) == 0)
                {
                    return NotFound();
                }
                SqlCommand cmd = new SqlCommand("sp_news", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@category_ID", news.CategoryId);
                cmd.Parameters.AddWithValue("@title", news.NewsTitle);
                cmd.Parameters.AddWithValue("@content", news.NewsContent);
                cmd.Parameters.AddWithValue("@postedDate", news.PostedDate);
                cmd.Parameters.AddWithValue("@view", news.Views);
                cmd.Parameters.AddWithValue("@summary", news.Summary);
                cmd.Parameters.AddWithValue("@titleimg", news.TitleImg);
                cmd.Parameters.AddWithValue("@stmttype", "updateNewsByID");

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
        // POST: api/News
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<int> PostNews(NewsPut news)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_news", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@category_ID", news.CategoryId);
                cmd.Parameters.AddWithValue("@title", news.NewsTitle);
                cmd.Parameters.AddWithValue("@content", news.NewsContent);
                cmd.Parameters.AddWithValue("@postedDate", news.PostedDate);
                cmd.Parameters.AddWithValue("@view", news.Views);
                cmd.Parameters.AddWithValue("@summary", news.Summary);
                cmd.Parameters.AddWithValue("@titleimg", news.TitleImg);
                cmd.Parameters.AddWithValue("@stmttype", "createNews");

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
        [HttpDelete("{id}")]
        public ActionResult<int> DeleteNews(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                if (NewsExists(id) == 0)
                {
                    return NotFound();
                }
                SqlCommand cmd = new SqlCommand("sp_news", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stmttype", "deleteNews");

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





        private int NewsExists(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());


            if (conn.State != ConnectionState.Open) conn.Open();

            SqlCommand cmd = new SqlCommand("sp_news", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@stmttype", "isExist");
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }
    }
}
