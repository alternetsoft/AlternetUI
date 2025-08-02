using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI
{
    public class CustomInternalSamplesPage : Panel
    {
        private readonly StdTreeView view = new()
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
            view.ListBox.SelectedIndexChanged += (s, e) =>
            {
                App.DebugLogIf(
                    $"InternalSamples.SelectedIndexChanged: {view.ListBox.SelectedIndex}",
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
                view.RootItem.Add(SampleItems.Pop());
            }

            RunWhenIdle(view.SelectFirstItemAndScroll);
        }

        public static Stack<TreeViewItem> SampleItems = new();

        [Conditional("DEBUG")]
        public static void AddIfDebug(string text, Func<Window> createForm)
        {
            Add(text, createForm);
        }

        public static void Add(string text, Func<Window> createForm)
        {
            TreeViewItem item = new(text);
            item.CustomAttr.SetAttribute<Action>("Fn", Fn);
            SampleItems.Push(item);

            void Fn()
            {
                var form = createForm();
                form.ShowAndFocus();
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
