using System;
using System.Collections.Generic;

namespace CSharpConceptsDemo
{
    delegate int MyDelegate(int x);

    static class MyGenerics
    {
        public static int Square(int x) => x * x;

        public static void Show()
        {
            List<int> list = new List<int> { 1, 2, 3 };
            list.ForEach(i => Console.Write(i + " "));
            Console.WriteLine();
        }
    }
}
