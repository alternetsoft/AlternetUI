using System;
using System.ComponentModel;
using System.IO;

using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public partial class MainWindow : Window
    {
        public static bool LogFocusedControl = false;

        private SplittedTreeAndCards? pageContainer;
        private readonly LogListBox eventsControl;
        private readonly LayoutPanel splitterPanel;
        private readonly Control panel;
        private readonly Splitter splitter = new();

        static MainWindow()
        {
            UixmlLoader.LoadFromResName = FormLoadFromResName;
            UixmlLoader.LoadFromStream = FormLoadFromStream;
            UixmlLoader.ReportLoadException = ReportFormLoadException;

            UseDebugBackgroundColor = false;

            ResourceLoader.CustomStreamFromUrl += ResourceLoader_CustomStreamFromUrl;
            ResourceLoader.CustomStreamFromUrl += ResourceLoader_CustomStreamFromUrl2;

            Control.FocusedControlChanged += (s, e) =>
            {
                if(LogFocusedControl)
                    App.LogReplace($"FocusedControl: {FocusedControl?.GetType()}", "FocusedControl:");
            };

            LogUtils.RegisterLogAction("Toggle LogFocusedControl", () =>
            {
                LogFocusedControl = !LogFocusedControl;
            });
        }

        public MainWindow()
        {
            SupressEsc = true;

            eventsControl = new()
            {
                Dock = DockStyle.Bottom,
                HasBorder = false,
                Margin = (0, 10, 0, 0),
            };

            splitterPanel = new()
            {
            };

            panel = new()
            {
                Padding = 5,
            };

            eventsControl.BindApplicationLog();
            ConsoleUtils.BindConsoleOutput();
            ConsoleUtils.BindConsoleError();
            DoInsideLayout(Initialize);
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

        public static bool ReportFormLoadException(Exception e, string? resName, UixmlLoader.Flags flags)
        {
            return false;
        }

        internal void SetDebugColors()
        {
            DebugBackgroundColor(Color.Red, nameof(MainWindow));
            pageContainer?.SetDebugColors();
        }

        public void Initialize()
        {
            pageContainer = new(SplittedTreeAndCards.TreeKind.ListBox);
            panel.Parent = this;
            Title = "AlterNET UI Demo";
            Size = (900, 700);
            StartLocation = WindowStartLocation.CenterScreen;

            Icon = new("embres:ControlsSampleDll.Sample.ico");

            void AddPage(string title, Func<Control> action)
            {
                pageContainer.Add(title, action);
            }

            AddPage("Welcome", CreateWelcomePage);
            AddPage("Text", CreateTextInputPage);
            AddPage("ListBoxes", CreateListControlsPage);
            AddPage("Buttons", CreateButtonsPage);
            AddPage("TreeView", CreateTreeViewPage);
            AddPage("ListView", CreateListViewPage);
            AddPage("DateTime", CreateDateTimePage);
            AddPage("WebBrowser", CreateWebBrowserPage);
            AddPage("Number", CreateNumericInputPage);
            AddPage("Slider and Progress", CreateSliderAndProgressPage);
            AddPage("Layout", CreateLayoutPage);
            AddPage("Notify and ToolTip", CreateNotifyIconPage);
            AddPage("TabControl", CreateTabControlPage);
            AddPage("Multimedia", CreateMultimediaPage);
            AddPage("Samples", CreateOtherPage);

            LogUtils.DebugLogVersion();

            splitterPanel.Parent = panel;
            pageContainer.Dock = DockStyle.Fill;
            splitter.Dock = DockStyle.Bottom;
            pageContainer.Parent = splitterPanel;
            splitter.Parent = splitterPanel;
            eventsControl.Height = 150;
            eventsControl.Parent = splitterPanel;

            pageContainer.SelectedIndex = 0;
            pageContainer.ListBox!.HScrollBarVisible = true;
            pageContainer.LeftControl?.SetFocusIfPossible();

            var logSizeChanged = false;

            if (logSizeChanged)
            {
                SizeChanged += MainWindow_SizeChanged;
                StateChanged += MainWindow_StateChanged;
            }
        }

        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            App.Log($"State changed: {Size}, {this.State}");
        }

        private void MainWindow_SizeChanged(object? sender, EventArgs e)
        {
            App.Log($"Size changed: {Size}, {this.State}");
        }

        Control CreateCustomPage(NameValue<Func<Control>>?[] pages)
        {
            TabControl result = new()
            {
                Margin = 10,
            };

            result.ContentPadding = 5;

            result.AddRange(pages);
            return result;
        }

        Control CreateListControlsPage()
        {
            NameValue<Func<Control>>? popupNameValue;

            popupNameValue = new("Popup", () => new ListControlsPopups());

            NameValue<Func<Control>>?[] pages =
            {
                new("List", () => new ListBoxPage()),
                new("Checks", () => new CheckListBoxPage()),
                new("Combo", () => new ComboBoxPage()),
                new("Virtual", () => new VListBoxSamplePage()),
                new("Colors", () => new ColorListBoxSamplePage()),

                popupNameValue,
            };

            return CreateCustomPage(pages);
        }

        Control CreateButtonsPage()
        {
            NameValue<Func<Control>>[] pages =
            {
                new("Button", () => new ButtonPage()),
                new("Check", () => new CheckBoxesPage()),
                new("Radio", () => new RadioButtonsPage()),
            };

            return CreateCustomPage(pages);
        }

        Control CreateSliderAndProgressPage()
        {
            NameValue<Func<Control>>[] pages =
            {
                new("Slider", () => new SliderPage()),
                new("Progress", () => new ProgressBarPage()),
            };

            return CreateCustomPage(pages);
        }

        Control CreateOtherPage()
        {
            NameValue<Func<Control>>[] pages =
            {
                new("Internal", CreateInternalSamplesPage),
                /*new("External", CreateAllSamplesPage),*/
            };

            return CreateCustomPage(pages);
        }

        Control CreateTextInputPage()
        {
            NameValue<Func<Control>>[] pages =
            {
                new("Text", () => new TextInputPage()),

                new("Numbers", () =>
                {
                    return new TextNumbersPage();
                }),

                new("Memo", () =>
                {
                    return new TextMemoPage();
                }),

                new("Rich", () =>
                {
                    return new TextRichPage();
                }),

                new("Other", () =>
                {
                    return new TextOtherPage();
                }),
            };

            return CreateCustomPage(pages);
        }

        Control CreateLayoutPage()
        {
            NameValue<Func<Control>>[] pages =
            {
                new("Splitter", () => new LayoutPanelPage()),
                new("Grid", () => new GridPage()),
                new("Other", () => new LayoutSample.LayoutMainControl()),                
            };

            return CreateCustomPage(pages);
        }

        Control CreateDateTimePage()
        {
            NameValue<Func<Control>>[] pages =
            {
                new("DateTime", () => new DateTimePage()),
                new("Calendar", () => new CalendarPage()),
                new("Popup", () => new DateTimePopups()),
            };

            return CreateCustomPage(pages);
        }

        Control CreateMultimediaPage()
        {
            NameValue<Func<Control>>? animationNameValue;

            if (!App.IsLinuxOS)
                animationNameValue = new("Animation", () => new AnimationPage());
            else
                animationNameValue = null;

            NameValue<Func<Control>>?[] pages =
            {
                new("System Sounds", () => new SystemSoundsPage()),
                new("Sound Player", () => new SoundPlayerPage()),
                animationNameValue,
            };

            return CreateCustomPage(pages);
        }

        Control CreateTreeViewPage() => new TreeViewPage();
        Control CreateListViewPage() => new ListViewPage();
        Control CreateTabControlPage() => new TabControlPage();
        Control CreateNumericInputPage() => new NumericInputPage();
        Control CreateNotifyIconPage() => new NotifyIconPage();
        Control CreateWebBrowserPage() => new WebBrowserPage();
        Control CreateAllSamplesPage() => new AllSamplesPage();
        Control CreateInternalSamplesPage() => new InternalSamplesPage();
        Control CreateWelcomePage() => new WelcomePage();

        private void LinkLabel_LinkClicked(
            object? sender,
            System.ComponentModel.CancelEventArgs e)
        {
            if (sender is not LinkLabel linkLabel)
                return;
            LogEvent(linkLabel.Url);
        }

        [Browsable(false)]
        public string? LastEventMessage => eventsControl.LastLogMessage;

        public void LogEventSmart(string? message, string? prefix)
        {
            eventsControl.LogReplace(message, prefix);
        }

        public void LogEvent(string? message)
        {
            eventsControl.Log(message);
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