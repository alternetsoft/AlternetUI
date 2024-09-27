namespace EditorMAUI;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Created += (s, e) =>
        {
        };

        window.Destroying += Window_Destroying;

        window.Stopped += Window_Stopped;

        window.Deactivated += Window_Deactivated;

        return window;
    }

    private void Window_Deactivated(object? sender, EventArgs e)
    {
    }

    private void Window_Stopped(object? sender, EventArgs e)
    {
    }

    private void Window_Destroying(object? sender, EventArgs e)
    {
    }
}

