using Microsoft.EntityFrameworkCore;
using Parking_Management.Server.Data;
using Parking_Management.Server.DTOs.ParkingSpaces;

namespace Parking_Management.Server.Services;

public class ParkingSpaceService
{
    private readonly ParkingManagementDbContext _context;

    public ParkingSpaceService(ParkingManagementDbContext context)
    {
        _context = context;
    }

    public async Task<List<ParkingSpaceDto>> GetAllAsync()
    {
        return await _context.ParkingSpaces
            .Select(x => new ParkingSpaceDto
            {
                Id = x.Id,
                SpaceNumber = x.SpaceNumber,
                SpaceType = x.SpaceType,
                IsActive = x.IsActive
            })
            .ToListAsync();
    }
}