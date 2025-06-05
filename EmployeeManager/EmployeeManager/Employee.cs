using System;

public interface IDisplayable
{
    void Display();
}

public abstract class Employee : IDisplayable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }

    public Employee() { }

    public Employee(int id, string name, string position)
    {
        Id = id;
        Name = name;
        Position = position;
    }

    public abstract void Display();
}
public class FullTimeEmployee : Employee
{
    public FullTimeEmployee() { }

    public FullTimeEmployee(int id, string name, string position)
        : base(id, name, position) { }

    public override void Display()
    {
        Console.WriteLine($"[Full-time] ID: {Id}, Name: {Name}, Position: {Position}");
    }
}

public class PartTimeEmployee : Employee
{
    public PartTimeEmployee() { }

    public PartTimeEmployee(int id, string name, string position)
        : base(id, name, position) { }

    public override void Display()
    {
        Console.WriteLine($"[Part-time] ID: {Id}, Name: {Name}, Position: {Position}");
    }
}

public struct Reminder
{
    public string Message;
    public DateTime ReminderTime;
}
