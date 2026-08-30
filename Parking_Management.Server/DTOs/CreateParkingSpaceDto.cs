using Parking_Management.Server.Models;

namespace Parking_Management.Server.DTOs.ParkingSpaces;

public class CreateParkingSpaceDto
{
    public string SpaceNumber { get; set; } = string.Empty;

    public SpaceType SpaceType { get; set; }
}