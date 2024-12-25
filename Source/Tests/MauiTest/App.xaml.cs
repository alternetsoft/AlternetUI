namespace SpinPaint;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Alternet.UI.App.LogMessage += (s, e) =>
        {
            if (e.Message is not null)
                Log(e.Message);
        };
    }

    public static event EventHandler<string>? LogMessage;

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

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

    public void Log(string s)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LogMessage?.Invoke(null, s);
        });
    }
}

