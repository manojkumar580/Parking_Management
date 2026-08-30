using Parking_Management.Server.Models;

namespace Parking_Management.Server.DTOs.ParkingSpaces;

public class ParkingSpaceDto
{
    public Guid Id { get; set; }

    public string SpaceNumber { get; set; } = string.Empty;

    public SpaceType SpaceType { get; set; }

    public bool IsActive { get; set; }
}
