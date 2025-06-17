using System;

namespace DailyHabitTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            HabitManager habitManager = new HabitManager();
            bool running = true;

            Console.WriteLine("Welcome to Daily Habit Tracker!");
            Console.WriteLine("================================");

            while (running)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddHabit(habitManager);
                        break;
                    case "2":
                        MarkHabitCompleted(habitManager);
                        break;
                    case "3":
                        habitManager.DisplayHabits();
                        break;
                    case "4":
                        habitManager.DisplayStreaks();
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("Thank you for using Daily Habit Tracker. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Add New Habit");
            Console.WriteLine("2. Mark Habit as Completed for Today");
            Console.WriteLine("3. View Today's Habits");
            Console.WriteLine("4. View Habit Streaks");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice (1-5): ");
        }

        static void AddHabit(HabitManager habitManager)
        {
            Console.Write("Enter habit name (e.g., 'Drink Water', 'Exercise'): ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Habit name cannot be empty.");
                return;
            }

            habitManager.AddHabit(name);
            Console.WriteLine($"Habit '{name}' added successfully!");
        }

        static void MarkHabitCompleted(HabitManager habitManager)
        {
            Console.Write("Enter habit name to mark as completed: ");
            string name = Console.ReadLine();

            if (habitManager.MarkHabitCompleted(name))
            {
                Console.WriteLine($"Habit '{name}' marked as completed for today!");
            }
            else
            {
                Console.WriteLine($"Habit '{name}' not found. Please check the name and try again.");
            }
        }
    }
}