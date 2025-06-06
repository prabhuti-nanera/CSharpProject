using System;

namespace CSharpConceptsDemo
{
    class Animal
    {
        public virtual void Speak() => Console.WriteLine("Animal speaks");
    }
    class Dog : Animal
    {
        public override void Speak() => Console.WriteLine("Dog barks");
    }

    abstract class AbstractAnimal
    {
        public abstract void MakeSound();
    }

    class Cat : AbstractAnimal
    {
        public override void MakeSound() => Console.WriteLine("Meow");
    }
}
