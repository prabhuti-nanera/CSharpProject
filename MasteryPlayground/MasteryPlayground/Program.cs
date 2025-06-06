using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace CSharpConceptsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to C# Concepts Demo!");
            Console.WriteLine("Arguments passed:");
            foreach (string arg in args)
                Console.WriteLine(arg);

            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            int age = 30;
            double height = 5.9;
            bool isLearning = true;
            char initial = 'A';
            object anything = "Can hold any type";

            double sum = age + height;
            int castedHeight = (int)height; 

            Console.WriteLine($"Hello {name}, you are {age} years old.");

            string text = "Hello World!";
            Console.WriteLine(text.ToUpper());
            Console.WriteLine(text.ToLower());
            Console.WriteLine(text.Substring(0, 5));

            Console.WriteLine("Line1\nLine2");
            string path = @"C:\Users\Name";

            int[] numbers = { 1, 2, 3, 4, 5 };
            for (int i = 0; i < numbers.Length; i++)
                Console.Write(numbers[i] + " ");

            Console.WriteLine();
            foreach (int num in numbers)
                Console.Write(num + " ");

            if (age > 18)
                Console.WriteLine("\nAdult");
            else if (age == 18)
                Console.WriteLine("Just an adult");
            else
                Console.WriteLine("Minor");

            string result = (age > 18) ? "Adult" : "Minor"; 
            Console.WriteLine(result);

            int switchCase = 2;
            switch (switchCase)
            {
                case 1: Console.WriteLine("One"); break;
                case 2: Console.WriteLine("Two"); break;
                default: Console.WriteLine("Other"); break;
            }

            int counter = 0;
            while (counter < 3)
            {
                Console.WriteLine("While: " + counter);
                counter++;
            }

            do
            {
                Console.WriteLine("Do While once");
            } while (false);

            Random rand = new Random();
            Console.WriteLine("Random: " + rand.Next(1, 100));

            try
            {
                int zero = 0;
                int bad = 5 / zero;
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Cannot divide by zero.");
            }

            StringBuilder sb = new StringBuilder("Hello");
            sb.Append(" World");
            Console.WriteLine(sb.ToString());

            int addResult = Add(2, 3);
            Console.WriteLine("Add: " + addResult);

            OutExample(out int outputVar);
            Console.WriteLine("Out: " + outputVar);

            int refVar = 5;
            PassByRef(ref refVar);
            Console.WriteLine("Ref Var: " + refVar);

            PrintNumbers(1, 2, 3, 4);

            Overload(1);
            Overload("Hello");

            DateTime now = DateTime.Now;
            TimeSpan ts = new TimeSpan(1, 2, 3);
            Console.WriteLine(now + " " + ts);

            Console.WriteLine(MyEnum.Monday);
            Person p = new Person("John", 25);
            p.Display();

            MyStruct ms = new MyStruct(10, 20);
            ms.Display();

            int? nullableInt = null;
            Console.WriteLine(nullableInt.HasValue);

            p.Age = 30;
            Console.WriteLine(p.Age);

            Console.WriteLine(Person.PI);

            Animal a = new Animal();
            Animal d = new Dog();
            a.Speak();
            d.Speak();

            OuterClass oc = new OuterClass();
            OuterClass.InnerClass ic = new OuterClass.InnerClass();
            ic.InnerMethod();

            AbstractAnimal cat = new Cat();
            cat.MakeSound();

            IMyInterface impl = new Implementation();
            impl.Show();

            ComplexOOPExample.Run();

            WarriorsFight.Run();

            ArrayList arrList = new ArrayList { 1, "two", 3.0 };
            Dictionary<string, int> dict = new Dictionary<string, int> { { "one", 1 } };
            Queue<int> queue = new Queue<int>();
            Stack<int> stack = new Stack<int>();
            queue.Enqueue(10);
            stack.Push(20);

            MyGenerics.Show();

            MyDelegate del = new MyDelegate(MyGenerics.Square);
            Console.WriteLine("Delegate: " + del(5));

            var squares = Enumerable.Range(1, 5).Select(x => x * x);
            foreach (var s in squares)
                Console.Write(s + " ");

            Thread t = new Thread(() => Console.WriteLine("Thread running!"));
            t.Start();

            File.WriteAllText("test.txt", "Hello File!");
            string content = File.ReadAllText("test.txt");
            Console.WriteLine(content);

            Console.WriteLine(Directory.GetCurrentDirectory());
        }

        static int Add(int a, int b) => a + b;

        static void OutExample(out int num) => num = 100;

        static void PassByRef(ref int num) => num *= 2;

        static void PrintNumbers(params int[] nums)
        {
            foreach (var n in nums)
                Console.Write(n + " ");
            Console.WriteLine();
        }

        static void Overload(int a) => Console.WriteLine("Overload Int");
        static void Overload(string a) => Console.WriteLine("Overload String");
    }
}
