using Microsoft.AspNetCore.Mvc;
using Parking_Management.Server.DTOs.Auth;
using Parking_Management.Server.Services;

namespace Parking_Management.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.Success)
            {
                return Conflict(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Registration successful.",
                userId = result.User!.Id,
                name = result.User.Name,
                email = result.User.Email
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred."
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);

            if (!result.Success)
            {
                return Unauthorized(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Login successful.",
                token = result.Token,
                userId = result.User!.Id,
                name = result.User.Name,
                email = result.User.Email
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred."
            });
        }
    }
}