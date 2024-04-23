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
using Microsoft.Extensions.Logging;

namespace Festival.Controllers.AdminController
{
    [Authorize(Roles = "1,2")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AdminCategoriesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        // GET: api/Categories
        [Authorize]
        [HttpGet]
        public List<CategoriesDetail> GetCategories()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<CategoriesDetail> _listCategories = new List<CategoriesDetail>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_categories", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stmttype", "getCategories-admin");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    CategoriesDetail categories = new CategoriesDetail();
                    categories.Category_Id = Convert.ToInt32(_Reader["Category_ID"]);
                    categories.CategoryName = _Reader["CategoryName"].ToString();
                    categories.CategoryDescription = _Reader["CategoryDescription"].ToString();
                    categories.Image = _Reader["Image"].ToString();
                    categories.TotalEvents = Convert.ToInt32(_Reader["TotalEvents"]);
                    categories.TotalNews = Convert.ToInt32(_Reader["TotalNews"]);
                    categories.TotalSubscribers = Convert.ToInt32(_Reader["TotalSubscribers"]);
                    _listCategories.Add(categories);
                }

                return _listCategories;
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

        // GET: api/Categories/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriesDetail>> GetCategories(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();


            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                CategoriesDetail categories = new CategoriesDetail();
                SqlCommand cmd = new SqlCommand("sp_categories", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stmttype", "getCategoriesByID-admin");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    categories.Category_Id = Convert.ToInt32(dt.Rows[0]["Category_ID"]);
                    categories.CategoryName = dt.Rows[0]["CategoryName"].ToString();
                    categories.CategoryDescription = dt.Rows[0]["CategoryDescription"].ToString();
                    categories.Image = dt.Rows[0]["Image"].ToString();
                    categories.TotalEvents = Convert.ToInt32(dt.Rows[0]["TotalEvents"]);
                    categories.TotalNews = Convert.ToInt32(dt.Rows[0]["TotalNews"]);
                    categories.TotalSubscribers = Convert.ToInt32(dt.Rows[0]["TotalSubscribers"]);
                }
                else
                {
                    return NotFound();
                }
                return categories;
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

        //--------------------------------------------------------------------------
        //UPDATE
        [Authorize(Roles = "1,2")]
        [HttpPut("{id}")]
        public ActionResult<int> PutCategories(int id, Categories categories)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                if (CategoriesExists(id) == 0)
                {
                    return NotFound();
                }
                SqlCommand cmd = new SqlCommand("sp_categories", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", categories.CategoryName);
                cmd.Parameters.AddWithValue("@description", categories.CategoryDescription);
                cmd.Parameters.AddWithValue("@stmttype", "updateCategoriesByID");

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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public ActionResult<int> PostCategories(Categories categories)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_categories", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", categories.Category_Id);
                cmd.Parameters.AddWithValue("@name", categories.CategoryName);
                cmd.Parameters.AddWithValue("@description", categories.CategoryDescription);
                cmd.Parameters.AddWithValue("@image", categories.Image);
                cmd.Parameters.AddWithValue("@stmttype", "createCategories");

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

        // DELETE: api/Categories/5
        [Authorize(Roles = "1,2")]
        [HttpDelete("{id}")]
        public ActionResult<int> DeleteCategories(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                if (CategoriesExists(id) == 0)
                {
                    return NotFound();
                }
                SqlCommand cmd = new SqlCommand("sp_categories", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stmttype", "deleteCategories");

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




        private int CategoriesExists(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());


            if (conn.State != ConnectionState.Open) conn.Open();

            SqlCommand cmd = new SqlCommand("sp_categories", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@stmttype", "isExist");
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }



    }
}
