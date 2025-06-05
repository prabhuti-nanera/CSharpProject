using System;
using System.Collections.Generic;

public class EmployeeManager
{
    private List<Employee> employees = new List<Employee>();

    public void AddEmployee(Employee employee)
    {
        employees.Add(employee);
        Console.WriteLine($"Employee added: {employee.Name}");
    }

    public void DisplayEmployees()
    {
        foreach (var emp in employees)
        {
            emp.Display();
        }
    }
}
