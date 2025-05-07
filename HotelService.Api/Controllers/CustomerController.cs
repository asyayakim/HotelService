using System.Security.Claims;
using HotelService.Db.DTOs;
using HotelService.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ReservationService _reservationService;
    private readonly CustomerService _customerService;

    public CustomerController(ReservationService reservationService, CustomerService customerService)
    {
        _reservationService = reservationService;
        _customerService = customerService;
    }

    [HttpPost]
     public async Task<IActionResult> AddCustomerAsync([FromBody] CustomerDto customerDto)
     {
         try
         {
             var newCustomer = await _customerService.AddNewCustomerAsync(customerDto);
             return Ok(newCustomer);
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw new Exception(e.Message);
         }
     }
    [Authorize]
    [HttpPatch("changeData")]
    public async Task<ActionResult<CustomerDto>> ChangeData([FromBody] CustomerDto customerDto)
    {
        try
        {
            var userIdData = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdData)) return Unauthorized();
            var updatedCustomer = await _customerService.ChangeData(customerDto);
            return Ok(updatedCustomer);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    [Authorize]
    [HttpGet("user")]
    public async Task<ActionResult<CustomerDto>> GetUserDataById()
    {
        try
        {
            var userIdData = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdData)) return Unauthorized();
            var userId = int.Parse(userIdData);
            var customer = await _customerService.GetUserData(userId);
            return Ok(customer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPatch("uploadImage")]
    public async Task<IActionResult> AddReservationAsync([FromForm] CustomerImageDto dto)
    {
        try
        {
            if (dto.Image == null)
                return BadRequest("Image file is missing.");
            var publicId = $"user_{dto.UserId}_avatar";
            
            var userIdData = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdData)) return Unauthorized();
            var imageUrl = await _customerService.UploadImageAsync(dto.Image, publicId, dto.UserId);
            return Ok(new
            {
                imageUrl
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        } 
    }
    
}