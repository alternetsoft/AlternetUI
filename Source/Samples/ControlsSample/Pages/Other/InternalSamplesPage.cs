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
    internal partial class InternalSamplesPage : Control
    {
        private readonly ListBox view = new()
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
        }

        private void AddDefaultItems()
        {
            void Add(string text, Func<Window> createForm)
            {
                ListControlItem item = new(text);
                item.CustomAttr.SetAttribute<Action>("Fn", Fn);
                view.Items.Add(item);

                void Fn()
                {
                    var form = createForm();
                    form.Show();
                }
            }

            Add("Property Grid", () => new PropertyGridSample.MainWindow());
            Add("Employee Form", ()=> new EmployeeFormSample.EmployeeWindow());
            Add("Common Dialogs", () => new CommonDialogsWindow());
            Add("Window Properties", () => new WindowPropertiesSample.WindowPropertiesWindow());
            Add("Custom Controls", () => new CustomControlsSample.CustomControlsWindow());
            Add("Paint Sample", () => new PaintSample.MainWindow());            
            Add("Data Binding", () => new DataBindingSample.DataBindingWindow());
            Add("Drag and Drop", () => new DragAndDropSample.DragAndDropWindow());
            Add("Drawing Sample", () => new DrawingSample.MainWindow());
            Add("Keyboard Input", () => new InputSample.KeyboardInputWindow());
            Add("Mouse Input", () => new InputSample.MouseInputWindow());
            Add("Menu Sample", () => new MenuSample.MainWindow());
            Add("Printing Sample", () => new PrintingSample.MainWindow());
            Add("Explorer UI Sample", () => new ExplorerUISample.MainWindow());
            Add("Threading Sample", () => new ThreadingSample.MainWindow());
        }

        private void RunButton_Click(object? sender, EventArgs e)
        {
            if (view.SelectedItem is not ListControlItem item)
                return;
            var fn = item.CustomAttr.GetAttribute<Action>("Fn");
            if (fn is not null)
            {
                Application.DoInsideBusyCursor(() =>
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