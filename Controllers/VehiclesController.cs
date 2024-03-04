using Booking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Booking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiclesController : ControllerBase
    {

        private readonly BookingContext _context;

        public VehiclesController(BookingContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GetAll of respective class
        /// </summary>
        /// <returns>All listings</returns>
        [HttpGet]
        public IEnumerable<VehicleModel> GetAll()
        {
            return _context.Vehicles.ToList();
        }

        /// <summary>
        /// Get listing of respective class by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single listing by id</returns>
        [HttpGet("{id}")]
        public VehicleModel? Get(int id)
        {
            try
            {
                return _context.Vehicles.Where(x => x.Id == id).First();
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
        public void Post([FromBody] VehicleModel value)
        {
            if (value.Name == null || value.LicensePlate == null || value.GasType == null)
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
        public void Put(int id, [FromBody] VehicleModel value)
        {
            VehicleModel vehicle = _context.Vehicles.Where(x=> x.Id == id).First();

            vehicle.Name = value.Name ?? vehicle.Name;
            if (value.Seats > 0) { vehicle.Seats = value.Seats; }
            vehicle.LicensePlate = value.LicensePlate ?? vehicle.LicensePlate;
            vehicle.GasType = value.GasType ?? vehicle.GasType;
            _context.SaveChanges();
        }

        /// <summary>
        /// Delete listing from respective class
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            VehicleModel vehicle = _context.Vehicles.Where(x => x.Id == id).First();
            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }
    }
}
