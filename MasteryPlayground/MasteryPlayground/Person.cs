using System;

namespace CSharpConceptsDemo
{
    class Person
    {
        public const double PI = 3.14;
        public readonly string Name;
        private int age;

        public Person(string name, int age)
        {
            this.Name = name;
            this.age = age;
        }

        public int Age { get => age; set => age = value; }
        public void Display() => Console.WriteLine($"{Name}, {Age}");
    }
}
