namespace Booking_System.Models
{
    public class BookingModel
    {
        public int Id { get; set; }
        public UserModel? User { get; set; }
        public int? UserModelId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public RoomModel? Room { get; set; }
        public int? RoomModelId { get; set; }
        public VehicleModel? Vehicle {  get; set; }
        public int? VehicleModelId { get; set; }
    }
}
