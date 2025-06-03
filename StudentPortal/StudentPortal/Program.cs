using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementSystem
{
 
    interface IReportable
    {
        void DisplayInfo();
    }

    class Student : IReportable
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<int> Grades { get; private set; }

        public Student(int id, string name, List<int> grades)
        {
            Id = id;
            Name = name;
            Grades = grades;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}, Name: {Name}, Grades: {string.Join(", ", Grades)}");
        }

        public double AverageGrade()
        {
            return Grades.Average();
        }
    }

    class GraduateStudent : Student
    {
        public string ThesisTitle { get; private set; }

        public GraduateStudent(int id, string name, List<int> grades, string thesis)
            : base(id, name, grades)
        {
            ThesisTitle = thesis;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Thesis Title: {ThesisTitle}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var studentIds = Enumerable.Range(1, 5);

            var students = new List<Student>
            {
                new Student(1, "Alice", new List<int> {85, 90, 78}),
                new GraduateStudent(2, "Bob", new List<int> {60, 70, 65}, "AI in Education"),
                new Student(3, "Charlie", new List<int> {95, 88, 92}),
                new Student(4, "David", new List<int> {55, 60, 58}),
                new GraduateStudent(5, "Eve", new List<int> {70, 80, 75}, "Cybersecurity in IoT")
            };

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- Student Management System ---");
                Console.WriteLine("1. Display All Students");
                Console.WriteLine("2. Show Top Students (average > 75)");
                Console.WriteLine("3. Check if All Students Passed (>= 60)");
                Console.WriteLine("4. Show Distinct Grades");
                Console.WriteLine("5. Show Common Grades between 2 Students");
                Console.WriteLine("6. Zip IDs and Names");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\nAll Students:");
                        foreach (var s in students)
                        {
                            s.DisplayInfo();
                        }
                        break;

                    case "2":
                        var topStudents = students.Where(s => s.AverageGrade() > 75);
                        Console.WriteLine("\nTop Students:");
                        foreach (var s in topStudents)
                        {
                            Console.WriteLine($"{s.Name}: Avg Grade = {s.AverageGrade()}");
                        }
                        break;

                    case "3":
                        var allPassed = students.All(s => s.Grades.All(g => g >= 60));
                        Console.WriteLine($"\nAll students passed? {allPassed}");
                        break;

                    case "4":
                        var distinctGrades = students.SelectMany(s => s.Grades).Distinct();
                        Console.WriteLine("\nDistinct Grades:");
                        foreach (var grade in distinctGrades)
                        {
                            Console.WriteLine(grade);
                        }
                        break;

                    case "5":
                        Console.Write("\nEnter first student ID: ");
                        int id1 = int.Parse(Console.ReadLine());
                        Console.Write("Enter second student ID: ");
                        int id2 = int.Parse(Console.ReadLine());

                        var student1 = students.FirstOrDefault(s => s.Id == id1);
                        var student2 = students.FirstOrDefault(s => s.Id == id2);

                        if (student1 != null && student2 != null)
                        {
                            var commonGrades = student1.Grades.Intersect(student2.Grades);
                            Console.WriteLine("Common Grades:");
                            foreach (var grade in commonGrades)
                            {
                                Console.WriteLine(grade);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid student IDs.");
                        }
                        break;

                    case "6":
                        var names = students.Select(s => s.Name);
                        var zipped = studentIds.Zip(names, (id, name) => $"ID: {id} - Name: {name}");
                        Console.WriteLine("\nZipped IDs and Names:");
                        foreach (var item in zipped)
                        {
                            Console.WriteLine(item);
                        }
                        break;

                    case "7":
                        exit = true;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}
