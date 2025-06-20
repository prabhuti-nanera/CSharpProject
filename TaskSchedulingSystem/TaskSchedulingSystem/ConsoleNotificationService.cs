using System;

namespace TaskSchedulingSystem
{
    public class ConsoleNotificationService : INotificationService
    {
        public void NotifyOverdue(TaskItem task)
        {
            Console.WriteLine(string.Format("[OVERDUE] Task ID: {0}, Description: {1}, Due: {2:yyyy-MM-dd}",
                task.Id, task.Description, task.DueDate));
        }
    }
}