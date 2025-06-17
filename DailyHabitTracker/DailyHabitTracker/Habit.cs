using System;
using System.Collections.Generic;
using System.Linq;

namespace DailyHabitTracker
{
    public class Habit
    {
        public string Name { get; set; }
        public List<DateTime> CompletionDates { get; set; }

        public Habit(string name)
        {
            Name = name;
            CompletionDates = new List<DateTime>();
        }

        public bool IsCompletedToday()
        {
            DateTime today = DateTime.Today;
            return CompletionDates.Exists(date => date.Date == today);
        }

        public int CalculateStreak()
        {
            if (CompletionDates.Count == 0)
                return 0;

            // Sort dates in descending order (most recent first)
            var sortedDates = CompletionDates.OrderByDescending(d => d.Date).ToList();
            int streak = 1; // Start with today if completed
            DateTime currentDate = DateTime.Today;

            // If not completed today, start from the most recent completion
            if (!IsCompletedToday())
            {
                if (sortedDates[0].Date >= currentDate)
                    return 0; // No streak if the most recent completion is in the future
                currentDate = sortedDates[0].Date;
                streak = 0; // Reset streak since today isn't completed
            }

            // Check for consecutive days
            for (int i = 0; i < sortedDates.Count; i++)
            {
                if (sortedDates[i].Date == currentDate)
                {
                    streak++;
                    currentDate = currentDate.AddDays(-1);
                }
                else if (sortedDates[i].Date > currentDate)
                {
                    continue; // Skip future dates
                }
                else
                {
                    break; // Streak breaks if there's a gap
                }
            }

            return streak - 1; // Subtract 1 because we don't count today if it was the starting point
        }
    }
}