using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;

namespace HotelService.Logic;

public class PaymentService
{
    private readonly AppDbContext _context;

    public PaymentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentMethod?> PaymentProcessAsync(PaymentDto request)
    {
        var paymentMethod = new PaymentMethod
        {
            CustomerId = request.CustomerId,
            CardType = request.CardType,
            CardNumber = request.CardNumber,
            ExpirationDate = request.ExpirationDate,
            CVV = request.CVV,
            BillingAddress = request.BillingAddress
        };
        await _context.PaymentMethods.AddAsync(paymentMethod);
        await _context.SaveChangesAsync();
        return paymentMethod;
    }
}