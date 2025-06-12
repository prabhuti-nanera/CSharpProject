using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Data.SqlClient;

// Enums for serialization type
public enum SerializationType { Json, Xml, Binary }

// Interface for serialization factory
public interface ISerializerFactory
{
    ISerializer CreateSerializer(SerializationType type);
}

// Interface for serializer
public interface ISerializer
{
    string Serialize(Person person);
    Person Deserialize(string data);
}

// Base class for serializers
public abstract class BaseSerializer
{
    protected void Log(string message)
    {
        Console.WriteLine("Log: " + message);
    }

    public virtual string FormatValue(object value)
    {
        if (value == null) return "null";
        if (value is string) return "\"" + value + "\"";
        if (value is ArrayList)
        {
            ArrayList list = (ArrayList)value;
            string[] items = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                items[i] = "\"" + list[i].ToString() + "\"";
            }
            return "[" + String.Join(",", items) + "]";
        }
        return value.ToString();
    }
}

// JSON serializer implementation
public class JsonSerializer : BaseSerializer, ISerializer
{
    public string Serialize(Person person)
    {
        Log("Starting JSON serialization");

        StringBuilder json = new StringBuilder("{");
        bool first = true;

        // Serialize Name
        if (!first) json.Append(",");
        json.Append("\"fullName\":\"" + person.Name + "\"");
        first = false;

        // Serialize Age
        if (!first) json.Append(",");
        json.Append("\"Age\":" + person.Age);
        first = false;

        // Serialize Hobbies
        if (!first) json.Append(",");
        json.Append("\"Hobbies\":" + FormatValue(person.Hobbies));
        first = false;

        json.Append("}");
        string result = json.ToString();
        Log("JSON serialization complete");
        return result;
    }

    public Person Deserialize(string jsonData)
    {
        Log("Starting JSON deserialization");

        Person result = new Person();
        if (string.IsNullOrEmpty(jsonData))
        {
            Log("Empty JSON data, returning default Person");
            return result;
        }

        string trimmed = jsonData.Trim('{', '}');
        string[] pairs = trimmed.Split(',');

        // Parse each key-value pair with validation
        for (int i = 0; i < pairs.Length; i++)
        {
            string[] keyValue = pairs[i].Split(new char[] { ':' }, 2);
            if (keyValue.Length != 2)
            {
                Log("Invalid JSON format in pair: " + pairs[i]);
                continue;
            }

            string key = keyValue[0].Trim('"');
            string value = keyValue[1].Trim('"');

            if (key == "fullName")
            {
                result.Name = value;
            }
            else if (key == "Age")
            {
                int age;
                if (Int32.TryParse(value, out age))
                {
                    result.Age = age;
                }
            }
            else if (key == "Hobbies")
            {
                string listData = value.Trim('[', ']');
                string[] items = listData.Split(',');
                ArrayList hobbies = new ArrayList();
                for (int j = 0; j < items.Length; j++)
                {
                    string item = items[j].Trim('"');
                    if (!string.IsNullOrEmpty(item))
                    {
                        hobbies.Add(item);
                    }
                }
                result.Hobbies = hobbies;
            }
        }

        Log("JSON deserialization complete");
        return result;
    }
}

// XML serializer implementation
public class XmlSerializer : BaseSerializer, ISerializer
{
    public string Serialize(Person person)
    {
        Log("Starting XML serialization");
        using (StringWriter writer = new StringWriter())
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Person));
            serializer.Serialize(writer, person);
            string result = writer.ToString();
            Log("XML serialization complete");
            return result;
        }
    }

    public Person Deserialize(string xml)
    {
        Log("Starting XML deserialization");
        using (StringReader reader = new StringReader(xml))
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Person));
            Person result = (Person)serializer.Deserialize(reader);
            Log("XML deserialization complete");
            return result;
        }
    }
}

// Factory for creating serializers
public class SerializerFactory : ISerializerFactory
{
    public ISerializer CreateSerializer(SerializationType type)
    {
        switch (type)
        {
            case SerializationType.Json:
                return new JsonSerializer();
            case SerializationType.Xml:
                return new XmlSerializer();
            default:
                throw new Exception("Unsupported serialization type");
        }
    }
}

// Database manager for SQL Server
public class DatabaseManager
{
    private readonly string connectionString;
    private void Log(string message)
    {
        Console.WriteLine("DB Log: " + message);
    }

    public DatabaseManager(string connString = null)
    {
        // Update this connection string based on your SQL Server setup
        connectionString = connString ?? "Data Source=.;Initial Catalog=SerializerDB;Integrated Security=True";
        // For LocalDB: "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SerializerDB;Integrated Security=True"
        // For SQL Server Authentication: "Data Source=.;Initial Catalog=SerializerDB;User Id=sa;Password=YourPassword;"
    }

    public void SaveToDatabase(Person person, string data)
    {
        try
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO SerializedData (Type, Data) VALUES (@Type, @Data)", conn);
            cmd.Parameters.AddWithValue("@Type", person.GetType().Name);
            cmd.Parameters.AddWithValue("@Data", data);
            cmd.ExecuteNonQuery();
            conn.Close();
            Log("Saved to database");
        }
        catch (SqlException ex)
        {
            Log("Database not available, skipping save. Error: " + ex.Message);
        }
    }

    public ArrayList ReadFromDatabase(string typeName)
    {
        ArrayList results = new ArrayList();
        try
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Data FROM SerializedData WHERE Type = @Type", conn);
            cmd.Parameters.AddWithValue("@Type", typeName);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(reader.GetString(0));
            }
            reader.Close();
            conn.Close();
            Log("Read from database");
        }
        catch (SqlException ex)
        {
            Log("Database not available, returning empty list. Error: " + ex.Message);
        }
        return results;
    }
}

// Base entity class for inheritance
public abstract class Entity
{
    public string Id { get; set; }
    public virtual string GetDescription()
    {
        return "Base Entity";
    }
}

// Complex class for serialization
[Serializable]
public class Person : Entity
{
    public string Name { get; set; }
    public int Age { get; set; }
    private string Password { get; set; }
    public ArrayList Hobbies { get; set; }
    public Address HomeAddress { get; set; }

    public Person()
    {
        Password = "secret";
        Hobbies = new ArrayList();
        HomeAddress = new Address();
    }

    public override string GetDescription()
    {
        return "Person: " + Name + ", " + Age + " years old";
    }
}

// Nested class for complex object model
[Serializable]
public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
}

// Warrior class for polymorphism
[Serializable]
public class Warrior : Person
{
    public int Strength { get; set; }
    public override string GetDescription()
    {
        return "Warrior: " + Name + ", Strength: " + Strength;
    }
}

// Interface for console interaction
public interface IConsoleUI
{
    void Run();
}

// Console UI
public class ConsoleUI : IConsoleUI
{
    private readonly ISerializerFactory factory;
    private readonly DatabaseManager db;
    private readonly Random random;

    public ConsoleUI(ISerializerFactory factoryInput, DatabaseManager dbInput)
    {
        factory = factoryInput;
        db = dbInput;
        random = new Random();
    }

    public void Run()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n1. Serialize Person\n2. Deserialize Person\n3. Serialize Warrior\n4. Exit");
            Console.Write("Choose an option: ");
            string input = Console.ReadLine();

            // Handle null input and trim
            if (input == null)
            {
                Console.WriteLine("Invalid option");
                continue;
            }
            input = input.Trim();

            if (input == "1")
            {
                SerializePerson();
            }
            else if (input == "2")
            {
                DeserializePerson();
            }
            else if (input == "3")
            {
                SerializeWarrior();
            }
            else if (input == "4")
            {
                running = false;
            }
            else
            {
                Console.WriteLine("Invalid option");
            }
        }
    }

    private void SerializePerson()
    {
        Person person = new Person();
        person.Id = "P" + random.Next(1000, 9999).ToString();
        person.Name = "Alice";
        person.Age = 30;
        person.Hobbies.Add("Reading");
        person.Hobbies.Add("Gaming");
        person.HomeAddress.Street = "123 Main St";
        person.HomeAddress.City = "Boston";

        ISerializer serializer = factory.CreateSerializer(SerializationType.Json);
        try
        {
            string jsonData = serializer.Serialize(person);
            db.SaveToDatabase(person, jsonData);
            File.WriteAllText("person.json", jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private void DeserializePerson()
    {
        try
        {
            string jsonData = "";
            if (File.Exists("person.json"))
            {
                jsonData = File.ReadAllText("person.json");
            }
            else
            {
                ArrayList dbResults = db.ReadFromDatabase("Person");
                if (dbResults.Count > 0)
                {
                    jsonData = (string)dbResults[0];
                }
            }

            if (String.IsNullOrEmpty(jsonData))
            {
                Console.WriteLine("No data to deserialize");
                return;
            }

            ISerializer serializer = factory.CreateSerializer(SerializationType.Json);
            Person person = serializer.Deserialize(jsonData);
            Console.WriteLine("Deserialized: " + person.GetDescription() + ", ID: " + person.Id + ", City: " + person.HomeAddress.City);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private void SerializeWarrior()
    {
        Warrior warrior = new Warrior();
        warrior.Id = "W" + random.Next(1000, 9999).ToString();
        warrior.Name = "Bob";
        warrior.Age = 35;
        warrior.Strength = 100;
        warrior.Hobbies.Add("Fighting");
        warrior.Hobbies.Add("Training");

        ISerializer serializer = factory.CreateSerializer(SerializationType.Json);
        try
        {
            string jsonData = serializer.Serialize(warrior);
            FileStream stream = new FileStream("warrior.bin", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, warrior);
            stream.Close();
            Console.WriteLine("Warrior serialized to binary");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}

public class Program
{
    public static void Main()
    {
        SerializerFactory factory = new SerializerFactory();
        DatabaseManager db = new DatabaseManager();
        IConsoleUI ui = new ConsoleUI(factory, db);
        ui.Run();
    }
}