#load "myclass.csx"

using System;

MyClass myQueueItem;

Run("test", out myQueueItem);

public static void Run(string testStr, out MyClass myQueueItem)
{
    Console.WriteLine($"C# Blob trigger function processed: {testStr}");
    myQueueItem = new MyClass() { Id = "myid" };
    Console.WriteLine(myQueueItem.Id);
    Console.ReadLine();
}
