using System;

namespace TaskSchedulingSystem
{
    public enum Priority { Low, Medium, High }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }

        public override string ToString()
        {
            return string.Format("ID: {0}, Description: {1}, Priority: {2}, Due: {3:yyyy-MM-dd}, Completed: {4}",
                Id, Description, Priority, DueDate, IsCompleted);
        }
    }
}