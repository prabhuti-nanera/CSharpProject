using System.Collections.Generic;

namespace TaskSchedulingSystem
{
    public interface ITaskRepository
    {
        List<TaskItem> GetTasks();
        void AddTask(TaskItem task);
        void UpdateTask(TaskItem task);
    }
}