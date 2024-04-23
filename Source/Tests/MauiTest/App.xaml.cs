using SharpHook;

namespace SpinPaint;

public partial class App : Application
{
    TaskPoolGlobalHook? hook;
    
    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

	}

    public static event EventHandler<string>? LogMessage;

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = base.CreateWindow(activationState);

        hook = new TaskPoolGlobalHook();

        hook.KeyTyped += Hook_KeyTyped;
        hook.KeyPressed += Hook_KeyPressed;
        hook.KeyReleased += Hook_KeyReleased;

        hook.MouseClicked += Hook_MouseClicked;
        hook.MousePressed += Hook_MousePressed;
        hook.MouseReleased += Hook_MouseReleased;
        hook.MouseMoved += Hook_MouseMoved;
        hook.MouseDragged += Hook_MouseDragged;

        hook.MouseWheel += Hook_MouseWheel;

        hook.RunAsync();

        window.Created += (s, e) =>
        {
            // Custom logic
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
        hook?.Dispose();
        hook = null;
    }

    private void Window_Destroying(object? sender, EventArgs e)
    {
        hook?.Dispose();
        hook = null;
    }

    public void Log(string s)
    {
        if (hook is null)
            return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            LogMessage?.Invoke(null, s);
        });
    }

    private void Hook_KeyTyped(object? sender, KeyboardHookEventArgs e)
    {
        Log("KeyTyped");
    }

    private void Hook_KeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        Log("KeyPressed");
    }

    private void Hook_KeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        Log("KeyReleased");
    }

    private void Hook_MouseClicked(object? sender, MouseHookEventArgs e)
    {
        Log("MouseClicked");
    }

    private void Hook_MousePressed(object? sender, MouseHookEventArgs e)
    {
        Log("MousePressed");
    }

    private void Hook_MouseReleased(object? sender, MouseHookEventArgs e)
    {
        Log("MouseReleased");
    }

    private void Hook_MouseMoved(object? sender, MouseHookEventArgs e)
    {
    }

    private void Hook_MouseDragged(object? sender, MouseHookEventArgs e)
    {
        Log("MouseDragged");
    }

    private void Hook_MouseWheel(object? sender, MouseWheelHookEventArgs e)
    {
        Log("MouseWheel");
    }
}

