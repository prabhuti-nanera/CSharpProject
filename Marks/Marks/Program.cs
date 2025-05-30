using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marks
{
    class Student
    {
        public string Name;
        public int[] Marks = new int[3];

        public int GetTotal()
        {
            int total = 0;
            for (int i = 0; i < Marks.Length; i++)
            {
                total += Marks[i];
            }
            return total;
        }

        public double GetAverage()
        {
            return GetTotal() / 3.0;
        }

        public string GetGrade()
        {
            double avg = GetAverage();

            if (avg >= 90)
                return "A";
            else if (avg >= 75)
                return "B";
            else if (avg >= 50)
                return "C";
            else
                return "Fail";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Student student = new Student();

            Console.Write("Enter Student Name: ");
            student.Name = Console.ReadLine();

            for (int i = 0; i < 3; i++)
            {
                Console.Write($"Enter marks for subject {i + 1}: ");
                student.Marks[i] = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("\n--- Report Card ---");
            Console.WriteLine($"Name   : {student.Name}");
            Console.WriteLine($"Total  : {student.GetTotal()}");
            Console.WriteLine($"Average: {student.GetAverage():F2}");
            Console.WriteLine($"Grade  : {student.GetGrade()}");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();

        }
    }
}
