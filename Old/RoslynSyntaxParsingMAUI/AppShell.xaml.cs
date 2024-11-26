namespace RoslynSyntaxParsing;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		if (Alternet.UI.App.IsDesktopOs)
		{
			Shell.SetTabBarIsVisible(this, false);
			Shell.SetNavBarIsVisible(this, false);
        }
	}
}

