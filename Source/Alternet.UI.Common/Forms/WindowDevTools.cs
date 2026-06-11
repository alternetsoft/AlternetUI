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
    internal class WindowDevTools : Window
    {
        private static bool logGotFocus;

#pragma warning disable
        internal static bool ShowFocusedProperties;
        internal static bool LogFocusedControlInfo;
#pragma warning restore

        private readonly PanelDevTools panel = new()
        {
        };

        static WindowDevTools()
        {
        }

        public WindowDevTools()
        {
            panel.Margin = 10;
            HasSystemMenu = false;
            App.LogFileIsEnabled = true;
            StartLocation = WindowStartLocation.CenterScreen;
            Title = "Developer Tools";
            Size = (900, 700);
            panel.Parent = this;
            Icon = App.MainWindow?.Icon ?? KnownIcons.Default;

            ComponentDesigner.SafeDefault.ControlGotFocus += OnDesignerControlGotFocus;
            ComponentDesigner.SafeDefault.ControlCreated += OnDesignerControlCreated;
            ComponentDesigner.SafeDefault.ControlDisposed += OnDesignerControlDisposed;
            ComponentDesigner.SafeDefault.ControlParentChanged += OnDesignerControlParentChanged;

            panel.CenterNotebook.SelectedIndex = 0;
            panel.RightNotebook.SelectedIndex = 0;
            panel.PropGrid.SuggestedInitDefaults();
            StatusBar = new StatusBar();
        }

        public PanelDevTools DevPanel => panel;

        internal static bool LogGotFocus
        {
            get => logGotFocus;

            set
            {
                logGotFocus = value;
                AbstractControl.ShowDebugFocusRect = value;
            }
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            ComponentDesigner.SafeDefault.ControlGotFocus -= OnDesignerControlGotFocus;
            ComponentDesigner.SafeDefault.ControlCreated -= OnDesignerControlCreated;
            ComponentDesigner.SafeDefault.ControlDisposed -= OnDesignerControlDisposed;
            ComponentDesigner.SafeDefault.ControlParentChanged -= OnDesignerControlParentChanged;
        }

        private void OnDesignerControlDisposed(object? sender, EventArgs e)
        {
            if (panel.LastFocusedControl == sender)
                panel.LastFocusedControl = null;

            var propGridControl = panel.PropGrid.FirstItemInstance;
            if(sender == propGridControl)
                panel.PropGrid.Clear();
        }

        private void OnDesignerControlParentChanged(object? sender, EventArgs e)
        {
        }

        private void OnDesignerControlCreated(object? sender, EventArgs e)
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
            if (control.ParentWindow is WindowDevTools)
                return true;
            return false;
        }

        private void OnDesignerControlGotFocus(object? sender, EventArgs e)
        {
            if (sender is not AbstractControl control)
                return;
            if (control.Parent is null || IgnoreControl(control))
                return;

            panel.LastFocusedControl = control;

            if (LogGotFocus)
            {
                var s = control.ParentWindow?.Title ?? control.ParentWindow?.GetType().Name;
                var prefix = "FocusedControl:";
                App.LogReplace($"{prefix} <{s}>.<{control.GetType().FullName}>", prefix);
            }

            if (LogFocusedControlInfo)
                LogFocusedControl(control);
        }

        private void LogFocusedControl(AbstractControl control, bool details = false)
        {
            var defaultColors = control.GetDefaultFontAndColor();

            var windowTitle = control.ParentWindow?.Title ?? control.ParentWindow?.GetType().Name;

            if (details)
            {
                App.LogBeginSection("Focused Control");
                App.LogNameValue("Name", control.Name);
                App.LogNameValue("Type", control.GetType().FullName);
                App.LogNameValue("Window", windowTitle);
                LogUtils.LogColor("ForegroundColor", control.ForegroundColor);
                LogUtils.LogColor("ForegroundColor (real)", control.RealForegroundColor);
                LogUtils.LogColor("ForegroundColor (defaults)", defaultColors.ForegroundColor);

                LogUtils.LogColor("BackgroundColor", control.BackgroundColor);
                LogUtils.LogColor("BackgroundColor (real)", control.RealBackgroundColor);
                LogUtils.LogColor("BackgroundColor (defaults)", defaultColors.BackgroundColor);

                App.LogNameValue("PixelScaleFactor", control.ScaleFactor);
                App.LogNameValue("PixelToDip(100)", control.PixelToDip(100));
                App.LogNameValue("DPI", control.GetDPI());
                App.LogEndSection();
            }
            else
            {
                App.Log($"FocusedControl: {control.GetType().FullName}");
            }

        }
    }
}
