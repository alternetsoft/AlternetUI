﻿using System;
using System.ComponentModel;
using System.IO;

using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public partial class MainWindow : CustomDemoWindow
    {
        static MainWindow()
        {
            DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
            {
            });

            UixmlLoader.Initialize();

            DefaultUseParentFont = true;

            AddGlobalWindowNotification(new GlobalFormActivity());
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);

            FormUtils.CloseOtherWindows(exceptWindow:this);
        }

        protected override void AddPages()
        {
            try
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

                if (DebugUtils.IsDebugDefined)
                {
                    AddPage("Dialogs", CreateDialogsPage);
                }
            }
            catch (Exception e)
            {
                LogUtils.LogExceptionToFile(e);
            }
        }

        AbstractControl CreateListControlsPage()
        {
            NameValue<Func<AbstractControl>>? popupNameValue;

            popupNameValue = new("Popup", () => new ListControlsPopups());

            NameValue<Func<AbstractControl>>?[] pages =
            {
                new("List", () => new ListBoxPage()),
                new("Checks", () => new CheckListBoxPage()),
                new("Combo", () => new ComboBoxPage()),
                new("Virtual", () => new VListBoxSamplePage()),
                new("Colors", () => new ColorListBoxSamplePage()),
                new("Other", () => new ListControlsOtherPage()),               

                popupNameValue,
            };

            return CreateCustomPage(pages);
        }

        AbstractControl CreateButtonsPage()
        {
            NameValue<Func<AbstractControl>>[] pages =
            {
                new("Button", () => new ButtonPage()),
                new("Check", () => new CheckBoxesPage()),
                new("Radio", () => new RadioButtonsPage()),
            };

            return CreateCustomPage(pages);
        }

        AbstractControl CreateSliderAndProgressPage()
        {
            NameValue<Func<AbstractControl>>[] pages =
            {
                new("Slider", () => new SliderPage()),
                new("Progress", () => new ProgressBarPage()),
            };

            return CreateCustomPage(pages);
        }

        AbstractControl CreateDialogsPage()
        {
            return Window.CreateAs<CommonDialogsWindow>(WindowKind.Control);
        }

        AbstractControl CreateOtherPage()
        {
            NameValue<Func<AbstractControl>>[] pagesDebug =
            {
                new("Internal", CreateInternalSamplesPage),
                /*new("External", CreateAllSamplesPage),*/
            };

            NameValue<Func<AbstractControl>>[] pagesRelease =
            {
                new("Internal", CreateInternalSamplesPage),
            };

            var pages = DebugUtils.IsDebugDefined ? pagesDebug : pagesRelease;

            return CreateCustomPage(pages);
        }

        AbstractControl CreateTextInputPage()
        {
            NameValue<Func<AbstractControl>>[] pages =
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

        AbstractControl CreateLayoutPage()
        {
            NameValue<Func<AbstractControl>>[] pages =
            {
                new("Splitter", () => new LayoutPanelPage()),
                new("Grid", () => new GridPage()),
                new("Other", () => new LayoutSample.LayoutMainControl()),                
            };

            return CreateCustomPage(pages);
        }

        AbstractControl CreateDateTimePage()
        {
            NameValue<Func<AbstractControl>>[] pages =
            {
                new("DateTime", () => new DateTimePage()),
                new("Calendar", () => new CalendarPage()),
                new("Popup", () => new DateTimePopups()),
            };

            return CreateCustomPage(pages);
        }

        AbstractControl CreateMultimediaPage()
        {
            NameValue<Func<AbstractControl>>? animationNameValue;

            if (!App.IsLinuxOS)
                animationNameValue = new("Animation", () => new AnimationPage());
            else
                animationNameValue = null;

            NameValue<Func<AbstractControl>>?[] pages =
            {
                new("System Sounds", () => new SystemSoundsPage()),
                new("Sound Player", () => new SoundPlayerPage()),
                animationNameValue,
            };

            return CreateCustomPage(pages);
        }

        AbstractControl CreateTreeViewPage() => new TreeViewPage();
        AbstractControl CreateListViewPage() => new ListViewPage();
        AbstractControl CreateTabControlPage() => new TabControlPage();
        AbstractControl CreateNumericInputPage() => new NumericInputPage();
        
        AbstractControl CreateNotifyIconPage()
        {
            NameValue<Func<AbstractControl>>? nameValue;

            if (NotifyIcon.IsAvailable)
                nameValue = new("Notify Icon", () => new NotifyIconPage());
            else
                nameValue = null;

            NameValue<Func<AbstractControl>>?[] pages =
            {
                new("Rich ToolTip", () => new ToolTipPage()),
                nameValue,
            };

            return CreateCustomPage(pages);
        }

        AbstractControl CreateWebBrowserPage() => new WebBrowserPage();
        /*AbstractControl CreateAllSamplesPage() => new AllSamplesPage();*/
        AbstractControl CreateInternalSamplesPage() => new InternalSamplesPage();
        AbstractControl CreateWelcomePage() => new WelcomePage();

        private void LinkLabel_LinkClicked(
            object? sender,
            System.ComponentModel.CancelEventArgs e)
        {
            if (sender is not LinkLabel linkLabel)
                return;
            LogEvent(linkLabel.Url);
        }

        private class GlobalFormActivity : BaseControlActivity
        {
            public override void AfterCreate(AbstractControl sender, EventArgs e)
            {
                ControlActivities.KeyboardZoomInOut.Initialize(sender);
            }
        }
    }
}