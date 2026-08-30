namespace Parking_Management.Server.DTOs.Booking;

public class BookingResponse
{
    public Guid Id { get; set; }

    public Guid ParkingSpaceId { get; set; }

    public string SpaceNumber { get; set; } = string.Empty;

    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public decimal? Amount { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}