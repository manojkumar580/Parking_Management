using Microsoft.EntityFrameworkCore;
using Parking_Management.Server.Data;
using Parking_Management.Server.DTOs.ParkingSpaces;
using Parking_Management.Server.Models;

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
            .AsNoTracking()
            .Select(x => new ParkingSpaceDto
            {
                Id = x.Id,
                SpaceNumber = x.SpaceNumber,
                SpaceType = x.SpaceType,
                IsActive = x.IsActive
            })
            .ToListAsync();
    }

    public async Task<ParkingSpaceDto?> GetByIdAsync(Guid id)
    {
        return await _context.ParkingSpaces
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ParkingSpaceDto
            {
                Id = x.Id,
                SpaceNumber = x.SpaceNumber,
                SpaceType = x.SpaceType,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ParkingSpaceDto> CreateAsync(
        CreateParkingSpaceDto dto)
    {
        var parkingSpace = new ParkingSpace
        {
            Id = Guid.NewGuid(),
            SpaceNumber = dto.SpaceNumber,
            SpaceType = dto.SpaceType,
            IsActive = true
        };

        _context.ParkingSpaces.Add(parkingSpace);
        await _context.SaveChangesAsync();

        return new ParkingSpaceDto
        {
            Id = parkingSpace.Id,
            SpaceNumber = parkingSpace.SpaceNumber,
            SpaceType = parkingSpace.SpaceType,
            IsActive = parkingSpace.IsActive
        };
    }

    public async Task<bool> DeactivateAsync(Guid id)
    {
        var parkingSpace = await _context.ParkingSpaces
            .FirstOrDefaultAsync(x => x.Id == id);

        if (parkingSpace == null)
        {
            return false;
        }

        parkingSpace.IsActive = false;

        await _context.SaveChangesAsync();

        return true;
    }
}