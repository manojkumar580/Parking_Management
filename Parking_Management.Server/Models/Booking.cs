namespace Parking_Management.Server.Models
{
    public class Booking
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ParkingSpaceId { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public decimal? Amount { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;

        public ParkingSpace ParkingSpace { get; set; } = null!;
    }
}