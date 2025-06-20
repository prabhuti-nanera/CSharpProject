namespace TaskSchedulingSystem
{
    public interface INotificationService
    {
        void NotifyOverdue(TaskItem task);
    }
}