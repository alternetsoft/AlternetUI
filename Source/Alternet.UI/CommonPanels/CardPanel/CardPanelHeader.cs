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
    [ControlCategory("Hidden")]
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

        private bool? useTabBold;
        private bool? useTabForegroundColor;
        private bool? useTabBackgroundColor;
        private Size additionalSpace = new(30, 30);
        private CardPanelHeaderItem? selectedTab;
        private bool? tabHasBorder;
        private CardPanel? cardPanel;
        private IReadOnlyFontAndColor? activeTabColors;
        private IReadOnlyFontAndColor? inactiveTabColors;

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
        /// Gets or sets default value of the <see cref="TabHasBorder"/>.
        /// </summary>
        public static bool DefaultTabHasBorder { get; set; } = false;

        /// <summary>
        /// Gets or sets default value for the <see cref="UseTabBold"/> property.
        /// </summary>
        public static bool DefaultUseTabBold { get; set; } = false;

        /// <summary>
        /// Gets or sets default value for the <see cref="UseTabForegroundColor"/> property.
        /// </summary>
        public static bool DefaultUseTabForegroundColor { get; set; } = true;

        /// <summary>
        /// Gets or sets default value for the <see cref="UseTabBackgroundColor"/> property.
        /// </summary>
        public static bool DefaultUseTabBackgroundColor { get; set; } = false;

        /// <summary>
        /// Gets or sets default active tab colors.
        /// </summary>
        public static IReadOnlyFontAndColor? DefaultActiveTabColors { get; set; }

        /// <summary>
        /// Gets or sets default inactive tab colors.
        /// </summary>
        public static IReadOnlyFontAndColor? DefaultInactiveTabColors { get; set; }

        /// <summary>
        /// Gets or sets whether to set bold style for the title of active tab.
        /// </summary>
        public bool? UseTabBold
        {
            get
            {
                return useTabBold;
            }

            set
            {
                if (useTabBold == value)
                    return;
                useTabBold = value;
                UpdateTabs();
            }
        }

        /// <summary>
        /// Gets or sets whether to set foreground color for the active tab.
        /// </summary>
        public bool? UseTabForegroundColor
        {
            get
            {
                return useTabForegroundColor;
            }

            set
            {
                if (useTabForegroundColor == value)
                    return;
                useTabForegroundColor = value;
                UpdateTabs();
            }
        }

        /// <summary>
        /// Gets or sets whether to set background color for the active tab.
        /// </summary>
        public bool? UseTabBackgroundColor
        {
            get
            {
                return useTabBackgroundColor;
            }

            set
            {
                if (useTabBackgroundColor == value)
                    return;
                useTabBackgroundColor = value;
                UpdateTabs();
            }
        }

        /// <summary>
        /// Gets or sets attached <see cref="CardPanel"/>
        /// </summary>
        public CardPanel? CardPanel
        {
            get => cardPanel;

            set
            {
                if (cardPanel == value)
                    return;
                cardPanel = value;
            }
        }

        /// <summary>
        /// Gets tabs collection.
        /// </summary>
        public IReadOnlyList<CardPanelHeaderItem> Tabs => tabs;

        /// <summary>
        /// Gets or sets whether tabs have border.
        /// </summary>
        public bool? TabHasBorder
        {
            get
            {
                return tabHasBorder;
            }

            set
            {
                if (tabHasBorder == value)
                    return;
                tabHasBorder = value;
                UpdateTabs();
            }
        }

        /// <summary>
        /// Gets or sets active tab colors.
        /// </summary>
        public IReadOnlyFontAndColor? ActiveTabColors
        {
            get
            {
                return activeTabColors;
            }

            set
            {
                if (activeTabColors == value)
                    return;
                activeTabColors = value;
                UpdateTabs();
            }
        }

        /// <summary>
        /// Gets or sets inactive tab colors.
        /// </summary>
        public IReadOnlyFontAndColor? InactiveTabColors
        {
            get
            {
                return inactiveTabColors;
            }

            set
            {
                if (inactiveTabColors == value)
                    return;
                inactiveTabColors = value;
                UpdateTabs();
            }
        }

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
        public WindowSizeToContentMode UpdateCardsMode { get; set; } =
            WindowSizeToContentMode.WidthAndHeight;

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
                    UpdateTab(tab);
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
                if (SelectedTab == tab)
                    continue;
                tab.CardControl?.Hide();
            }

            var id = SelectedTab?.CardUniqueId;
            if (id is null)
                cardPanel?.Hide();

            SelectedTab?.CardControl?.Show();

            if(id is not null)
            {
                cardPanel?.Show();
                cardPanel?.SetActiveCard(id);
            }
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
        public virtual CardPanelHeaderItem Add(string text, Control? cardControl = null)
        {
            var control = new Button(text)
            {
                HasBorder = TabHasBorder ?? DefaultTabHasBorder,
                HorizontalAlignment = HorizontalAlignment.Center,
                Parent = stackPanel,
            };
            control.Click += Item_Click;

            var item = new CardPanelHeaderItem(control)
            {
                CardControl = cardControl,
            };

            tabs.Add(item);

            UpdateTab(item);

            return item;
        }

        /// <summary>
        /// Adds new item to the control.
        /// </summary>
        /// <param name="text">Item title.</param>
        /// <param name="cardId">Associated card unique id.</param>
        public virtual CardPanelHeaderItem Add(string text, ObjectUniqueId cardId)
        {
            var result = Add(text);
            result.CardUniqueId = cardId;
            return result;
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

        private IReadOnlyFontAndColor GetColors(bool isActive)
        {
            if (isActive)
                return GetActiveColors();
            return GetInactiveColors();
        }

        private IReadOnlyFontAndColor GetActiveColors()
        {
            var colors = ActiveTabColors ?? DefaultActiveTabColors;
            if (colors is not null)
                return colors;
            Color activeColor = IsDarkBackground ? SystemColors.ControlText :
                SystemColors.ControlText;
            colors = new FontAndColor(activeColor);
            return colors;
        }

        private IReadOnlyFontAndColor GetInactiveColors()
        {
            var colors = InactiveTabColors ?? DefaultInactiveTabColors;
            Color color = IsDarkBackground ? SystemColors.GrayText :
                SystemColors.GrayText;
            colors = new FontAndColor(color);
            return colors;
        }

        private void UpdateTab(CardPanelHeaderItem item)
        {
            var isSelected = item == selectedTab;
            item.HeaderControl.IsBold = isSelected && (UseTabBold ?? DefaultUseTabBold);
            var colors = GetColors(isSelected);
            if (colors is null)
                return;
            if (UseTabForegroundColor ?? DefaultUseTabForegroundColor)
                item.HeaderControl.ForegroundColor = colors.ForegroundColor;
            if (UseTabBackgroundColor ?? DefaultUseTabBackgroundColor)
                item.HeaderControl.BackgroundColor = colors.BackgroundColor;
        }

        private void UpdateTabs()
        {
            foreach (var item in Tabs)
            {
                if (item.HeaderControl is Button button)
                    button.HasBorder = tabHasBorder ?? DefaultTabHasBorder;
                UpdateTab(item);
            }
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