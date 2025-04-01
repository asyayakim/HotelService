namespace HotelService.Db;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }
    public double LogPrice { get; set; }
    public bool HasThumbnail { get; set; } 
}