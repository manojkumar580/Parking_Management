using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Parking_Management.Server.Data;
using Parking_Management.Server.DTOs.Auth;
using Parking_Management.Server.Models;

namespace Parking_Management.Server.Services;

public class AuthService
{
    private readonly ParkingManagementDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly JwtService _jwtService;

    public AuthService(
        ParkingManagementDbContext context,
        IPasswordHasher<User> passwordHasher,
        JwtService jwtService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<(bool Success, string? Error, User? User)> RegisterAsync(
        RegisterRequest request)
    {
        try
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (existingUser != null)
            {
                return (false, "Email is already registered.", null);
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = email,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                request.Password);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return (true, null, user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while registering the user.",
                ex);
        }
    }

    public async Task<(bool Success, string? Error, string? Token, User? User)> LoginAsync(
        LoginRequest request)
    {
        try
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return (false, "Invalid email or password.", null, null);
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return (false, "Invalid email or password.", null, null);
            }

            var token = _jwtService.GenerateToken(user);

            return (true, null, token, user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while logging in the user.",
                ex);
        }
    }
}