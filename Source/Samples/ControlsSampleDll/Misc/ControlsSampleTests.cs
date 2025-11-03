using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace ControlsSample
{
    public static class ControlsSampleTests
    {
        public static int TestTimerInterval = 30;

        public static System.Timers.Timer TestSystemTimer = new ();
        public static System.Threading.Timer? TestThreadingTimer;
        public static Timer TestTimer = new();
        public static int TestCounter = 0;
        public static DateTime TestStartTime;
        public static List<string> TestTimerLog = new();

        public static void TestTimersInit(string timerKind, int interval)
        {
            TestCounter = 0;
            App.Log($"Test {timerKind} timer: started with {interval} ms interval");
            TestStartTime = DateTime.Now;
            TestTimerLog.Clear();
        }

        public static void TestThreadingTimerXX()
        {
            System.Threading.TimerCallback callback = state =>
            {
                TimerTickAction();
            };

            TestTimersInit("ThreadingTimer", TestTimerInterval);
            TestThreadingTimer = new (
                callback,
                null,
                dueTime: TestTimerInterval,
                period: TestTimerInterval);
        }

        public static void TestMacOsSpecific()
        {
            if (App.Handler is not IMacOsApplicationHandler macOsApp)
            {
                App.Log("MacOS specific application info is not available on this platform.");
                return;
            }

            App.LogBeginSection("MacOS specific application info");

            App.LogNameValue("HelpMenuTitleName", macOsApp.HelpMenuTitleName);
            App.LogNameValue("Window", macOsApp.WindowMenuTitleName);

            App.LogEndSection();
        }

        public static void TestTimerXX()
        {
            TestTimerWithInterval(TestTimerInterval);
        }

        public static void TestSystemTimerXX()
        {
            TestSystemTimerWithInterval(TestTimerInterval);
        }

        private static void TimerTickAction(object? sender, EventArgs e)
        {
            TimerTickAction();
        }

        private static void TimerTickAction()
        {
            var now = DateTime.Now;
            var elapsed = (now - TestStartTime).TotalMilliseconds;
            TestStartTime = now;
            Output($"Timer tick: {TestCounter}, elapsed from last tick: {elapsed} ms");

            if (TestCounter < 30)
            {
                TestCounter++;
            }
            else
            {
                TestTimer.Stop();
                TestSystemTimer.Stop();
                TestThreadingTimer?.Dispose();
                TestThreadingTimer = null;
                Output("Timer stopped after 30 ticks.");
                foreach (var item in TestTimerLog)
                {
                    App.Log(item);
                }
            }

            void Output(string message)
            {
                TestTimerLog.Add(message);
            }
        }

        public static void TestSystemTimerWithInterval(int interval)
        {
            TestTimersInit("system", interval);
            TestSystemTimer.Interval = interval;
            TestSystemTimer.Elapsed -= TimerTickAction;
            TestSystemTimer.Elapsed += TimerTickAction;
            TestSystemTimer.Start();
        }

        public static void TestTimerWithInterval(int interval)
        {
            TestTimersInit("ui", interval);
            TestTimer.Interval = interval;
            TestTimer.TickAction = TimerTickAction;
            TestTimer.Start();
        }

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

        private static void Control_Deactivated(object? sender, EventArgs e)
        {
            App.Log("Control is Deactivated");
        }

        private static void Window_Deactivated(object? sender, EventArgs e)
        {
            App.Log("Window is Deactivated");
        }

        private static void Control_Activated(object? sender, EventArgs e)
        {
            App.Log("Control is Activated");
        }

        private static void Window_Activated(object? sender, EventArgs e)
        {
            App.Log("Window is Activated");
        }
    }
}
