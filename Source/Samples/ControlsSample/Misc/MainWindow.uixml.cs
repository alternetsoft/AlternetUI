using Alternet.UI;
using Alternet.Drawing;
using System;

namespace ControlsSample
{
    internal partial class MainWindow : Window
    {
        private readonly PageContainer pageContainer = new();
        private readonly LogListBox eventsControl = new()
        {
            HasBorder = false,
            Margin = new(0, 10, 0, 0),
            SuggestedHeight = 150,
        };        
        private readonly Grid mainGrid = new();
        private readonly Control mainGridParent = new();

        public MainWindow()
        {
            eventsControl.BindApplicationLog();

            Icon = ImageSet.FromUrlOrNull("embres:ControlsSample.Sample.ico");
            InitializeComponent();

            mainGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(100, GridUnitType.Star)
            }
            );
            mainGrid.AddAutoRow();
                
            mainGrid.Children.Add(pageContainer);
            mainGrid.Children.Add(eventsControl);
            Grid.SetRow(pageContainer, 0);
            Grid.SetRow(eventsControl, 1);

            var pages = pageContainer.Pages;

            void AddPage(string title, Func<Control> action)
            {
                var item = new PageContainer.Page(title, action);
                pages.Add(item);
            }

            AddPage("Welcome", CreateWelcomePage);
            AddPage("Text Input", CreateTextInputPage);
            AddPage("List Controls", CreateListControlsPage);
            AddPage("Buttons", CreateButtonsPage);
            AddPage("Tree View", CreateTreeViewPage);
            AddPage("List View", CreateListViewPage);
            AddPage("Date and Time", CreateDateTimePage);
            AddPage("Web Browser", CreateWebBrowserPage);
            AddPage("Numeric Input", CreateNumericInputPage);
            AddPage("Slider and Progress", CreateSliderAndProgressPage);
            AddPage("Layout", CreateLayoutPage);
            AddPage("Animation", CreateAnimationPage);
            AddPage("Notify and ToolTip", CreateNotifyIconPage);
            AddPage("Tab Control", CreateTabControlPage);

            if(AllSamplesPage.GetSamplesFolder() is not null)
                AddPage("Other Samples", CreateAllSamplesPage);

            pageContainer.SelectedIndex = 0;

            mainGridParent.Padding = 10;
            mainGridParent.Children.Add(mainGrid);
            Children.Add(mainGridParent);

            LogUtils.DebugLogVersion();

            if (pageContainer.PagesControl.CanAcceptFocus)
                pageContainer.PagesControl.SetFocus();
        }

        Control CreateCustomPage(NameValue<Func<Control>>[] pages)
        {
            GenericTabControl result = new()
            {
                Padding = 5,
            };

            result.AddRange(pages);
            result.SelectFirstTab();
            return result;
        }

        Control CreateListControlsPage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("ListBox", () => new ListBoxPage()),
                new("CheckListBox", () => new CheckListBoxPage()),
                new("ComboBox", () => new ComboBoxPage()),
                new("Popups", () => new ListControlsPopups()),
            ];

            return CreateCustomPage(pages);
        }

        Control CreateButtonsPage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("Button", () => new ButtonPage()),
                new("CheckBox", () => new CheckBoxesPage()),
                new("RadioButton", () => new RadioButtonsPage()),
            ];

            return CreateCustomPage(pages);
        }

        Control CreateSliderAndProgressPage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("Slider", () => new SliderPage()),
                new("ProgressBar", () => new ProgressBarPage()),
            ];

            return CreateCustomPage(pages);
        }

        Control CreateTextInputPage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("Old", () => new TextInputPage()),
            ];

            return CreateCustomPage(pages);
        }

        Control CreateLayoutPage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("SplitterPanel", () => new SplitterPanelPage()),
                new("Grid", () => new GridPage()),
                new("LayoutPanel", () => new LayoutPanelPage()),
            ];

            return CreateCustomPage(pages);
        }

        Control CreateDateTimePage()
        {
            NameValue<Func<Control>>[] pages =
            [
                new("DateTimeEdit", () => new DateTimePage()),
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