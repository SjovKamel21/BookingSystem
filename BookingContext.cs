using Booking_System.Models;
using Microsoft.EntityFrameworkCore;
namespace Booking_System
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options) { }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<BookingModel> Bookings { get; set; }
        public DbSet<VehicleModel>  Vehicles { get; set; }
        public DbSet<RoomModel> Rooms { get; set; }
    }
}
