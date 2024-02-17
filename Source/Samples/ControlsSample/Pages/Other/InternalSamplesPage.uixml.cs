﻿using System;
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
        public InternalSamplesPage()
        {
            InitializeComponent();
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

            Add("Employee Form", ()=> new EmployeeFormSample.EmployeeWindow());
            Add("Common Dialogs", () => new CommonDialogsWindow());
            Add("Window Properties", () => new WindowPropertiesSample.WindowPropertiesWindow());
            Add("Custom Controls", () => new CustomControlsSample.CustomControlsWindow());
            Add("Data Binding", () => new DataBindingSample.DataBindingWindow());
            Add("Drag and Drop", () => new DragAndDropSample.DragAndDropWindow());            
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