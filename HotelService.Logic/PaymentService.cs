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
        var hashedNumber = BCrypt.Net.BCrypt.HashPassword(request.CardNumber);
        var expirationDate =   BCrypt.Net.BCrypt.HashPassword( request.ExpirationDate);
        var cvvHashedNumber = BCrypt.Net.BCrypt.HashPassword(request.CVV);
        var billingAddressHashed =  BCrypt.Net.BCrypt.HashPassword( request.BillingAddress);
        var paymentMethod = new PaymentMethod
        {
            CustomerId = request.CustomerId,
            CardType = request.CardType,
            CardNumber = hashedNumber,
            ExpirationDate = expirationDate,
            CVV = cvvHashedNumber,
            BillingAddress = billingAddressHashed
        };
        await _context.PaymentMethods.AddAsync(paymentMethod);
        await _context.SaveChangesAsync();
        return paymentMethod;
    }
}