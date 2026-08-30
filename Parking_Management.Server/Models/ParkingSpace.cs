namespace Parking_Management.Server.Models
{
    public class ParkingSpace
    {
        public Guid Id { get; set; }

        public string SpaceNumber { get; set; } = string.Empty;

        public SpaceType SpaceType { get; set; }

        public bool IsActive { get; set; }

        // Navigation properties
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}