using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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

            ComponentDesigner.SafeDefault.ControlGotFocus += Designer_ControlGotFocus;
            ComponentDesigner.SafeDefault.ControlCreated += Designer_ControlCreated;
            ComponentDesigner.SafeDefault.ControlDisposed += Designer_ControlDisposed;
            ComponentDesigner.SafeDefault.ControlParentChanged += Designer_ControlParentChanged;

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
            base.DisposeManagedResources();
            ComponentDesigner.SafeDefault.ControlGotFocus -= Designer_ControlGotFocus;
            ComponentDesigner.SafeDefault.ControlCreated -= Designer_ControlCreated;
            ComponentDesigner.SafeDefault.ControlDisposed -= Designer_ControlDisposed;
            ComponentDesigner.SafeDefault.ControlParentChanged -= Designer_ControlParentChanged;
        }

        private void Designer_ControlDisposed(object? sender, EventArgs e)
        {
            if (panel.LastFocusedControl == sender)
                panel.LastFocusedControl = null;

            var propGridControl = panel.PropGrid.FirstItemInstance;
            if(sender == propGridControl)
                panel.PropGrid.Clear();
        }

        private void Designer_ControlParentChanged(object? sender, EventArgs e)
        {
        }

        private void Designer_ControlCreated(object? sender, EventArgs e)
        {
            if (sender is not Control control)
                return;
            if (IgnoreControl(control))
                return;

            var type = sender?.GetType();
            if (type is null)
                return;
            var events = AssemblyUtils.EnumEvents(type, true);

            foreach (var item in events)
            {
                var handler = CodeGeneratorUtils.GetEventLogDelegate(item);
                if(handler is not null)
                    item.AddEventHandler(sender, handler);
            }
        }

        private bool IgnoreControl(Control control)
        {
            if (control.ParentWindow is WindowDeveloperTools)
                return true;
            return false;
        }

        private void Designer_ControlGotFocus(object? sender, EventArgs e)
        {
            if (sender is not Control control)
                return;
            if (IgnoreControl(control))
                return;
            panel.LastFocusedControl = control;

            if (logGotFocus)
                Application.Log(control.GetType().Name);

            if (logFocusedControl)
                LogFocusedControl(control);
        }

        private void LogFocusedControl(Control control)
        {
            var defaultColors = control.GetDefaultFontAndColor();

            Application.LogSeparator();
            Application.LogNameValue("Name", control.Name);
            Application.LogNameValue("Type", control.GetType().Name);
            LogUtils.LogColor("ForegroundColor", control.ForegroundColor);
            LogUtils.LogColor("ForegroundColor (real)", control.RealForegroundColor);
            LogUtils.LogColor("ForegroundColor (defaults)", defaultColors.ForegroundColor);

            LogUtils.LogColor("BackgroundColor", control.BackgroundColor);
            LogUtils.LogColor("BackgroundColor (real)", control.RealBackgroundColor);
            LogUtils.LogColor("BackgroundColor (defaults)", defaultColors.BackgroundColor);

            Application.LogNameValue("PixelScaleFactor", control.GetPixelScaleFactor());
            Application.LogNameValue("PixelToDip(100)", control.PixelToDip(100));
            Application.LogNameValue("DPI", control.GetDPI());
            Application.LogSeparator();
        }
    }
}
