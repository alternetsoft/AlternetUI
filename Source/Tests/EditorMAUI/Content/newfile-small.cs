using System;

string[] arguments = Environment.GetCommandLineArgs();
Console.WriteLine("GetCommandLineArgs: {0}", string.Join(", ", arguments));

Console.WriteLine("Simple Properties");

// Create a new Person object:
(string Name, int Age) person = new("N/A", 0);

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

// Console.WriteLine("Enter your name: ");

// var name = System.Console.ReadLine();
// Console.WriteLine($"Your name is: {name}");

var result = Calc(5, 3);

Console.WriteLine($"{result}");


int Calc(int a, int b)
{
    var ab = a + b;
    var ab2 = a - b;
    return ab - ab2;
}


