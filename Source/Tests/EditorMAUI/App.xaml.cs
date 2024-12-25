namespace EditorMAUI;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new NavigationPage(new AppShell()));

        window.Created += (s, e) =>
        {
        };

        window.Destroying += (s, e) =>
        {
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

