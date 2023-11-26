using Alternet.UI;
using Alternet.Drawing;
using System;

namespace ControlsSample
{
    internal partial class MainWindow : Window
    {
        private SplittedTreeAndCards? pageContainer;
        private readonly LogListBox eventsControl = new()
        {
            HasBorder = false,
            Margin = (0, 10, 0, 0),
        };

        private readonly SplitterPanel splitterPanel = new()
        {
        };

        private readonly Control panel = new()
        {
            Padding = 5,
        };

        static MainWindow()
        {
            UseDebugBackgroundColor = false;
        }

        public MainWindow()
        {
            eventsControl.BindApplicationLog();
            DoInsideLayout(Initialize);
        }

        public void Initialize()
        {
            DebugBackgroundColor(Color.Red, nameof(MainWindow));
            pageContainer = new();
            pageContainer.TreeView.MakeAsListBox();
            pageContainer.SetDebugColors();
            panel.Parent = this;
            Title = "Alternet UI Controls Sample";
            Size = (900, 700);
            StartLocation = WindowStartLocation.CenterScreen;

            Icon = ImageSet.FromUrlOrNull("embres:ControlsSample.Sample.ico");

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

            // On Ubuntu 23 animation is not working properly.
            if (!Application.IsLinuxOS)
                AddPage("Animation", CreateAnimationPage);

            if (AllSamplesPage.GetSamplesFolder() is not null)
                AddPage("Other", CreateAllSamplesPage);

            LogUtils.DebugLogVersion();

            splitterPanel.Parent = panel;
            pageContainer.Parent = splitterPanel;
            eventsControl.Parent = splitterPanel;

            splitterPanel.SashGravity = 1.0;

            // Split is called at the end. This is done because we need to be sure that
            // used controls will not be recreated due to property changes.
            pageContainer.SplitVerticalDip(140);
            splitterPanel.SplitHorizontalDip(pageContainer, eventsControl, -150);
            pageContainer.SelectedIndex = 0;
            pageContainer.TreeView.SetFocusIfPossible();
        }

        Control CreateCustomPage(NameValue<Func<Control>>[] pages)
        {
            GenericTabControl result = new()
            {
            };

            result.AddRange(pages);
            result.SelectFirstTab();
            if (result.Header.Tabs.Count <= 1)
                result.Header.Hide();
            return result;
        }

        Control CreateListControlsPage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("List", () => new ListBoxPage()),
                new("Checks", () => new CheckListBoxPage()),
                new("Combo", () => new ComboBoxPage()),
                new("Popup", () => new ListControlsPopups()),
            ];

            return CreateCustomPage(pages);
        }

        Control CreateButtonsPage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("Button", () => new ButtonPage()),
                new("Check", () => new CheckBoxesPage()),
                new("Radio", () => new RadioButtonsPage()),
            ];

            return CreateCustomPage(pages);
        }

        Control CreateSliderAndProgressPage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("Slider", () => new SliderPage()),
                new("Progress", () => new ProgressBarPage()),
            ];

            return CreateCustomPage(pages);
        }

        Control CreateTextInputPage()
        {
            NameValue<Func<Control>>[] pages =
            [
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
            ];

            return CreateCustomPage(pages);
        }

        Control CreateLayoutPage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("Splitter", () => new SplitterPanelPage()),
                new("Grid", () => new GridPage()),
                new("LayoutPanel", () => new LayoutPanelPage()),
            ];

            return CreateCustomPage(pages);
        }

        Control CreateDateTimePage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("DateTime", () => new DateTimePage()),
                new("Calendar", () => new CalendarPage()),
            ];

            return CreateCustomPage(pages);
        }

        Control CreateAnimationPage() => new AnimationPage();
        Control CreateTreeViewPage() => new TreeViewPage();
        Control CreateListViewPage() => new ListViewPage();
        Control CreateTabControlPage() => new TabControlPage();
        Control CreateNumericInputPage() => new NumericInputPage();
        Control CreateNotifyIconPage() => new NotifyIconPage();
        Control CreateWebBrowserPage() => new WebBrowserPage();
        Control CreateAllSamplesPage() => new AllSamplesPage();
        Control CreateWelcomePage() => new WelcomePage();

        private void LinkLabel_LinkClicked(
            object? sender,
            System.ComponentModel.CancelEventArgs e)
        {
            if (sender is not LinkLabel linkLabel)
                return;
            LogEvent(linkLabel.Url);
        }

        public string? LastEventMessage => eventsControl.LastLogMessage;

        public void LogEventSmart(string? message, string? prefix)
        {
            eventsControl.LogReplace(message, prefix);
        }

        public void LogEvent(string? message)
        {
            eventsControl.Log(message);
        }
    }
}