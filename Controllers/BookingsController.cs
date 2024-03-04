using Booking_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Booking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {

        private readonly BookingContext _context;

        public BookingsController(BookingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GetAll of respective class
        /// </summary>
        /// <returns>All listings</returns>
        [HttpGet]
        public IEnumerable<BookingModel> GetAll()
        {
            var user = User.Identity;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claims = identity.Claims;
            }
            Console.WriteLine(identity);
            return _context.Bookings.Include(x => x.User).Include(x => x.Room).Include(x => x.Vehicle).ToList();
        }

        /// <summary>
        /// Get listing of respective class by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single listing by id</returns>
        [HttpGet("{id}")]
        public BookingModel? Get(int id)
        {
            try
            {
                return _context.Bookings.Include(x => x.User).Include(x => x.Room).Include(x => x.Vehicle).First();
            }
            catch (Exception)
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
        public void Post([FromBody] BookingModel value)
        {
            try
            {
                UserModel user = _context.Users.First(x => x.Id == value.UserModelId);
            }
            catch (Exception)
            {
                Response.StatusCode = 404;
                return;
            }



            bool isBooked = _context.Bookings
                .Where(x => x.RoomModelId == value.RoomModelId || x.VehicleModelId == value.VehicleModelId)
                .Where(x => x.StartTime <= value.EndTime)
                .Where(x => x.EndTime >= value.StartTime)
                .Any();
            if (isBooked)
            {
                Response.StatusCode = 409;
                return;
            }
            TimeSpan? duration = value.EndTime - value.StartTime;
            if (duration?.Days > 1)
            {
                Response.StatusCode = 409;
                return;
            }
            try
            {
                _context.Add(value);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 422;
                return;
            }
        }

        /// <summary>
        /// Edit listing in respective class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BookingModel value)
        {
            BookingModel? booking = _context.Bookings.FirstOrDefault(x => x.Id == id);

            bool isOverlapping = _context.Bookings
                .Where(x => (x.RoomModelId == value.RoomModelId || x.VehicleModelId == value.VehicleModelId) && x.Id != id)
                .Where(x => x.StartTime <= value.EndTime)
                .Where(x => x.EndTime >= value.StartTime)
                .Any();

            if (isOverlapping)
            {
                Response.StatusCode = 409;
                return;
            }

            TimeSpan? duration = value.EndTime - value.StartTime;
            if (duration?.Days > 1)
            {
                Response.StatusCode = 409;
                return;
            }
            try
            {
            booking.User = value.User ?? booking.User;
            booking.StartTime = value.StartTime ?? booking.StartTime;
            booking.EndTime = value.EndTime ?? booking.EndTime;
            }
            catch (Exception ex)
            {
                Response.StatusCode = 422;
                return;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 422;
                return;
            }
        }

        /// <summary>
        /// Delete listing from respective class
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            try
            {
            BookingModel booking = _context.Bookings.Where(x => x.Id == id).First();
            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 404;
                return;
            }
        }
    }
}
