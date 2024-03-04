using Booking_System.Models;
using Booking_System.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly BookingContext _context;

        public LoginController(BookingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Used for user login
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public string? Post([FromBody] UserLoginModel value)
        {
            UserModel? user = _context.Users.Where(u => u.Email == value.Email).FirstOrDefault();
            if (user == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            string hashedPassword = Hashish.ComputeSha256Hash(value.Password);
            if (hashedPassword == user.Password)
            {

                return JWT.GenerateJSONWebToken(user);
            }
            Response.StatusCode = 401;
            return null;
        }
    }
}
