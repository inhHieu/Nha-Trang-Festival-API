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
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public NewsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews(int offset, int limit)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<News> _listNews = new List<News>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_news", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@stmttype", "getNews");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    News news = new News();
                    news.NewsId = Convert.ToInt32(_Reader["News_ID"]);
                    news.Category = new Categories();
                    news.Category.CategoryName = _Reader["CategoryName"].ToString();
                    news.CategoryId = Convert.ToInt32(_Reader["Category_ID"]);
                    news.NewsTitle = _Reader["NewsTitle"].ToString();
                    news.NewsContent = _Reader["NewsContent"].ToString();
                    news.PostedDate = Convert.ToDateTime(_Reader["PostedDate"]);
                    news.Views = Convert.ToInt32(_Reader["Views"]);
                    news.Summary = _Reader["Summary"].ToString();
                    news.TitleImg = _Reader["TitleImg"].ToString();
                    _listNews.Add(news);
                }

                return _listNews;
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

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();


            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                News news = new News();
                SqlCommand cmd = new SqlCommand("sp_news", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stmttype", "getNewsByID");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    news.NewsId = Convert.ToInt32(dt.Rows[0]["News_ID"]);
                    news.Category = new Categories();
                    news.Category.CategoryName = dt.Rows[0]["CategoryName"].ToString();
                    news.CategoryId = Convert.ToInt32(dt.Rows[0]["Category_ID"]);
                    news.NewsTitle = dt.Rows[0]["NewsTitle"].ToString();
                    news.NewsContent = dt.Rows[0]["NewsContent"].ToString();
                    news.PostedDate = Convert.ToDateTime(dt.Rows[0]["PostedDate"]);
                    news.Views = Convert.ToInt32(dt.Rows[0]["Views"]);
                    news.TitleImg = dt.Rows[0]["TitleImg"].ToString();
                    news.Summary = dt.Rows[0]["Summary"].ToString();
                }
                else
                {
                    return NotFound();
                }
                return news;
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

        //Newest sort
        [HttpGet, Route("lastest")]
        public async Task<ActionResult<IEnumerable<News>>> Lastest(int offset, int limit)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<News> _listNews = new List<News>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_news", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@stmttype", "getLastestNews");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    News news = new News();
                    news.NewsId = Convert.ToInt32(_Reader["News_ID"]);
                    news.Category = new Categories();
                    news.Category.CategoryName = _Reader["CategoryName"].ToString();
                    news.CategoryId = Convert.ToInt32(_Reader["Category_ID"]);
                    news.NewsTitle = _Reader["NewsTitle"].ToString();
                    news.NewsContent = _Reader["NewsContent"].ToString();
                    news.PostedDate = Convert.ToDateTime(_Reader["PostedDate"]);
                    news.Views = Convert.ToInt32(_Reader["Views"]);
                    news.Summary = _Reader["Summary"].ToString();
                    news.TitleImg = _Reader["TitleImg"].ToString();
                    _listNews.Add(news);
                }

                return _listNews;
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
        
        
        [HttpGet, Route("lastest/category/{categoryID}")]
        public async Task<ActionResult<IEnumerable<News>>> Lastest(int offset, int limit, int categoryID)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<News> _listNews = new List<News>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_news", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@id", categoryID);
                cmd.Parameters.AddWithValue("@stmttype", "getLastestNewsByCategory");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    News news = new News();
                    news.NewsId = Convert.ToInt32(_Reader["News_ID"]);
                    news.Category = new Categories();
                    news.Category.CategoryName = _Reader["CategoryName"].ToString();
                    news.CategoryId = Convert.ToInt32(_Reader["Category_ID"]);
                    news.NewsTitle = _Reader["NewsTitle"].ToString();
                    news.NewsContent = _Reader["NewsContent"].ToString();
                    news.PostedDate = Convert.ToDateTime(_Reader["PostedDate"]);
                    news.Views = Convert.ToInt32(_Reader["Views"]);
                    news.Summary = _Reader["Summary"].ToString();
                    news.TitleImg = _Reader["TitleImg"].ToString();
                    _listNews.Add(news);
                }

                return _listNews;
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

        //Trending sort
        [HttpGet, Route("trending")]
        public async Task<ActionResult<IEnumerable<News>>> Trending(int offset, int limit)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<News> _listNews = new List<News>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_news", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@stmttype", "getTrendingNews");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    News news = new News();
                    news.NewsId = Convert.ToInt32(_Reader["News_ID"]);
                    news.Category = new Categories();
                    news.Category.CategoryName = _Reader["CategoryName"].ToString();
                    news.CategoryId = Convert.ToInt32(_Reader["Category_ID"]);
                    news.NewsTitle = _Reader["NewsTitle"].ToString();
                    news.NewsContent = _Reader["NewsContent"].ToString();
                    news.PostedDate = Convert.ToDateTime(_Reader["PostedDate"]);
                    news.Views = Convert.ToInt32(_Reader["Views"]);
                    news.Summary = _Reader["Summary"].ToString();
                    news.TitleImg = _Reader["TitleImg"].ToString();
                    _listNews.Add(news);
                }

                return _listNews;
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
        [HttpGet, Route("trending/category/{categoryID}")]
        public async Task<ActionResult<IEnumerable<News>>> Trending(int offset, int limit, int categoryID)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<News> _listNews = new List<News>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_news", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@id", categoryID);
                cmd.Parameters.AddWithValue("@stmttype", "getTrendingNewsByCategory");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    News news = new News();
                    news.NewsId = Convert.ToInt32(_Reader["News_ID"]);
                    news.Category = new Categories();
                    news.Category.CategoryName = _Reader["CategoryName"].ToString();
                    news.CategoryId = Convert.ToInt32(_Reader["Category_ID"]);
                    news.NewsTitle = _Reader["NewsTitle"].ToString();
                    news.NewsContent = _Reader["NewsContent"].ToString();
                    news.PostedDate = Convert.ToDateTime(_Reader["PostedDate"]);
                    news.Views = Convert.ToInt32(_Reader["Views"]);
                    news.Summary = _Reader["Summary"].ToString();
                    news.TitleImg = _Reader["TitleImg"].ToString();
                    _listNews.Add(news);
                }

                return _listNews;
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
