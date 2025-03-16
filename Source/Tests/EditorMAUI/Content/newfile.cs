using System;

/// <summary>
/// This tutorial shows how properties are an integral part of the C# programming language.
/// It demonstrates how properties are declared and used.
/// </summary>
public class Person
{
    private string myName = "N/A";
    private int myAge = 0;

    public static void Main()
    {
        Console.WriteLine("Simple Properties");

        // Create a new Person object:
        Person person = new Person();

        // Print out the name and the age associated with the person:
        Console.WriteLine("Person details - {0}", person);

        // Set some values on the person object:
        person.Name = "Joe";
        person.Age = 99;
        Console.WriteLine("Person details - {0}", person);

        // Increment the Age property:
        person.Age += 1;
        Console.WriteLine("Name: {0}, Age: {1}", person.Name, person.Age);
        int i;   // Unused variable

        /* Console.WriteLine("Press Enter key to continue...");*/
        /* System.Console.Read(); */
    }

    /// <summary>
    /// Declare a Name property of type string:
    /// </summary>
    public string Name
    {
        get
        {
            return myName;
        }
        set
        {
            myName = value;
        }
    }

    /// <summary>
    /// Declare an Age property of type int:
    /// </summary>
    public int Age
    {
        get
        {
            return myAge;
        }
        set
        {
            myAge = value;
        }
    }

    public override string ToString()
    {
        return "Name = " + Name + ", Age = " + Age;
    } 
}