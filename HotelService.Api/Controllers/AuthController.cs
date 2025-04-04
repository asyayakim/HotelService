using HotelService.Db.DTOs;
using HotelService.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        try
        {
            var existingUser = await _authService.GetUsersDataAsync(request.Username);
            if (existingUser != null)
            {
                return BadRequest($"User with username '{request.Username}' already exists.");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword( request.Password);
            string role = string.IsNullOrEmpty(request.Role) ? "User" : request.Role;
            var newUser =
                await _authService.AddUserDataAsync
                (request.Username, hashedPassword,
                    request.Email, role);

            return Ok(new
            {
                Message = "User registered successfully.",
                User = new
                {
                    Id = newUser.Id,
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    Role = newUser.Role
                }
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        try
        {
            var user = await _authService.GetUsersDataAsync(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                Console.WriteLine("Login failed: Invalid username or password.");
                return Unauthorized("Invalid username or password.");
            }
            var token = _authService.GenerateJwtToken(user);
            return Ok(new
            {
                Token = token,
                User = new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    } 
}