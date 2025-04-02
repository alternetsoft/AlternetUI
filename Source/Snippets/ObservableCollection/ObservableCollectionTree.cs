using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

public class Item
{
    public string Name { get; set; }
    public string Text { get; set; }
    public Item Parent { get; internal set; } // Linked to Parent Item
    public ObservableCollection<Item> Children { get; private set; } // Child Items

    // Parameterless constructor
    public Item()
    {
        Name = "Unnamed";
        Text = "";
        Children = new ObservableCollection<Item>();
        Children.CollectionChanged += OnCollectionChanged;
    }

    // Constructor with parameters
    public Item(string name, string text) : this()
    {
        Name = name;
        Text = text;
    }

    public void Add(string name, string text)
    {
        var newItem = new Item(name, text) { Parent = this };
        Children.Add(newItem);
        Console.WriteLine($"Added: {newItem.Name} with Text: \"{newItem.Text}\" under {Name}");
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Console.WriteLine($"Children of {Name} Updated!");

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (Item newItem in e.NewItems)
            {
                newItem.Parent = this;
                Console.WriteLine($"Added: {newItem.Name}, Parent set.");
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (Item oldItem in e.OldItems)
            {
                oldItem.Parent = null;
                Console.WriteLine($"Removed: {oldItem.Name}, Parent cleared.");
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Replace)
        {
            foreach (Item oldItem in e.OldItems)
            {
                oldItem.Parent = null;
            }
            foreach (Item newItem in e.NewItems)
            {
                newItem.Parent = this;
            }
            Console.WriteLine($"Items replaced under {Name}, Parent updated.");
        }
    }
}

class Program
{
    static void Main()
    {
        Item root = new Item("Root Item", "Root Text");

        // Using the parameterless constructor
        Item defaultItem = new Item();
        Console.WriteLine($"Default Item: Name = \"{defaultItem.Name}\", Text = \"{defaultItem.Text}\"");

        // Adding child items
        root.Add("Child 1", "This is Child 1");
        root.Add("Child 2", "This is Child 2");

        Console.WriteLine($"Child 1 Parent: {root.Children[0].Parent.Name}, Text: {root.Children[0].Text}");

        root.Children.Remove(root.Children[0]);
        Console.WriteLine($"Child 1 Parent after removal: {root.Children[0].Parent == null}");
    }
}
