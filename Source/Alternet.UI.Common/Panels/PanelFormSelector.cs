using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains grouped listbox with the list of forms and "Open" button which
    /// creates and shows them. Use <see cref="AddGroup"/> to add group and <see cref="Add"/>
    /// to add item.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelFormSelector"/> class.
        /// </summary>
        public PanelFormSelector()
        {
            Padding = 10;

            Layout = LayoutStyle.Horizontal;
            openButton.Parent = buttonPanel;
            openButton.Click += HandleOpenButtonClick;

            view.HorizontalAlignment = HorizontalAlignment.Fill;
            view.Parent = this;

            buttonPanel.HorizontalAlignment = HorizontalAlignment.Right;
            buttonPanel.Parent = this;

            AddDefaultItems();

            view.SelectionChanged += View_SelectionChanged;

            view.SelectFirstItem();
            view.EnsureVisible(0);
            View_SelectionChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Gets collection of the items.
        /// </summary>
        public IListControlItems<ListControlItem> Items => ListBox.Items;

        /// <summary>
        /// Gets "Open" button.
        /// </summary>
        public Button OpenButton => openButton;

        /// <summary>
        /// Gets listbox with the list of forms.
        /// </summary>
        public VirtualListBox ListBox => view;

        /// <summary>
        /// Gets button panel which contains "Open" button.
        /// </summary>
        public VerticalStackPanel ButtonPanel => buttonPanel;

        /// <summary>
        /// Gets or sets group font. Assign it before adding of the groups.
        /// </summary>
        public virtual Font? GroupFont { get; set; }

        /// <summary>
        /// Gets or sets whether any groups were added. This property is used
        /// in order to determine identation of the items.
        /// </summary>
        public virtual bool HasGroups { get; set; }

        /// <summary>
        /// Adds item if DEBUG conditional is specified.
        /// </summary>
        [Conditional("DEBUG")]
        public void AddIfDebug(string text, Func<Window> createForm)
        {
            Add(text, createForm);
        }

        /// <summary>
        /// Adds group.
        /// </summary>
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

        /// <summary>
        /// Adds an item with the specified text and form create action.
        /// </summary>
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

        /// <summary>
        /// Called from the constructor. You can override this method in order to
        /// add default items. Use <see cref="AddGroup"/> and <see cref="Add"/> inside this method
        /// for adding the default items.
        /// </summary>
        protected virtual void AddDefaultItems()
        {
        }

        /// <summary>
        /// Called when "Open" button is clicked.
        /// </summary>
        protected virtual void HandleOpenButtonClick(object? sender, EventArgs e)
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

        private void View_SelectionChanged(object? sender, EventArgs e)
        {
            if (view.SelectedItem is not ListControlItem item)
            {
                openButton.Enabled = false;
                return;
            }

            openButton.Enabled = !item.HideSelection;
        }
    }
}
