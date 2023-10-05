using Alternet.UI;
using Alternet.Drawing;
using System;

namespace ControlsSample
{
    internal partial class MainWindow : Window, IPageSite
    {
        private readonly PageContainer pageContainer = new();
        private readonly TreeView eventsControl = new();
        private readonly Grid mainGrid = new();
        private readonly Control mainGridParent = new();
        private readonly LinkLabel? linkLabel;
        private readonly LinkLabel? linkLabel2;
        private string? lastEventMessage = null;
        private int lastEventNumber = 1;

        public MainWindow()
        {
            Application.Current.LogMessage += Application_LogMessage;

            Icon = ImageSet.FromUrlOrNull("embres:ControlsSample.Sample.ico");
            InitializeComponent();

            eventsControl.MakeAsListBox();

            mainGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Auto)
            }
            );
            mainGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(100, GridUnitType.Star)
            }
            );
            mainGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(150, GridUnitType.Pixel)
                }
            );

            var headerPanel = new HorizontalStackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            var homePage = @"https://www.alternet-ui.com/";
            var docsHomePage = @"https://docs.alternet-ui.com/";
            linkLabel = new LinkLabel()
            {
                Text = "Home Page",
                Margin = new Thickness(5,0,5,10),
                Url = homePage,
            };
            linkLabel.LinkClicked += LinkLabel_LinkClicked;
            
            linkLabel2 = new LinkLabel()
            {
                Text = "Documentation",
                Margin = new Thickness(5,0,10,10),
                Url = $"{docsHomePage}introduction/getting-started.html",
            };
            linkLabel2.LinkClicked += LinkLabel_LinkClicked;

            linkLabel.VisitedColor = linkLabel.NormalColor;
            linkLabel2.VisitedColor = linkLabel.NormalColor;

            headerPanel.Children.Add(linkLabel);
            headerPanel.Children.Add(linkLabel2);

            eventsControl.Margin = new(0,10,0,0);
            mainGrid.Children.Add(headerPanel);
            mainGrid.Children.Add(pageContainer);
            mainGrid.Children.Add(eventsControl);
            Alternet.UI.Grid.SetRow(headerPanel, 0);
            Alternet.UI.Grid.SetRow(pageContainer, 1);
            Alternet.UI.Grid.SetRow(eventsControl, 2);

            var pages = pageContainer.Pages;

            void AddPage(string title, CreateControlAction action)
            {
                var item = new PageContainer.Page(title, action);
                pages.Add(item);
            }

            AddPage("Tree View", CreateTreeViewPage);
            AddPage("List View", CreateListViewPage);
            AddPage("List Box", CreateListBoxPage);
            AddPage("Combo Box", CreateComboBoxPage);
            AddPage("Check List Box", CreateCheckListBoxPage);
            AddPage("Tab Control", CreateTabControlPage);
            AddPage("Progress Bar", CreateProgressBarPage);
            AddPage("Button", CreateButtonPage);
            AddPage("Slider", CreateSliderPage);
            AddPage("Grid", CreateGridPage);
            AddPage("Numeric Input", CreateNumericInputPage);
            AddPage("Radio Button", CreateRadioButtonsPage);
            AddPage("Check Box", CreateCheckBoxesPage);
            AddPage("Text Input", CreateTextInputPage);
            AddPage("Date Time", CreateDateTimePage);
            if(NotifyIcon.IsAvailable)
                AddPage("Notify Icon", CreateNotifyIconPage);
            AddPage("Web Browser", CreateWebBrowserPage);
            AddPage("Splitter Panel", CreateSplitterPanelPage);
            AddPage("Layout Panel", CreateLayoutPanelPage);
            AddPage("All Samples", CreateAllSamplesPage);

            pageContainer.SelectedIndex = 0;

            mainGridParent.Padding = 10;
            mainGridParent.Children.Add(mainGrid);
            Children.Add(mainGridParent);
#if DEBUG
            LogEvent("Net Version = " + Environment.Version.ToString());
#endif
            if (pageContainer.PagesControl.CanAcceptFocus)
                pageContainer.PagesControl.SetFocus();
        }

        Control CreateTreeViewPage() => new TreeViewPage() { Site = this };
        Control CreateListViewPage() => new ListViewPage() { Site = this };
        Control CreateListBoxPage() => new ListBoxPage() { Site = this };
        Control CreateComboBoxPage() => new ComboBoxPage() { Site = this };
        Control CreateCheckListBoxPage() => new CheckListBoxPage() { Site = this };
        Control CreateTabControlPage() => new TabControlPage() { Site = this };
        Control CreateProgressBarPage() => new ProgressBarPage() { Site = this };
        Control CreateButtonPage() => new ButtonPage() { Site = this };
        Control CreateSliderPage() => new SliderPage() { Site = this };
        Control CreateGridPage() => new GridPage() { Site = this };
        Control CreateNumericInputPage() => new NumericInputPage() { Site = this };
        Control CreateRadioButtonsPage() => new RadioButtonsPage() { Site = this };
        Control CreateCheckBoxesPage() => new CheckBoxesPage() { Site = this };
        Control CreateTextInputPage() => new TextInputPage() { Site = this };
        Control CreateDateTimePage() => new DateTimePage() { Site = this };
        Control CreateNotifyIconPage() => new NotifyIconPage() { Site = this };
        Control CreateWebBrowserPage() => new WebBrowserPage() { Site = this };
        Control CreateSplitterPanelPage() => new SplitterPanelPage() { Site = this };
        Control CreateLayoutPanelPage() => new LayoutPanelPage() { Site = this };
        Control CreateAllSamplesPage() => new AllSamplesPage() { Site = this };

        private void Application_LogMessage(object? sender, LogMessageEventArgs e)
        {
#if DEBUG
            if (e.ReplaceLastMessage)
                LogEventSmart(e.Message, e.MessagePrefix);
            else
                LogEvent(e.Message);
#endif
        }

        private void LinkLabel_LinkClicked(
            object? sender,
            System.ComponentModel.CancelEventArgs e)
        {
            if (sender is not LinkLabel linkLabel)
                return;
            LogEvent(linkLabel.Url);
        }

        public string? LastEventMessage => lastEventMessage;

        public void LogEventSmart(string message, string? prefix)
        {
            var s = lastEventMessage;
            var b = s?.StartsWith(prefix ?? string.Empty) ?? false;

            var item = eventsControl.LastRootItem;

            if (b && item is not null)
            {
                item.Text = ConstructMessage(message);
                eventsControl.SelectAndShowItem(item);
            }
            else
                LogEvent(message);
        }

        private string ConstructMessage(string message)
        {
            var s = $"{lastEventNumber++}. {message}";
            return s;
        }

        public void LogEvent(string message)
        {
            lastEventMessage = message;

            var item = new TreeViewItem(ConstructMessage(message));
            eventsControl.Items.Add(item);
            eventsControl.SelectAndShowItem(item);
        }
    }
}