using HotelService.Db.DTOs;
using HotelService.Logic;
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
     public async Task<ActionResult<CustomerDto>> Login([FromBody] CustomerDto customerDto)
     {
         try
         {
             var newCustomer = _customerService.AddNewCustomerAsync(customerDto);
             return Ok(newCustomer);
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw new Exception(e.Message);
         }
     }

    [HttpPatch("changeData")]
    public async Task<ActionResult<CustomerDto>> ChangeData([FromBody] CustomerDto customerDto)
    {
        try
        {
            var updatedCustomer = _customerService.ChangeData(customerDto);
            return Ok(updatedCustomer);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}