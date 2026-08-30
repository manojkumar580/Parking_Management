namespace Parking_Management.Server.DTOs.Subscription;

public class SubscriptionResponse
{
    public Guid Id { get; set; }

    public Guid ParkingSpaceId { get; set; }

    public string SpaceNumber { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}