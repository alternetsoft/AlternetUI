using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class PanelFormSelector : Panel
    {
        private readonly VirtualListBox view = new()
        {
            MinimumSize = (350, 400),
        };

        private readonly VerticalStackPanel buttonPanel = new()
        {
            Margin = (10, 0, 0, 0),
        };

        private readonly Button openButton = new()
        {
            Text = "Open",
            Margin = (0, 0, 0, 5),
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        public PanelFormSelector()
        {
            Padding = 10;

            Layout = LayoutStyle.Horizontal;
            openButton.Parent = buttonPanel;
            openButton.Click += RunButton_Click;

            view.Parent = this;
            buttonPanel.Parent = this;

            AddDefaultItems();

            foreach(var item in Items)
            {
                view.Items.Add(item);
            }

            view.SelectionChanged += View_SelectionChanged;

            view.SelectFirstItem();
            view.EnsureVisible(0);
            View_SelectionChanged(null, EventArgs.Empty);
        }

        public Button OpenButton => openButton;

        public VirtualListBox ListBox => view;

        public VerticalStackPanel ButtonPanel => buttonPanel;

        public virtual Font? GroupFont { get; set; }

        public virtual bool HasGroups { get; set; }

        private void View_SelectionChanged(object? sender, EventArgs e)
        {
            if(view.SelectedItem is not ListControlItem item)
            {
                openButton.Enabled = false;
                return;
            }

            openButton.Enabled = !item.HideSelection;
        }

        private readonly List<ListControlItem> Items = new();

        [Conditional("DEBUG")]
        public void AddIfDebug(string text, Func<Window> createForm)
        {
            Add(text, createForm);
        }

        public void AddGroup(string title)
        {
            HasGroups = true;
            ListControlItem item = new(title)
            {
                Font = GroupFont ??= Control.DefaultFont.Larger().AsBold,
                HideSelection = true,
                HideFocusRect = true,
            };
            Items.Add(item);
        }

        public void Add(string text, Func<Window> createForm)
        {
            ListControlItem item = new(text);

            if (HasGroups)
            {
                item.SvgImage = KnownSvgImages.ImgEmpty;
                item.SvgImageSize = GraphicsFactory.PixelFromDip(16, ScaleFactor);
            }

            item.CustomAttr.SetAttribute<Action>("Fn", Fn);
            Items.Add(item);

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
            if (item.HideSelection)
                return;
            var fn = item.CustomAttr.GetAttribute<Action>("Fn");
            if (fn is not null)
            {
                App.DoInsideBusyCursor(() =>
                {
                    openButton.Enabled = false;
                    openButton.Refresh();
                    try
                    {
                        fn();
                    }
                    finally
                    {
                        openButton.Enabled = true;
                        openButton.Refresh();
                    }
                });
            }
        }
    }
}
