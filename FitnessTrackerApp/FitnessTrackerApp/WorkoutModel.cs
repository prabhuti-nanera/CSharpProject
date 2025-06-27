namespace FitnessTrackerApp;

public class WorkoutModel
{
    public string? Name { get; set; }
    public string? Notes { get; set; }
    public string? Intensity { get; set; }
    public bool IsOutdoor { get; set; }
    public bool TrackProgress { get; set; }
    public DateTime WorkoutDate { get; set; }
    public TimeSpan WorkoutTime { get; set; }
    public double Duration { get; set; }
    public int Sets { get; set; }
}