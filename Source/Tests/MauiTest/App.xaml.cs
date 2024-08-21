namespace SpinPaint;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

        Alternet.UI.App.LogMessage += App_LogMessage;

	}

    private void App_LogMessage(object? sender, Alternet.UI.LogMessageEventArgs e)
    {
        if(e.Message is not null)
            LogMessage?.Invoke(this, e.Message);
    }

    public static event EventHandler<string>? LogMessage;

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

    public void Log(string s)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LogMessage?.Invoke(null, s);
        });
    }
}

