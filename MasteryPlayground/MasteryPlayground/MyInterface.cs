using System;

namespace CSharpConceptsDemo
{
    interface IMyInterface
    {
        void Show();
    }

    class Implementation : IMyInterface
    {
        public void Show() => Console.WriteLine("Interface Implementation");
    }
}
