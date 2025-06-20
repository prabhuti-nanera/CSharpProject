using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TaskSchedulingSystem
{
    public class TaskSchedulerService
    {
        private readonly ITaskRepository _repository;
        private readonly INotificationService _notificationService;
        private readonly CustomPriorityQueue<TaskItem> _taskQueue;

        public TaskSchedulerService(ITaskRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _taskQueue = new CustomPriorityQueue<TaskItem>();
        }

        public void AddTask(string description, Priority priority, DateTime dueDate)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty");

            var task = new TaskItem
            {
                Description = description,
                Priority = priority,
                DueDate = dueDate,
                IsCompleted = false
            };
            _repository.AddTask(task);
            EnqueueTask(task);
        }

        public void SimulateTaskExecution()
        {
            List<TaskItem> tasks = _repository.GetTasks();
            foreach (TaskItem task in tasks.Where(t => !t.IsCompleted && t.DueDate < DateTime.Now))
            {
                _notificationService.NotifyOverdue(task);
            }

            while (_taskQueue.Count > 0)
            {
                if (_taskQueue.TryDequeue(out TaskItem task) && !task.IsCompleted)
                {
                    task.IsCompleted = true;
                    task.CompletionDate = DateTime.Now;
                    _repository.UpdateTask(task);
                    Console.WriteLine(string.Format("Executed Task ID: {0}, Description: {1}", task.Id, task.Description));
                    Thread.Sleep(500); // Simulate processing time
                }
            }
        }

        public List<TaskItem> GetPendingTasks()
        {
            return _repository.GetTasks().Where(t => !t.IsCompleted).OrderBy(t => t.DueDate).ToList();
        }

        public Dictionary<Priority, int> GetTaskDistributionByPriority()
        {
            return _repository.GetTasks()
                .GroupBy(t => t.Priority)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        private void EnqueueTask(TaskItem task)
        {
            int priorityValue = task.Priority switch
            {
                Priority.High => 1,
                Priority.Medium => 2,
                Priority.Low => 3,
                _ => 3
            };
            _taskQueue.Enqueue(task, priorityValue);
        }
    }
}