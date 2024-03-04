using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Booking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Booking_System.Utils;

namespace Booking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly BookingContext _context;

        public UsersController(BookingContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GetAll of respective class
        /// </summary>
        /// <returns>All listings</returns>
        [HttpGet]
        public IEnumerable<UserModel> GetAll()
        {
            return _context.Users.ToList();
        }

        /// <summary>
        /// Get listing of respective class by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single listing by id</returns>
        [HttpGet("{id}")]
        public UserModel? Get(int id)
        {
            try
            {
                return _context.Users.Where(x => x.Id == id).First();
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
        public void Post(int id,[FromBody] UserModel value)
        {
            bool exists = _context.Users.Any(x => x.Name == value.Name || x.Email == value.Email);

            if (value.Name == null || value.Email == null || value.Password == null)
            {
                Response.StatusCode = 422;
                return;
            }
            if (!exists)
            {
            value.Password = Hashish.ComputeSha256Hash(value.Password);
            _context.Add(value);
            _context.SaveChanges();
            }
            else
            {
                Response.StatusCode = 403;
                return;
            }
        }

        /// <summary>
        /// Edit listing in respective class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UserModel value)
        {
            UserModel user = _context.Users.Where(x => x.Id == id).First();
            bool exists = _context.Users.Any(x => x.Name == value.Name || x.Email == value.Email);

            if (!exists)
            {
                user.Name = value.Name ?? user.Name;
                user.Email = value.Email ?? user.Email;
                value.Password = value.Password != null ? Hashish.ComputeSha256Hash(value.Password) : user.Password;
                user.Password = value.Password ?? user.Password;
                _context.SaveChanges();
            }
            else
            {
                Response.StatusCode = 403;
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
            UserModel user = _context.Users.Where(x => x.Id == id).First();
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
