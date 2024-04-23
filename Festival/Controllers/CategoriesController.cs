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

namespace Festival.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CategoriesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Categories
        [HttpGet]
        public List<Categories> GetCategories()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<Categories> _listCategories = new List<Categories>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_categories", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stmttype", "getCategories");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    Categories categories = new Categories();
                    categories.Category_Id = Convert.ToInt32(_Reader["Category_ID"]);
                    categories.CategoryName = _Reader["CategoryName"].ToString();
                    categories.CategoryDescription = _Reader["CategoryDescription"].ToString();
                    categories.Image = _Reader["Image"].ToString();
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Categories>> GetCategories(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();


            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                Categories categories = new Categories();
                SqlCommand cmd = new SqlCommand("sp_categories", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stmttype", "getCategoriesByID");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    categories.Category_Id = Convert.ToInt32(dt.Rows[0]["Category_ID"]);
                    categories.CategoryName = dt.Rows[0]["CategoryName"].ToString();
                    categories.CategoryDescription = dt.Rows[0]["CategoryDescription"].ToString();
                    categories.Image = dt.Rows[0]["Image"].ToString();
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

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        

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
