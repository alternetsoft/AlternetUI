using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// This control can change visibility of other controls depending on
    /// the selected tab.
    /// </summary>
    /// <remarks>
    /// Allows to switch cards in the <see cref="CardPanel"/> control by clicking
    /// on the card title.
    /// </remarks>
    public class CardPanelHeader : Control
    {
        private readonly Collection<CardPanelHeaderItem> tabs = new();
        private readonly HorizontalStackPanel stackPanel = new();

        private readonly Border border = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            BorderWidth = new(0, 0, 0, 3),
            Padding = new(0, 0, 0, 5),
            Margin = new(0, 0, 0, 5),
        };

        private Size additionalSpace = new(30, 30);
        private CardPanelHeaderItem? selectedTab;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanelHeader"/> class.
        /// </summary>
        public CardPanelHeader()
        {
            stackPanel.Parent = border;
            border.Parent = this;
        }

        /// <summary>
        /// Occurs when the tab is clicked.
        /// </summary>
        public event EventHandler? TabClick;

        /// <summary>
        /// Gets tabs added with <see cref="Add"/> method.
        /// </summary>
        public IReadOnlyList<CardPanelHeaderItem> Tabs => tabs;

        /// <summary>
        /// Gets or sets size of the additional space which is added when
        /// <see cref="UpdateCardsMode"/> is not <see cref="WindowSizeToContentMode.None"/>.
        /// </summary>
        public Size AdditionalSpace
        {
            get
            {
                return additionalSpace;
            }

            set
            {
                additionalSpace = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to update visibility of the cards when selected tab is changed.
        /// </summary>
        public bool UpdateCardsVisible { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to update width of the cards when selected tab is changed.
        /// </summary>
        public WindowSizeToContentMode UpdateCardsMode { get; set; } = WindowSizeToContentMode.WidthAndHeight;

        /// <summary>
        /// Gets selected tab.
        /// </summary>
        public CardPanelHeaderItem? SelectedTab
        {
            get => selectedTab;
            set
            {
                if (selectedTab == value)
                    return;
                selectedTab = value;
                foreach(var tab in tabs)
                {
                    if (selectedTab == tab)
                        tab.HeaderControl.Font = Font.Default.AsBold;
                    else
                        tab.HeaderControl.Font = null;
                }
            }
        }

        /// <summary>
        /// Gets selected tab index or <c>null</c> if no tab is selected.
        /// </summary>
        public int? SelectedTabIndex
        {
            get
            {
                if (selectedTab == null)
                    return null;
                var result = tabs.IndexOfOrNull(selectedTab);
                return result;
            }
        }

        /// <summary>
        /// Updates visibility of the cards.
        /// </summary>
        public virtual void SetCardsVisible()
        {
            foreach (var tab in tabs)
            {
                if (tab.CardControl is null || SelectedTab == tab)
                    continue;
                tab.CardControl.Visible = false;
            }

            var control = SelectedTab?.CardControl;

            if (control is not null)
                control.Visible = true;
        }

        /// <summary>
        /// Calculates maximal width of cards.
        /// </summary>
        public virtual Size GetMaxCardSize()
        {
            return Size.MaxWidthHeight(GetCardSizes());
        }

        /// <summary>
        /// Selects the first tab if it exists.
        /// </summary>
        public virtual void SelectFirstTab()
        {
            if (Tabs.Count > 0)
                SelectedTab = Tabs[0];
        }

        /// <summary>
        /// Sets width of all cards to value obtained with <see cref="GetMaxCardSize"/>.
        /// </summary>
        public virtual void CardsWidthToMax(WindowSizeToContentMode mode)
        {
            if (mode == WindowSizeToContentMode.None)
                return;

            var parent = GetCardsParent();

            if (parent is null)
                return;

            var update = false;

            switch (mode)
            {
                case WindowSizeToContentMode.Width:
                    update = double.IsNaN(parent.SuggestedWidth);
                    break;
                case WindowSizeToContentMode.Height:
                    update = double.IsNaN(parent.SuggestedHeight);
                    break;
                case WindowSizeToContentMode.WidthAndHeight:
                    update = Size.AnyIsNaN(parent.SuggestedSize);
                    break;
            }

            if (update)
            {
                var maxSize = GetMaxCardSize();
                parent.SuspendLayout();

                var newWidth = Math.Max(
                    maxSize.Width + additionalSpace.Width,
                    parent.Bounds.Width);
                var newHeight = Math.Max(
                    maxSize.Height + this.Bounds.Height + this.Margin.Vertical + additionalSpace.Height,
                    parent.Bounds.Height);

                switch (mode)
                {
                    case WindowSizeToContentMode.Width:
                        parent.SuggestedWidth = newWidth;
                        parent.MinWidth = newWidth;
                        break;
                    case WindowSizeToContentMode.Height:
                        parent.SuggestedHeight = newHeight;
                        parent.MinHeight = newHeight;
                        break;
                    case WindowSizeToContentMode.WidthAndHeight:
                        parent.SuggestedSize = new Size(newWidth, newHeight);
                        parent.MinimumSize = parent.SuggestedSize;
                        break;
                }

                parent.ResumeLayout();
            }
        }

        /// <summary>
        /// Adds new item to the control.
        /// </summary>
        /// <param name="text">Item title.</param>
        /// <param name="cardControl">Associated card control.</param>
        public virtual void Add(string text, Control? cardControl = null)
        {
            var control = new Button(text)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Parent = stackPanel,
            };
            control.Click += Item_Click;

            var item = new CardPanelHeaderItem(control)
            {
                CardControl = cardControl,
            };

            tabs.Add(item);
        }

        /// <summary>
        /// Gets parent control of the cards.
        /// </summary>
        public virtual Control? GetCardsParent()
        {
            foreach (var tab in tabs)
            {
                if (tab.CardControl is not null)
                {
                    return tab.CardControl.Parent;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets cards width and height.
        /// </summary>
        public virtual Size[] GetCardSizes()
        {
            Size[] result = new Size[tabs.Count];
            for (int i = 0; i < tabs.Count; i++)
            {
                var control = tabs[i].CardControl;
                if (control is null)
                    continue;
                result[i] = control.Bounds.Size;
            }

            return result;
        }

        private void Item_Click(object? sender, EventArgs e)
        {
            foreach(var tab in tabs)
            {
                if(sender == tab.HeaderControl && SelectedTab != tab)
                {
                    SelectedTab = tab;
                    if(UpdateCardsVisible) SetCardsVisible();
                    CardsWidthToMax(UpdateCardsMode);
                    TabClick?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}