using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HeroBattleSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n=== Welcome to Hero Battle Simulator ===");
            Console.Write("Enter your hero's name: ");
            string heroName = Console.ReadLine();

            Hero player = new Hero(heroName, HeroType.Warrior, 100);
            Hero enemy = new Hero("Goblin", HeroType.Archer, 80);

            Console.WriteLine($"\n{player.Name} VS {enemy.Name}");
            BattleSimulator.StartBattle(player, enemy);

            DemoAllConcepts(); // Covers additional concepts not in battle
        }

        static void DemoAllConcepts()
        {
            // Casting, Nullable, StringBuilder
            double precise = 45.8;
            int casted = (int)precise;
            int? maybeNull = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("This is a string builder\n");

            // DateTime, TimeSpan, Enum
            DateTime start = DateTime.Now;
            TimeSpan duration = TimeSpan.FromSeconds(3);
            HeroType type = HeroType.Mage;

            // Functions, Params, Out, Ref
            int result;
            Multiply(3, 5, out result);
            int power = 10;
            Increase(ref power);
            PrintNames("Archer", "Mage", "Rogue");

            // Interface, Abstract, Inheritance, Polymorphism
            Fighter mage = new Mage("Zara");
            mage.Fight();
            ((IHealer)mage).Heal();

            // Struct
            Item potion = new Item { Name = "Health Potion" };

            // Inner Class
            Outer.Inner inner = new Outer.Inner();
            inner.Show();

            // Collections
            ArrayList arrayList = new ArrayList() { 1, "two", 3.0 };
            Dictionary<string, int> levels = new Dictionary<string, int> { { "A", 1 }, { "B", 2 } };
            Queue<string> queue = new Queue<string>(); queue.Enqueue("Task1");
            Stack<string> stack = new Stack<string>(); stack.Push("Undo");

            // Constant, Readonly
            Config cfg = new Config(5);

            Console.WriteLine(sb.ToString());
        }

        static void Multiply(int a, int b, out int result) => result = a * b;
        static void Increase(ref int num) => num += 5;
        static void PrintNames(params string[] names)
        {
            foreach (var name in names)
                Console.WriteLine($"Name: {name}");
        }
    }

    enum HeroType { Warrior, Mage, Archer }

    class Hero : Fighter
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public HeroType Type { get; set; }
        private static int count = 0;

        public Hero(string name, HeroType type, int health)
        {
            this.Name = name;
            this.Type = type;
            this.Health = health;
            count++;
        }

        public override void Fight()
        {
            Console.WriteLine($"{Name} attacks with {Type} power!");
        }
    }

    abstract class Fighter
    {
        public abstract void Fight();
    }

    interface IHealer
    {
        void Heal();
    }

    class Mage : Fighter, IHealer
    {
        private string Name;
        public Mage(string name) { Name = name; }
        public override void Fight() => Console.WriteLine($"{Name} casts a fireball!");
        public void Heal() => Console.WriteLine($"{Name} heals an ally.");
    }

    struct Item
    {
        public string Name;
    }

    class Outer
    {
        public class Inner
        {
            public void Show() => Console.WriteLine("Inner class activated!");
        }
    }

    class Config
    {
        public const string GameName = "Hero Battle";
        public readonly int MaxPlayers;

        public Config(int max) => MaxPlayers = max;
    }

    static class BattleSimulator
    {
        public static void StartBattle(Hero hero1, Hero hero2)
        {
            Random rand = new Random();

            while (hero1.Health > 0 && hero2.Health > 0)
            {
                int h1Hit = rand.Next(5, 15);
                int h2Hit = rand.Next(5, 15);

                hero2.Health -= h1Hit;
                hero1.Health -= h2Hit;

                Console.WriteLine($"{hero1.Name} hits {hero2.Name} for {h1Hit}, {hero2.Name} HP: {hero2.Health}");
                Console.WriteLine($"{hero2.Name} hits {hero1.Name} for {h2Hit}, {hero1.Name} HP: {hero1.Health}");

                System.Threading.Thread.Sleep(500);
            }

            string winner = hero1.Health > 0 ? hero1.Name : hero2.Name;
            Console.WriteLine($"\nBattle Over! Winner: {winner}");
        }
    }
}
