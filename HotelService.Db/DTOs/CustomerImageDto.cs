namespace HotelService.Db.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class CustomerImageDto
{
   [FromForm(Name = "image")]
   public IFormFile Image { get; set; } = null!;
   [FromForm(Name = "userId")]
   public int UserId {get; set;}
}