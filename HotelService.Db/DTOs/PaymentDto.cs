namespace HotelService.Db.DTOs;

public class PaymentDto
{
    public int CustomerId { get; set; }
    public string CardType { get; set; }
    public string CardNumber { get; set; }
    public string ExpirationDate { get; set; }
    public string CVV { get; set; }
    public string BillingAddress { get; set; }
}
