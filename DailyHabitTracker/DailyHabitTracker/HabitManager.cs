using System;
using System.Collections.Generic;

namespace DailyHabitTracker
{
    public class HabitManager
    {
        private Dictionary<string, Habit> habits;

        public HabitManager()
        {
            habits = new Dictionary<string, Habit>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddHabit(string name)
        {
            if (!habits.ContainsKey(name))
            {
                habits[name] = new Habit(name);
            }
            else
            {
                Console.WriteLine($"Habit '{name}' already exists.");
            }
        }

        public bool MarkHabitCompleted(string name)
        {
            if (habits.TryGetValue(name, out Habit habit))
            {
                DateTime today = DateTime.Today;
                if (!habit.IsCompletedToday())
                {
                    habit.CompletionDates.Add(today);
                }
                else
                {
                    Console.WriteLine($"Habit '{name}' is already marked as completed for today.");
                }
                return true;
            }
            return false;
        }

        public void DisplayHabits()
        {
            if (habits.Count == 0)
            {
                Console.WriteLine("No habits found. Add some habits to start tracking!");
                return;
            }

            Console.WriteLine("\nToday's Habits:");
            Console.WriteLine("===============");
            Console.WriteLine("Habit Name          | Completed Today");
            Console.WriteLine("-------------------------------------");
            foreach (var habit in habits.Values)
            {
                string status = habit.IsCompletedToday() ? "Yes" : "No";
                Console.WriteLine($"{habit.Name,-20} | {status}");
            }
        }

        public void DisplayStreaks()
        {
            if (habits.Count == 0)
            {
                Console.WriteLine("No habits found. Add some habits to start tracking!");
                return;
            }

            Console.WriteLine("\nHabit Streaks:");
            Console.WriteLine("==============");
            Console.WriteLine("Habit Name          | Streak (Days)");
            Console.WriteLine("-------------------------------------");
            foreach (var habit in habits.Values)
            {
                int streak = habit.CalculateStreak();
                Console.WriteLine($"{habit.Name,-20} | {streak}");
                if (streak >= 7)
                {
                    Console.WriteLine($"Great job! You've maintained '{habit.Name}' for {streak} days!");
                }
            }
        }
    }
}