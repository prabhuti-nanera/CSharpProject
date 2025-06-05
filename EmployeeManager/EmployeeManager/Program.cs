using System;
using static EmployeeManager;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Employee Manager Project ===");

        EmployeeManager manager = new EmployeeManager();

        manager.AddEmployee(new FullTimeEmployee(1, "Alice", "Manager"));
        manager.AddEmployee(new PartTimeEmployee(2, "Bob", "Developer"));
        manager.AddEmployee(new FullTimeEmployee(3, "Charlie", "Tester"));

        Console.WriteLine("\nEmployee List:");
        manager.DisplayEmployees();

        Reminder reminder = new Reminder
        {
            Message = "Team Meeting",
            ReminderTime = DateTime.Now.AddHours(1)
        };

        Console.WriteLine($"\nReminder: {reminder.Message} at {reminder.ReminderTime}");
    }
}
