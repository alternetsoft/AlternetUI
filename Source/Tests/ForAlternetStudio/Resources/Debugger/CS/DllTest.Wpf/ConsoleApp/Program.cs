using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassLibrary.HelloWorld helloWorld = new ClassLibrary.HelloWorld();
            string line = string.Empty;
            do
            {
                line = Console.ReadLine();

                helloWorld.Display(line);
            }
            while (line != "exit");
        }
    }
}
