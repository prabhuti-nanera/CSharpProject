using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace TaskSchedulingSystem
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _filePath = "tasks.json";

        public List<TaskItem> GetTasks()
        {
            try
            {
                if (!File.Exists(_filePath)) return new List<TaskItem>();
                string json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading tasks", ex);
            }
        }

        public void AddTask(TaskItem task)
        {
            List<TaskItem> tasks = GetTasks();
            task.Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
            tasks.Add(task);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(tasks, Formatting.Indented));
        }

        public void UpdateTask(TaskItem task)
        {
            List<TaskItem> tasks = GetTasks();
            TaskItem existing = tasks.Find(t => t.Id == task.Id);
            if (existing == null) throw new Exception("Task not found");
            existing.Description = task.Description;
            existing.Priority = task.Priority;
            existing.DueDate = task.DueDate;
            existing.IsCompleted = task.IsCompleted;
            existing.CompletionDate = task.CompletionDate;
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(tasks, Formatting.Indented));
        }
    }
}