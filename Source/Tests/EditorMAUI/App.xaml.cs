namespace EditorMAUI;

public partial class App : Application
{
    public static Window? MainWindow;

    public App()
	{
		InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new NavigationPage(new AppShell()));
        MainWindow = window;

        window.Created += (s, e) =>
        {
        };

        window.Destroying += (s, e) =>
        {
            MainWindow = null;
        };

        window.Stopped += (s, e) =>
        {
        };

        window.Deactivated += (s, e) =>
        {
        };

        return window;
    }
}

