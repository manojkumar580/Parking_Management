using Microsoft.AspNetCore.Mvc;
using Parking_Management.Server.DTOs.ParkingSpaces;
using Parking_Management.Server.Services;

namespace Parking_Management.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingSpacesController : ControllerBase
{
    private readonly ParkingSpaceService _service;

    public ParkingSpacesController(ParkingSpaceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var parkingSpaces = await _service.GetAllAsync();

        return Ok(parkingSpaces);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var parkingSpace = await _service.GetByIdAsync(id);

        if (parkingSpace == null)
        {
            return NotFound();
        }

        return Ok(parkingSpace);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateParkingSpaceDto dto)
    {
        var parkingSpace = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = parkingSpace.Id },
            parkingSpace);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var success = await _service.DeactivateAsync(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}