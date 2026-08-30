using Microsoft.AspNetCore.Mvc;
using Parking_Management.Server.Services;

namespace Parking_Management.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingSpacesController : ControllerBase
    {
        private readonly ParkingSpaceService _parkingSpaceService;
        public ParkingSpacesController(ParkingSpaceService parkingSpaceService)
        {
            _parkingSpaceService = parkingSpaceService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllParkingSpaces()
        {
            var parkingSpaces = await _parkingSpaceService.GetAllAsync();
            return Ok(parkingSpaces);
        }
    }
}
