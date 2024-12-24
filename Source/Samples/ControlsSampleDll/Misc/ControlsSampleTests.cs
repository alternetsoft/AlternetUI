using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace ControlsSample
{
    public static class ControlsSampleTests
    {
        public static void LogManifestResourceNames(Assembly? assembly = null)
        {
            assembly ??= typeof(ControlsSampleTests).Assembly;

            var resources = assembly?.GetManifestResourceNames();

            if (resources is null)
                return;

            foreach (var item in resources)
            {
                LogUtils.LogToFile(item);
            }
        }

        [Description("Show WindowTextInput dialog...")]
        public static void TestWindowTextInput()
        {
            var window = new WindowTextInput();
            window.ShowDialogAsync();
        }

        [Description("Add input binding [Ctrl+Shift+Z]")]
        public static void TestInputBinding()
        {
            var window = Window.Default;
            KeyGesture gesture = new(Key.Z, ModifierKeys.ControlShift);
            Command command = new();
            command.AfterExecute += (param) =>
            {
                App.Log($"Binding is called: {param}");
            };
            InputBinding binding = new(command, gesture);
            binding.CommandParameter = "parameter1";
            window.InputBindings.Add(binding);

            App.Log($"Registered binding to form: {window.Name ?? window.GetType().ToString()}");
        }

        public static void TestActivateEvents()
        {
            var window = App.FindWindow<MainWindow>();
            if (window is null)
                return;
            window.Activated -= Window_Activated;
            window.Activated += Window_Activated;
            window.Deactivated -= Window_Deactivated;
            window.Deactivated += Window_Deactivated;

            var ch = window.GetChildren<LogListBox>(true);
            var control = ch.First;

            if (control is null)
                return;
            control.Activated -= Control_Activated;
            control.Activated += Control_Activated;
            control.Deactivated -= Control_Deactivated;
            control.Deactivated += Control_Deactivated;
        }

        private static void Control_Deactivated(object sender, EventArgs e)
        {
            App.Log("Control is Deactivated");
        }

        private static void Window_Deactivated(object sender, EventArgs e)
        {
            App.Log("Window is Deactivated");
        }

        private static void Control_Activated(object sender, EventArgs e)
        {
            App.Log("Control is Activated");
        }

        private static void Window_Activated(object sender, EventArgs e)
        {
            App.Log("Window is Activated");
        }
    }
}
