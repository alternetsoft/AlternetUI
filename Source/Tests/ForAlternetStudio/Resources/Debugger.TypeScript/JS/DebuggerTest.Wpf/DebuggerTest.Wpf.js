x = 10;

function debuggerTest()
{
    System.Console.WriteLine("Hello from JS 1");
    System.Console.WriteLine("Hello from JS 2");
    System.Console.WriteLine("Hello from JS 3");

    c = System.Console;

    i = 0;
    while (true)
    {
        i++;
        i += 2;
        x++;
    }
}

debuggerTest();