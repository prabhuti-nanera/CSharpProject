namespace UltimateControlsDemo.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; } // Made nullable to avoid CS8618 warning
        public bool IsDone { get; set; }
    }
}