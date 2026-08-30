namespace Parking_Management.Server.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ParkingSpaceId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Amount { get; set; }

        public SubscriptionStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;

        public ParkingSpace ParkingSpace { get; set; } = null!;
    }
}