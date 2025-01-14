namespace WebAppAPI.Models;

public class Review
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RestaurantId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; } // e.g., 1-5
    public User User { get; set; }
    public Restaurant Restaurant { get; set; }
}