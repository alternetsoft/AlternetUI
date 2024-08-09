using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;
using System.IO;
using System.Diagnostics;

namespace ControlsSample
{
    public partial class InternalSamplesPage : Control
    {
        private readonly VirtualListBox view = new()
        {
            SuggestedWidth = 350,
            SuggestedHeight = 400,
        };

        private readonly VerticalStackPanel buttonPanel = new()
        {
            Margin = (10, 0, 0, 0),
            Padding = 5,
        };

        private readonly Button runButton = new()
        {
            Text = "Run Sample",
            Margin = (0,0,0,5),
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        public InternalSamplesPage()
        {
            view.Parent = this;
            buttonPanel.Parent = this;
            runButton.Parent = buttonPanel;
            runButton.Click += RunButton_Click;
            Layout = LayoutStyle.Horizontal;
            Padding = 10;
            AddDefaultItems();
            view.SelectFirstItem();
            view.EnsureVisible(0);
        }

        public static Stack<ListControlItem> SampleItems = new();

        [Conditional("DEBUG")]
        public static void AddIfDebug(string text, Func<Window> createForm)
        {
            Add(text, createForm);
        }

        public static void Add(string text, Func<Window> createForm)
        {
            ListControlItem item = new(text);
            item.CustomAttr.SetAttribute<Action>("Fn", Fn);
            SampleItems.Push(item);

            void Fn()
            {
                var form = createForm();
                form.Show();
            }
        }

        private void AddDefaultItems()
        {
            /*
                Add("NinePatch Drawing Sample", () => new NinePatchDrawingWindow());
            */

            Add("Threading Sample", () => new ThreadingSample.ThreadingMainWindow());
            AddIfDebug("Test Page", () => new SkiaDrawingWindow());
            Add("Preview File Sample", () => new PreviewSample.PreviewSampleWindow());
            Add("Explorer UI Sample", () => new ExplorerUISample.ExplorerMainWindow());
            Add("Printing Sample", () => new PrintingSample.PrintingMainWindow());
            Add("Menu Sample", () => new MenuSample.MenuMainWindow());
            Add("Mouse Input", () => new InputSample.MouseInputWindow());
            Add("Keyboard Input", () => new InputSample.KeyboardInputWindow());
            Add("Drawing Sample", () => new DrawingSample.DrawingMainWindow());
            Add("Drag and Drop", () => new DragAndDropSample.DragAndDropWindow());
            Add("SkiaSharp MegaDemo", () => new SkiaSharpExamplesWindow());
            Add("Paint Sample", () => new PaintSample.PaintMainWindow());
            Add("Custom Controls", () => new CustomControlsSample.CustomControlsWindow());
            Add("Window Properties", () => new WindowPropertiesSample.WindowPropertiesWindow());
            Add("Common Dialogs", () => new CommonDialogsWindow());
            Add("Employee Form", () => new EmployeeFormSample.EmployeeWindow());
            Add("Property Grid", () => new PropertyGridSample.MainWindow());

            while(SampleItems.Count > 0)
            {
                view.Items.Add(SampleItems.Pop());
            }
        }

        private void RunButton_Click(object? sender, EventArgs e)
        {
            if (view.SelectedItem is not ListControlItem item)
                return;
            var fn = item.CustomAttr.GetAttribute<Action>("Fn");
            if (fn is not null)
            {
                App.DoInsideBusyCursor(() =>
                {
                    runButton.Enabled = false;
                    runButton.Refresh();
                    try
                    {
                        fn();
                    }
                    finally
                    {
                        runButton.Enabled = true;
                        runButton.Refresh();
                    }
                });
            }
        }
    }
}