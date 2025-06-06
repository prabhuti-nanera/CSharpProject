using System;

namespace CSharpConceptsDemo
{
    struct MyStruct
    {
        public int X, Y;
        public MyStruct(int x, int y) { X = x; Y = y; }
        public void Display() => Console.WriteLine($"Struct: {X}, {Y}");
    }
}
