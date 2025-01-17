﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI
{
    public class CustomInternalSamplesPage : Control
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
            Margin = (0, 0, 0, 5),
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        public CustomInternalSamplesPage()
        {
            view.SelectedIndexChanged += (s, e) =>
            {
                App.DebugLogIf(
                    $"InternalSamples.SelectedIndexChanged: {view.SelectedIndex}",
                    false);
            };

            view.Parent = this;
            buttonPanel.Parent = this;
            runButton.Parent = buttonPanel;
            runButton.Click += RunButton_Click;
            Layout = LayoutStyle.Horizontal;
            Padding = 10;
            AddDefaultItems();

            while (SampleItems.Count > 0)
            {
                view.Items.Add(SampleItems.Pop());
            }

            RunWhenIdle(view.SelectFirstItem);
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

        protected virtual void AddDefaultItems()
        {
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
