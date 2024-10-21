using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class ApiDocCustomSamplesPage : Control
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

        private Font? groupFont;

        public ApiDocCustomSamplesPage()
        {
            view.Parent = this;
            buttonPanel.Parent = this;
            runButton.Parent = buttonPanel;
            runButton.Click += RunButton_Click;
            Layout = LayoutStyle.Horizontal;
            Padding = 10;
            AddDefaultItems();

            foreach(var item in SampleItems)
            {
                view.Items.Add(item);
            }

            view.SelectionChanged += View_SelectionChanged;

            view.SelectFirstItem();
            view.EnsureVisible(0);
            View_SelectionChanged(null, EventArgs.Empty);
        }

        private void View_SelectionChanged(object? sender, EventArgs e)
        {
            if(view.SelectedItem is not ListControlItem item)
            {
                runButton.Enabled = false;
                return;
            }

            runButton.Enabled = !item.HideSelection;
        }

        private List<ListControlItem> SampleItems = new();

        [Conditional("DEBUG")]
        public void AddIfDebug(string text, Func<Window> createForm)
        {
            Add(text, createForm);
        }

        public void AddGroup(string title)
        {
            ListControlItem item = new(title);
            item.Font = groupFont ??= Control.DefaultFont.Larger().AsBold;
            item.HideSelection = true;
            item.HideFocusRect = true;
            SampleItems.Add(item);
        }

        public void Add(string text, Func<Window?> createForm)
        {
            ListControlItem item = new(text);
            item.SvgImage = KnownSvgImages.ImgEmpty;
            item.SvgImageSize = GraphicsFactory.PixelFromDip(16, ScaleFactor);
            item.CustomAttr.SetAttribute<Action>("Fn", Fn);
            SampleItems.Add(item);

            void Fn()
            {
                var form = createForm();
                form?.Show();
            }
        }

        protected virtual void AddDefaultItems()
        {
        }

        private void RunButton_Click(object? sender, EventArgs e)
        {
            if (view.SelectedItem is not ListControlItem item)
                return;
            if (item.HideSelection)
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
