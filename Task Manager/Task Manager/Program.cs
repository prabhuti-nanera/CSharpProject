using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace TaskManagerApp
{
    struct Reminder
    {
        public string Message;
        public DateTime ReminderTime;
    }

    interface INotifiable
    {
        void Notify();
    }

    abstract class TaskBase
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public string OptionalNote { get; set; } 

        public abstract void Display();
    }

    class WorkTask : TaskBase, INotifiable
    {
        public string ProjectName { get; set; }

        public override void Display()
        {
            string noteDisplay = string.IsNullOrEmpty(OptionalNote) ? "None" : OptionalNote;
            Console.WriteLine($"[Work] {Title} (Due: {DueDate}, Priority: {Priority}, Project: {ProjectName}, Note: {noteDisplay})");
        }

        public void Notify()
        {
            Console.WriteLine($"Reminder: Work task '{Title}' is due on {DueDate.ToShortDateString()}!");
        }
    }

    class PersonalTask : TaskBase
    {
        public string Notes { get; set; }

        public class Tag
        {
            public string Label { get; set; }
        }

        public Tag PersonalTag { get; set; } = new Tag { Label = "Home" };

        public override void Display()
        {
            string noteDisplay = string.IsNullOrEmpty(OptionalNote) ? "None" : OptionalNote;
            Console.WriteLine($"[Personal] {Title} (Due: {DueDate}, Priority: {Priority}, Notes: {Notes}, Tag: {PersonalTag.Label}, Note: {noteDisplay})");
        }
    }

    class Program
    {
        static List<TaskBase> tasks = new List<TaskBase>();
        static Queue<Reminder> reminders = new Queue<Reminder>();
        static Stack<string> taskHistory = new Stack<string>();
        static Dictionary<string, string> taskOwners = new Dictionary<string, string>();
        static ArrayList quickNotes = new ArrayList();

        public delegate void TaskAddedHandler(TaskBase task);
        public static event TaskAddedHandler OnTaskAdded;

        static void Main()
        {
            // Setup event
            OnTaskAdded += task => Console.WriteLine($"New task added: {task.Title}");

            // Add tasks
            AddTask(new WorkTask
            {
                Title = "Prepare report",
                DueDate = DateTime.Today.AddDays(2),
                Priority = 1,
                ProjectName = "Project Alpha",
                OptionalNote = "Check data first"
            });
            AddTask(new PersonalTask
            {
                Title = "Buy groceries",
                DueDate = DateTime.Today.AddDays(1),
                Priority = 2,
                Notes = "Milk, eggs, bread",
                OptionalNote = null 
            });
            var highPriorityTasks = tasks.Where(t => t.Priority == 1);
            Console.WriteLine("\nHigh Priority Tasks:");
            foreach (var task in highPriorityTasks)
            {
                task.Display();
            }

            Console.WriteLine("\nTask Types:");
            foreach (var task in tasks)
            {
                Type type = task.GetType();
                Console.WriteLine($"- {type.Name}");
            }

            SaveTasksToFile("tasks.txt");

            try
            {
                LoadTasksFromFile("tasks.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\nTask Summary:");
            foreach (var task in tasks)
            {
                sb.AppendLine($"- {task.Title} (Priority: {task.Priority})");
            }
            Console.WriteLine(sb.ToString());

            quickNotes.Add("Call plumber");
            quickNotes.Add("Send email");
            Console.WriteLine("\nQuick Notes:");
            foreach (var note in quickNotes)
            {
                Console.WriteLine($"- {note}");
            }

            taskOwners["Prepare report"] = "Alice";
            taskOwners["Buy groceries"] = "Bob";
            Console.WriteLine("\nTask Owners:");
            foreach (var pair in taskOwners)
            {
                Console.WriteLine($"- {pair.Key}: {pair.Value}");
            }

            reminders.Enqueue(new Reminder { Message = "Meeting in 1 hour", ReminderTime = DateTime.Now.AddMinutes(60) });
            reminders.Enqueue(new Reminder { Message = "Dentist appointment tomorrow", ReminderTime = DateTime.Now.AddDays(1) });
            Console.WriteLine("\nReminders:");
            while (reminders.Count > 0)
            {
                var reminder = reminders.Dequeue();
                Console.WriteLine($"- {reminder.Message} at {reminder.ReminderTime}");
            }

            taskHistory.Push("Created report task");
            taskHistory.Push("Created groceries task");
            Console.WriteLine("\nTask History:");
            while (taskHistory.Count > 0)
            {
                Console.WriteLine($"- {taskHistory.Pop()}");
            }

            Random random = new Random();
            int randomMinutes = random.Next(10, 60);
            DateTime reminderTime = DateTime.Now.AddMinutes(randomMinutes);
            TimeSpan timeLeft = reminderTime - DateTime.Now;
            Console.WriteLine($"\nRandom Reminder in {timeLeft.Minutes} minutes at {reminderTime}");

            Console.WriteLine("\nSimulating delay (3 seconds)...");
            Thread.Sleep(3000);
            Console.WriteLine("Done!");

            Console.WriteLine("\nNotifications:");
            foreach (var task in tasks)
            {
                if (task is INotifiable notifiable)
                {
                    notifiable.Notify();
                }
            }
        }

        static void AddTask(TaskBase task)
        {
            tasks.Add(task);
            OnTaskAdded?.Invoke(task);
        }

        static void SaveTasksToFile(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var task in tasks)
                {
                    string note = string.IsNullOrEmpty(task.OptionalNote) ? "None" : task.OptionalNote;
                    writer.WriteLine($"{task.GetType().Name}|{task.Title}|{task.DueDate}|{task.Priority}|{note}");
                }
            }
            Console.WriteLine("\nTasks saved to file.");
        }

        static void LoadTasksFromFile(string filename)
        {
            Console.WriteLine("\nLoading tasks from file:");
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine($"Loaded: {line}");
                }
            }
        }
    }
}
