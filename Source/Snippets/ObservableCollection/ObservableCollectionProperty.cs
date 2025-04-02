using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

public class Item
{
    public string Name { get; set; }
}

public class ItemManager
{
    public ObservableCollection<Item> Items { get; private set; }

    public ItemManager()
    {
        Items = new ObservableCollection<Item>();
        Items.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Console.WriteLine("Collection Updated!");

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (Item newItem in e.NewItems)
            {
                Console.WriteLine($"Added: {newItem.Name}");
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (Item oldItem in e.OldItems)
            {
                Console.WriteLine($"Removed: {oldItem.Name}");
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Replace)
        {
            Console.WriteLine("Items replaced.");
        }
    }
}

class Program
{
    static void Main()
    {
        ItemManager manager = new ItemManager();

        manager.Items.Add(new Item { Name = "Item 1" });
        manager.Items.Add(new Item { Name = "Item 2" });
        manager.Items.RemoveAt(0);
    }
}
