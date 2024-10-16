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
    internal class DeveloperToolsWindow : Window
    {
        private static bool logGotFocus;
        private static bool logFocusedControl;

        private readonly DeveloperToolsPanel panel = new()
        {
            SuggestedSize = new(900, 700),
        };

        static DeveloperToolsWindow()
        {
            LogUtils.RegisterLogAction("Toggle GotFocus event logging", () =>
            {
                logGotFocus = !logGotFocus;
                App.LogNameValue("GotFocus event logging", logGotFocus);
            });

            LogUtils.RegisterLogAction("Toggle Focused Info", () =>
            {
                logFocusedControl = !logFocusedControl;
                App.LogNameValue("Focused control info", logFocusedControl);
            });
        }

        public DeveloperToolsWindow()
        {
            App.LogFileIsEnabled = true;
            StartLocation = WindowStartLocation.CenterScreen;
            Title = "Developer Tools";
            panel.Parent = this;
            panel.Update();
            SetSizeToContent();

            ComponentDesigner.SafeDefault.ControlGotFocus += Designer_ControlGotFocus;
            ComponentDesigner.SafeDefault.ControlCreated += Designer_ControlCreated;
            ComponentDesigner.SafeDefault.ControlDisposed += Designer_ControlDisposed;
            ComponentDesigner.SafeDefault.ControlParentChanged += Designer_ControlParentChanged;

            panel.CenterNotebook.SelectedIndex = 0;
            panel.RightNotebook.SelectedIndex = 0;
            panel.PropGrid.SuggestedInitDefaults();
        }

        public DeveloperToolsPanel DevPanel => panel;

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
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
            if (sender is not AbstractControl control)
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

        private bool IgnoreControl(AbstractControl control)
        {
            if (control.ParentWindow is DeveloperToolsWindow)
                return true;
            return false;
        }

        private void Designer_ControlGotFocus(object? sender, EventArgs e)
        {
            if (sender is not AbstractControl control)
                return;
            if (control.Parent is null || IgnoreControl(control))
                return;

            panel.LastFocusedControl = control;

            if (logGotFocus)
                App.Log(control.GetType().Name);

            if (logFocusedControl)
                LogFocusedControl(control);
        }

        private void LogFocusedControl(AbstractControl control)
        {
            var defaultColors = control.GetDefaultFontAndColor();

            App.LogSeparator();
            App.LogNameValue("Name", control.Name);
            App.LogNameValue("Type", control.GetType().Name);
            LogUtils.LogColor("ForegroundColor", control.ForegroundColor);
            LogUtils.LogColor("ForegroundColor (real)", control.RealForegroundColor);
            LogUtils.LogColor("ForegroundColor (defaults)", defaultColors.ForegroundColor);

            LogUtils.LogColor("BackgroundColor", control.BackgroundColor);
            LogUtils.LogColor("BackgroundColor (real)", control.RealBackgroundColor);
            LogUtils.LogColor("BackgroundColor (defaults)", defaultColors.BackgroundColor);

            App.LogNameValue("PixelScaleFactor", control.ScaleFactor);
            App.LogNameValue("PixelToDip(100)", control.PixelToDip(100));
            App.LogNameValue("DPI", control.GetDPI());
            App.LogSeparator();
        }
    }
}
