using HotelService.Db.DTOs;
using HotelService.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers;
[Route("api/[controller]")]
[ApiController]


public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> PaymentAsync([FromBody] PaymentDto request)
    {
        try
        {
            var payment = await _paymentService.PaymentProcessAsync(request);
            if (payment == null)
            {
                return BadRequest();
            }
            return Ok(new { PaymentMethodId = payment.PaymentMethodId });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    } 
}