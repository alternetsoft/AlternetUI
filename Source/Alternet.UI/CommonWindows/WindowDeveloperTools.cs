using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Developer tools window with additional debug related features.
    /// </summary>
    internal class WindowDeveloperTools : Window
    {
        private readonly PanelDeveloperTools panel = new()
        {
            SuggestedSize = new(800, 600),
        };

        private bool logGotFocus;
        private bool logFocusedControl;

        public WindowDeveloperTools()
            : base()
        {
            StartLocation = WindowStartLocation.CenterScreen;
            Title = "Developer Tools";
            panel.Parent = this;
            panel.Update();
            SetSizeToContent();

            ComponentDesigner.InitDefault();
            ComponentDesigner.Default!.ControlGotFocus += Designer_ControlGotFocus;

            panel.AddAction("Toggle GotFocus logging", () =>
            {
                logGotFocus = !logGotFocus;
                if(logGotFocus)
                    Application.Log("GotFocus event logging enabled");
                else
                    Application.Log("GotFocus event logging disabled");
            });

            panel.AddAction("Toggle Focused Info", () =>
            {
                logFocusedControl = !logFocusedControl;

                if (logGotFocus)
                    Application.Log("Focused control info enabled");
                else
                    Application.Log("Focused control info disabled");
            });
        }

        protected override void DisposeManagedResources()
        {
            ComponentDesigner.Default!.ControlGotFocus -= Designer_ControlGotFocus;
        }

        private void Designer_ControlGotFocus(object? sender, EventArgs e)
        {
            var focusedControl = Control.GetFocusedControl();
            if (focusedControl is null)
                return;
            var parentWindow = focusedControl.ParentWindow;
            if (parentWindow is null || parentWindow is WindowDeveloperTools)
                return;

            if(logGotFocus)
                Application.Log(focusedControl.GetType().Name);

            if (logFocusedControl)
                LogFocusedControl(focusedControl);
        }

        private void LogFocusedControl(Control control)
        {
            var defaultColors = control.GetDefaultFontAndColor();

            Application.LogSeparator();
            Application.Log($"Name = {control.Name}");
            Application.Log($"Type = {control.GetType().Name}");
            LogUtils.LogColor("ForegroundColor", control.ForegroundColor);
            LogUtils.LogColor("ForegroundColor (real)", control.RealForegroundColor);
            LogUtils.LogColor("ForegroundColor (defaults)", defaultColors.ForegroundColor);

            LogUtils.LogColor("BackgroundColor", control.BackgroundColor);
            LogUtils.LogColor("BackgroundColor (real)", control.RealBackgroundColor);
            LogUtils.LogColor("BackgroundColor (defaults)", defaultColors.BackgroundColor);

            Application.LogSeparator();
        }
    }
}
