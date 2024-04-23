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
    public class AdminEventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public AdminEventsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }




        //-------------------------------------------------------------------------------------------------------
        // GET
        [HttpGet]
        public List<EventsDetail> GetEvents(int offset, int limit)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<EventsDetail> _listEventsDetail = new List<EventsDetail>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_events", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@stmttype", "getEventsAdmin");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    EventsDetail eventsDetail = new EventsDetail();
                    eventsDetail.EventId = Convert.ToInt32(_Reader["Event_ID"]);
                    eventsDetail.CategoryId = Convert.ToInt32(_Reader["Category_ID"]);
                    eventsDetail.DateStart = Convert.ToDateTime(_Reader["DateStart"]);
                    eventsDetail.EventDescription = _Reader["EventDescription"].ToString();
                    eventsDetail.EventName= _Reader["EventName"].ToString();
                    eventsDetail.TakePlace= _Reader["TakePlace"].ToString();
                    eventsDetail.Summary= _Reader["Summary"].ToString();
                    eventsDetail.ImageUrl = _Reader["ImageUrl"].ToString();
                    eventsDetail.TotalSub= Convert.ToInt32( _Reader["TotalSub"]);
                    _listEventsDetail.Add(eventsDetail);
                }

                return _listEventsDetail;
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
        }//-------------------------------------------------------------------------------------------------------
        // GET
        [HttpGet,Route("category/{categoryID}")]
        public List<EventsDetail> GetEventsByCategory(int offset, int limit, int categoryID)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                List<EventsDetail> _listEventsDetail = new List<EventsDetail>();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_events", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@categoryId", categoryID);
                cmd.Parameters.AddWithValue("@stmttype", "getEventsOnCategoryAdmin");
                SqlDataReader _Reader = cmd.ExecuteReader();

                while (_Reader.Read())
                {
                    EventsDetail eventsDetail = new EventsDetail();
                    eventsDetail.EventId = Convert.ToInt32(_Reader["Event_ID"]);
                    eventsDetail.CategoryId = Convert.ToInt32(_Reader["Category_ID"]);
                    eventsDetail.DateStart = Convert.ToDateTime(_Reader["DateStart"]);
                    eventsDetail.EventDescription = _Reader["EventDescription"].ToString();
                    eventsDetail.EventName= _Reader["EventName"].ToString();
                    eventsDetail.TakePlace= _Reader["TakePlace"].ToString();
                    eventsDetail.Summary= _Reader["Summary"].ToString();
                    eventsDetail.ImageUrl = _Reader["ImageUrl"].ToString();
                    eventsDetail.TotalSub= Convert.ToInt32( _Reader["TotalSub"]);
                    _listEventsDetail.Add(eventsDetail);
                }

                return _listEventsDetail;
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




        //-------------------------------------------------------------------------------------------------------
        // UPDATE: api/admin/adEvents/3
        [HttpPut("{id}")]
        public ActionResult<int> PutEvents(int id, EventsPut events)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                if (EventsExists(id) == 0)
                {
                    return NotFound();
                }
                SqlCommand cmd = new SqlCommand("sp_events", conn);
                cmd.Parameters.AddWithValue("@categoryId", events.CategoryId);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@eventname", events.EventName);
                cmd.Parameters.AddWithValue("@datestart", events.DateStart);
                cmd.Parameters.AddWithValue("@takeplace", events.TakePlace);
                cmd.Parameters.AddWithValue("@description", events.EventDescription);
                cmd.Parameters.AddWithValue("@summary", events.Summary);
                cmd.Parameters.AddWithValue("@imageurl", events.ImageUrl);
                cmd.Parameters.AddWithValue("@stmttype", "updateEventsByID");

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

        //-------------------------------------------------------------------------------------------------------
        // POST:  api/admin/adEvents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
         [HttpPost]
        public ActionResult<int> PostEvents(EventsPut events)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_events", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", events.EventId);
                cmd.Parameters.AddWithValue("@categoryId", events.CategoryId);
                cmd.Parameters.AddWithValue("@eventname", events.EventName);
                cmd.Parameters.AddWithValue("@datestart", events.DateStart);
                cmd.Parameters.AddWithValue("@takeplace", events.TakePlace);
                cmd.Parameters.AddWithValue("@description", events.EventDescription);
                cmd.Parameters.AddWithValue("@summary", events.Summary);
                cmd.Parameters.AddWithValue("@imageurl", events.ImageUrl);
                cmd.Parameters.AddWithValue("@stmttype", "createEvents");

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

        //-------------------------------------------------------------------------------------------------------
        // DELETE:  api/admin/adEvents/3
        [HttpDelete("{id}")]
        public ActionResult<int> DeleteEvents(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            conn.Open();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                if (EventsExists(id) == 0)
                {
                    return NotFound();
                }
                SqlCommand cmd = new SqlCommand("sp_events", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stmttype", "deleteEvents");

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
