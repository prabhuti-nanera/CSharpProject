namespace TravelBuddy.Models;

public class TravelPlan
{
    public string Id { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime TravelDate { get; set; }
    public TimeSpan TravelTime { get; set; }
    public bool IsFavorite { get; set; }
}