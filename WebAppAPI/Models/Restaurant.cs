using System.Text.Json.Serialization;

namespace WebAppAPI.Models;

public class Restaurants
{
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string RestName { get; set; }
    [JsonPropertyName("address")]
    public string RestAddress { get; set; }
    public string ImageUrl { get; set; }
    public int Rating { get; set; }
}