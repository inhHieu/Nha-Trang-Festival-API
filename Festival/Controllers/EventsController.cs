using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Festival.Data;
using Festival.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Data.Common;

namespace Festival.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EventsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Events
        [HttpGet]
        public List<Events> GetEvents(int offset, int limit)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<Events> _listEvents = new List<Events>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_events", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@stmttype", "getEvents");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    Events events = new Events();
                    events.EventId = Convert.ToInt32(_Reader["Event_ID"]);
                    events.CategoryId = Convert.ToInt32(_Reader["Category_ID"]);
                    events.EventName = _Reader["EventName"].ToString();
                    events.DateStart = Convert.ToDateTime(_Reader["DateStart"]);
                    events.TakePlace = _Reader["TakePlace"].ToString();
                    events.EventDescription = _Reader["EventDescription"].ToString();
                    events.Summary = _Reader["Summary"].ToString();
                    events.ImageUrl = _Reader["ImageUrl"].ToString();
                    _listEvents.Add(events);
                }

                return _listEvents;
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

        //======================================================================================
        //GET EVENT BASE CATEGORIES
        [HttpGet("category/{categoryID}")]
        public List<Events> GetEventsOnCategory(int offset, int limit, int categoryID)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<Events> _listEvents = new List<Events>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_events", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@categoryID", categoryID);
                cmd.Parameters.AddWithValue("@stmttype", "getEventsOnCategory");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    Events events = new Events();
                    events.EventId = Convert.ToInt32(_Reader["Event_ID"]);
                    events.CategoryId = Convert.ToInt32(_Reader["Category_ID"]);
                    events.EventName = _Reader["EventName"].ToString();
                    events.DateStart = Convert.ToDateTime(_Reader["DateStart"]).Date;
                    events.TakePlace = _Reader["TakePlace"].ToString();
                    events.EventDescription = _Reader["EventDescription"].ToString();
                    events.Summary = _Reader["Summary"].ToString();
                    events.ImageUrl = _Reader["ImageUrl"].ToString();
                    _listEvents.Add(events);
                }

                return _listEvents;
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



        // GET: api/Events/5
        [HttpGet("{id}")]
        /*public async Task<ActionResult<Events>> GetEvents(int id)
        {
            var events = await _context.Events.FindAsync(id);

            if (events == null)
            {
                return NotFound();
            }

            return events;
        }*/
        public async Task<ActionResult<Events>> GetEvents(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();


            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                Events events = new Events();
                SqlCommand cmd = new SqlCommand("sp_events", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stmttype", "getEventsByID");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    events.EventId = Convert.ToInt32(dt.Rows[0]["Event_ID"]);
                    events.CategoryId = Convert.ToInt32(dt.Rows[0]["Category_ID"]);
                    events.EventName = dt.Rows[0]["EventName"].ToString();
                    events.DateStart = Convert.ToDateTime(dt.Rows[0]["DateStart"]);
                    events.TakePlace = dt.Rows[0]["TakePlace"].ToString();
                    events.EventDescription = dt.Rows[0]["EventDescription"].ToString();
                    events.Summary = dt.Rows[0]["Summary"].ToString();
                    events.ImageUrl = dt.Rows[0]["ImageUrl"].ToString();

                }
                else
                {
                    return NotFound();
                }
                return events;
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



        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        

        private int EventsExists(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());


            if (conn.State != ConnectionState.Open) conn.Open();

            SqlCommand cmd = new SqlCommand("sp_events", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@stmttype", "isExist");
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;



            //return _context.Events.Any(e => e.Event_ID == id);
        }
    }
}
