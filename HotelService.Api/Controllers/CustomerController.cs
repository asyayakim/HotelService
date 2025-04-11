using HotelService.Db.DTOs;
using HotelService.Logic;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ReservationService _reservationService;

    public CustomerController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost("customer/login")]
     public async Task<ActionResult<CustomerDto>> Login([FromBody] CustomerDto customerDto)
     {
         try
         {
             var newCustomer = _reservationService.AddNewCustomerAsync(customerDto);
             return Ok(newCustomer);
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw new Exception(e.Message);
         }
     }
}