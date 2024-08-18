using System;
using System.ComponentModel;
using System.IO;

using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public partial class MainWindow : CustomDemoWindow
    {
        protected override void AddPages()
        {
            AddPage("Welcome", CreateWelcomePage);
            AddPage("Text", CreateTextInputPage);
            AddPage("ListBoxes", CreateListControlsPage);
            AddPage("Buttons", CreateButtonsPage);
            AddPage("TreeView", CreateTreeViewPage);
            AddPage("ListView", CreateListViewPage);
            AddPage("DateTime", CreateDateTimePage);
            AddPage("WebBrowser", CreateWebBrowserPage);
            AddPage("Number", CreateNumericInputPage);
            AddPage("Slider, Progress", CreateSliderAndProgressPage);
            AddPage("Layout", CreateLayoutPage);
            AddPage("Notify, ToolTip", CreateNotifyIconPage);
            AddPage("TabControl", CreateTabControlPage);
            AddPage("Multimedia", CreateMultimediaPage);
            AddPage("Samples", CreateOtherPage);
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
    }
}