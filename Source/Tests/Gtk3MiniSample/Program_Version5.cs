using Gtk;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();

        // Create a new window
        var window = new Window("GtkSharp Gtk3 Sample");
        window.SetDefaultSize(400, 200);
        window.DeleteEvent += (o, e) => Application.Quit();

        // Add a label
        var label = new Label("Hello from GtkSharp Gtk3!");
        window.Add(label);

        window.ShowAll();
        Application.Run();
    }
}