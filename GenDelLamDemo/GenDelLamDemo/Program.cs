using System;
using System.Collections.Generic;
using System.Linq;

namespace GenDelLamDemo
{
    class Pair<T1, T2>
    {
        public T1 A { get; }
        public T2 B { get; }
        public Pair(T1 a, T2 b) { A = a; B = b; }
    }

    class Student
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }

    class Program
    {
        delegate bool Predicate<Student>(Student s);

        static void Main()
        {
            var students = new List<Student>
            {
                new Student { Name = "Alice", Score = 85 },
                new Student { Name = "Bob", Score = 92 },
                new Student { Name = "Charlie", Score = 78 },
                new Student { Name = "David", Score = 95 },
                new Student { Name = "Eva", Score = 88 }
            };

            Predicate<Student> isHigh = s => s.Score > 80;

            var high = students.Where(s => isHigh(s));
            var names = high.Select(s => s.Name.ToUpper());

            Console.WriteLine("High scorers:");
            foreach (var n in names) Console.WriteLine(n);

            var ids = Enumerable.Range(1001, students.Count);
            var pairs = students.Zip(ids, (s, id) => new Pair<int, string>(id, s.Name));

            Console.WriteLine("\nIDs and Names:");
            foreach (var p in pairs) Console.WriteLine($"{p.A}: {p.B}");

            int total = students.Select(s => s.Score).Aggregate((x, y) => x + y);
            double avg = students.Select(s => s.Score).Average();
            bool allAbove75 = students.All(s => s.Score > 75);
            bool anyAbove90 = students.Any(s => s.Score > 90);

            Console.WriteLine($"\nTotal: {total}");
            Console.WriteLine($"Average: {avg:F2}");
            Console.WriteLine($"All > 75? {allAbove75}");
            Console.WriteLine($"Any > 90? {anyAbove90}");
        }
    }
}
