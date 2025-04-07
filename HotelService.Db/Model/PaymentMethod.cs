using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelService.Db.Model;

public class PaymentMethod
{
    [Key]
    public int PaymentMethodId { get; set; }
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public string CardType { get; set; }
    public string CardNumber { get; set; }
    public string ExpirationDate { get; set; }
    public string CVV { get; set; }
    public string BillingAddress { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}