using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Migrations;
using HotelService.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelService.Db.Model;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HotelService.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly DbRepository _dbRepository;
    private readonly CustomerService _customerService;

    public AuthController(AuthService authService, DbRepository dbRepository, CustomerService customerService)
    {
        _authService = authService;
        _dbRepository = dbRepository;
        _customerService = customerService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        try
        {
            var existingUser = await _dbRepository.GetUsersDataByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest($"User with email '{request.Email}' already registered.");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword( request.Password);
            string role = string.IsNullOrEmpty(request.Role) ? "User" : request.Role;
            var newUser = 
                await _dbRepository.AddNewUserDataAsync
                (request.Username, hashedPassword,
                    request.Email, role);

            return Ok(newUser.UserId);
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
            var user = await _dbRepository.GetUsersDataAsync(request.Username, request.Email);
            var customer = await _customerService.GetUserData(user.UserId);
            var loyaltyP = customer.LoyaltyPoints;
            Console.WriteLine($"loyalty points: {loyaltyP}");
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username/email or password.");
            }
            var token =  _authService.GenerateJwtToken(user);
            var customerImage = await _dbRepository.FindCustomerAndReturnImage(user);
            return Ok(new
            {
                Token = token,
                UserDto = new
                {
                    Id = user.UserId,
                    UserName = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    RegistrationDate = user.RegistrationDate,
                    ImageUrl = customerImage,
                    LoyaltyPoints = loyaltyP
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var allUsers = await _dbRepository.GetAllUsers();
            return Ok(allUsers);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
}