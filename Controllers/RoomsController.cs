using Booking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Booking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomsController : ControllerBase
    {

        private readonly BookingContext _context;

        public RoomsController(BookingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GetAll of respective class
        /// </summary>
        /// <returns>All listings</returns>
        [HttpGet]
        public IEnumerable<RoomModel> GetAll()
        {
            return _context.Rooms.ToList();
        }

        /// <summary>
        /// Get listing of respective class by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single listing by id</returns>
        [HttpGet("{id}")]
        public RoomModel? Get(int id)
        {
            try
            {
                return _context.Rooms.Where(x => x.Id == id).First();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 404;
            }
            return null;
        }

        /// <summary>
        /// Adds listing to respective class
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] RoomModel value)
        {
            if (value.Name == null)
            {
                Response.StatusCode = 422;
                return;
            }
            _context.Add(value);
            _context.SaveChanges();
        }

        /// <summary>
        /// Edit listing in respective class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] RoomModel value)
        {
            RoomModel room = _context.Rooms.Where(x=> x.Id == id).First();

            room.Name = value.Name ?? room.Name;
            if (value.Seats > 0) { room.Seats = value.Seats; }
            if (value.Size > 0) { room.Size = value.Size; }
            room.TvScreen = value.TvScreen ?? room.TvScreen;
            room.WhiteBoard = value.WhiteBoard ?? room.WhiteBoard;
            _context.SaveChanges();
        }

        /// <summary>
        /// Delete listing from respective class
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            RoomModel room = _context.Rooms.Where(x => x.Id == id).First();
            _context.Rooms.Remove(room);
            _context.SaveChanges();
        }
    }
}
