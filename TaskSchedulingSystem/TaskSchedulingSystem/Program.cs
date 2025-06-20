using System;
using System.Collections.Generic;

namespace TaskSchedulingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Manual dependency injection
            ITaskRepository repository = new TaskRepository();
            INotificationService notificationService = new ConsoleNotificationService();
            TaskSchedulerService scheduler = new TaskSchedulerService(repository, notificationService);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\nTask Scheduling System");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. Simulate Task Execution");
                Console.WriteLine("3. View Pending Tasks");
                Console.WriteLine("4. Show Task Priority Distribution");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddTask(scheduler);
                            break;
                        case "2":
                            scheduler.SimulateTaskExecution();
                            break;
                        case "3":
                            ViewPendingTasks(scheduler);
                            break;
                        case "4":
                            ShowTaskDistribution(scheduler);
                            break;
                        case "5":
                            running = false;
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        static void AddTask(TaskSchedulerService scheduler)
        {
            Console.Write("Enter task description: ");
            string description = Console.ReadLine();
            Console.Write("Enter priority (Low/Medium/High): ");
            if (!Enum.TryParse(description, true, out Priority priority))
                throw new ArgumentException("Invalid priority.");
            Console.Write("Enter due date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
                throw new ArgumentException("Invalid date format.");

            scheduler.AddTask(description, priority, dueDate);
            Console.WriteLine("Task added successfully!");
        }

        static void ViewPendingTasks(TaskSchedulerService scheduler)
        {
            List<TaskItem> tasks = scheduler.GetPendingTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("No pending tasks.");
                return;
            }

            Console.WriteLine("\nPending Tasks:");
            foreach (TaskItem task in tasks)
            {
                Console.WriteLine(task);
            }
        }

        static void ShowTaskDistribution(TaskSchedulerService scheduler)
        {
            Dictionary<Priority, int> distribution = scheduler.GetTaskDistributionByPriority();
            if (distribution.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                return;
            }

            Console.WriteLine("\nTask Distribution by Priority:");
            int lowCount = distribution.ContainsKey(Priority.Low) ? distribution[Priority.Low] : 0;
            int mediumCount = distribution.ContainsKey(Priority.Medium) ? distribution[Priority.Medium] : 0;
            int highCount = distribution.ContainsKey(Priority.High) ? distribution[Priority.High] : 0;

            foreach (KeyValuePair<Priority, int> pair in distribution)
            {
                Console.WriteLine(string.Format("{0}: {1} tasks", pair.Key, pair.Value));
            }

            // Text-based chart simulation
            Console.WriteLine("\nPriority Distribution Chart (approximate):");
            Console.WriteLine("Low    : " + new string('*', lowCount * 2));
            Console.WriteLine("Medium : " + new string('*', mediumCount * 2));
            Console.WriteLine("High   : " + new string('*', highCount * 2));
        }
    }
}