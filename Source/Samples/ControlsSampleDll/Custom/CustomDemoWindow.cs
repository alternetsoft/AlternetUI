using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alternet.UI
{
    public class CustomDemoWindow : Window
    {
        public static bool UseLogListBox = true;
        public static bool AddBuildNumber = true;
        public static bool LogFocusedControl = false;

        public static bool BindConsole = false;

        private SplittedTreeAndCards? pageContainer;
        private readonly LogListBox? eventsControl;
        private readonly LayoutPanel splitterPanel;
        private readonly Panel panel;
        private readonly Splitter splitter = new();

        static CustomDemoWindow()
        {
            UixmlLoader.LoadFromResName = FormLoadFromResName;
            UixmlLoader.LoadFromStream = FormLoadFromStream;
            UixmlLoader.ReportLoadException = ReportFormLoadException;

            UseDebugBackgroundColor = false;

            ResourceLoader.CustomStreamFromUrl += ResourceLoader_CustomStreamFromUrl;
            ResourceLoader.CustomStreamFromUrl += ResourceLoader_CustomStreamFromUrl2;

            LogUtils.RegisterLogAction("Setup Main Form for Screenshot", () =>
            {
                var window = App.FindWindow<CustomDemoWindow>();
                if (window is null)
                    return;

                window.eventsControl?.SetVisible(false);
                window.splitter.Visible = false;
                window.Height = (window.Width / 3) * 2;
                AddBuildNumber = false;
                window.UpdateTitle();
            });
        }

        public CustomDemoWindow()
        {
            if (DebugUtils.DebugLoading)
            {
                LogUtils.LogToFile("CustomDemoWindow constructor");
            }

            SuppressEsc = true;

            if (UseLogListBox)
            {
                eventsControl = new()
                {
                    Dock = DockStyle.Bottom,
                    HasBorder = false,
                    Margin = (0, 10, 0, 0),
                };
            }

            splitterPanel = new()
            {
            };

            panel = new()
            {
                Padding = 5,
            };

            if(eventsControl is not null)
            {
                eventsControl.ShowDebugWelcomeMessage = true;
                eventsControl.BindApplicationLog();
            }

            if (BindConsole)
            {
                ConsoleUtils.BindConsoleOutput();
                ConsoleUtils.BindConsoleError();
            }

            DoInsideLayout(Initialize);

            if (DebugUtils.DebugLoading)
            {
                LogUtils.LogToFile("CustomDemoWindow constructor done");
            }

            MinimumSize = (800, 600);

            StartLocation = WindowStartLocation.CenterScreen;

            App.AddIdleTask(() =>
            {
                pageContainer!.SelectedIndex = 0;
            });
        }

        public static bool FormLoadFromResName(string resName, object obj, UixmlLoader.Flags flags)
        {
            return false;
        }

        public static bool FormLoadFromStream(
            Stream stream,
            object obj,
            string? resName,
            UixmlLoader.Flags flags)
        {
            return false;
        }

        public static bool ReportFormLoadException(
            Exception e,
            string? resName,
            UixmlLoader.Flags flags)
        {
            return false;
        }

        internal void SetDebugColors()
        {
            DebugBackgroundColor(Color.Red, nameof(CustomDemoWindow));
            pageContainer?.SetDebugColors();
        }

        public void AddPage(string title, Func<AbstractControl> action)
        {
            if (DebugUtils.DebugLoading)
            {
                LogUtils.LogToFile($"AddPage {title}");
            }

            pageContainer?.Add(title, action);
        }

        public void UpdateTitle()
        {
            Title = $"AlterNET UI Demo";

            if (AddBuildNumber)
            {
                Title = $"{Title} {SystemSettings.Handler.GetUIVersion()}";
            }
        }

        public virtual void Initialize()
        {
            pageContainer = new(SplittedTreeAndCards.TreeKind.ListBox);
            panel.Parent = this;

            UpdateTitle();

            Size = (900, 700);
            StartLocation = WindowStartLocation.CenterScreen;

            Icon = IconSet.FromUrlOrDefault("embres:ControlsSampleDll.Sample.ico", App.DefaultIcon);

            AddPages();

            splitterPanel.Parent = panel;
            pageContainer.Dock = DockStyle.Fill;
            splitter.Dock = DockStyle.Bottom;
            pageContainer.Parent = splitterPanel;
            splitter.Parent = splitterPanel;

            if (eventsControl is not null)
            {
                eventsControl.Height = 150;
                eventsControl.Parent = splitterPanel;
            }

            if(pageContainer.LeftControlKind == SplittedTreeAndCards.TreeKind.ListBox)
            {
                pageContainer.ListBox!.HorizontalScrollbar = true;
            }

            var logSizeChanged = false;

            if (logSizeChanged)
            {
                SizeChanged += MainWindow_SizeChanged;
                StateChanged += MainWindow_StateChanged;
            }

            ActiveControl = pageContainer.LeftControl;
        }

        protected virtual void AddPages()
        {
        }

        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            App.Log($"State changed: {Size}, {this.State}");
        }

        private void MainWindow_SizeChanged(object? sender, EventArgs e)
        {
            App.Log($"Size changed: {Size}, {this.State}");
        }

        protected TabControl CreateCustomPage(NameValue<Func<AbstractControl>>?[] pages)
        {
            TabControl result = new()
            {
                Margin = 10,
            };

            result.ContentPadding = 5;

            result.AddRange(pages);
            return result;
        }

        [Browsable(false)]
        public string? LastEventMessage => eventsControl?.LastLogMessage;

        public void LogEventSmart(string? message, string? prefix)
        {
            eventsControl?.LogReplace(message, prefix);
        }

        public void LogEvent(string? message)
        {
            eventsControl?.Log(message);
        }

        private static void ResourceLoader_CustomStreamFromUrl(object? sender, StreamFromUrlEventArgs e)
        {
            if (e.Value is null)
                return;

            e.Handled = true;
            e.Result = ResourceLoader.DefaultStreamFromUrl(e.Value);
        }

        private static void ResourceLoader_CustomStreamFromUrl2(object? sender, StreamFromUrlEventArgs e)
        {
            App.Log("CustomStreamFromUrl 2");
        }
    }
}
