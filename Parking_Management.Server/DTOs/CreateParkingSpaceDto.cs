using Parking_Management.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace Parking_Management.Server.DTOs.ParkingSpaces;

public class CreateParkingSpaceDto
{
    [Required]
    [MaxLength(50)]
    public string SpaceNumber { get; set; } = string.Empty;

    [Required]
    public SpaceType SpaceType { get; set; }
}