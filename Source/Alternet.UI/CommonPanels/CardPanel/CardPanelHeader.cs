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
    [ControlCategory("Panels")]
    public class CardPanelHeader : Control, ITextProperty
    {
        /// <summary>
        /// Gets or sets function which creates button for the <see cref="CardPanelHeader"/>.
        /// </summary>
        public static Func<SpeedButton>? CreateButton;

        /// <summary>
        /// Gets or sets default border side width.
        /// </summary>
        /// <remarks>
        /// <see cref="DefaultBorderWidth"/> by default is calculated dynamically using this field.
        /// </remarks>
        public static double DefaultBorderSideWidth = 1;

        /// <summary>
        /// Gets or sets default value of the <see cref="BorderPadding"/> property.
        /// </summary>
        public static Thickness DefaultBorderPadding = 3;

        /// <summary>
        /// Gets or sets default value of the <see cref="BorderMargin"/> property.
        /// </summary>
        public static Thickness DefaultBorderMargin = new (0, 0, 0, 5);

        /// <summary>
        /// Gets or sets default value for the tab margin.
        /// </summary>
        public static Thickness DefaultTabMargin = 1;

        /// <summary>
        /// Gets or sets default value of the <see cref="AdditionalSpace"/> property.
        /// </summary>
        public static SizeD DefaultAdditionalSpace = new(30, 30);

        internal static Color DefaultUnderlineColorLight = Color.FromRgb(0, 80, 197);

        private static Thickness? defaultBorderWidth;

        private readonly Collection<CardPanelHeaderItem> tabs = [];
        private readonly StackPanel stackPanel = new()
        {
            Orientation = StackPanelOrientation.Horizontal,
        };

        private readonly Border control = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch, // do not change, horizontal line must be on full width
            VerticalAlignment = VerticalAlignment.Top,
            BorderWidth = DefaultBorderWidth,
            Padding = DefaultBorderPadding,
            Margin = DefaultBorderMargin,
        };

        private bool useTabBold = DefaultUseTabBold;
        private bool useTabForegroundColor = DefaultUseTabForegroundColor;
        private bool useTabBackgroundColor = DefaultUseTabBackgroundColor;
        private SizeD additionalSpace = DefaultAdditionalSpace;
        private CardPanelHeaderItem? selectedTab;
        private bool tabHasBorder = DefaultTabHasBorder;
        private CardPanel? cardPanel;
        private IReadOnlyFontAndColor? activeTabColors;
        private IReadOnlyFontAndColor? inactiveTabColors;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanelHeader"/> class.
        /// </summary>
        public CardPanelHeader()
        {
            tabs.ThrowOnNullAdd = true;
            tabs.ItemInserted += Tabs_ItemInserted;
            tabs.ItemRemoved += Tabs_ItemRemoved;
            control.Parent = this;
            stackPanel.Parent = control;
        }

        /// <summary>
        /// Occurs when the tab is clicked.
        /// </summary>
        public event EventHandler? TabClick;

        /// <summary>
        /// Gets or sets default value of the border width.
        /// </summary>
        public static Thickness DefaultBorderWidth
        {
            get
            {
                if (defaultBorderWidth is null)
                    return new(0, 0, 0, DefaultBorderSideWidth);
                return defaultBorderWidth.Value;
            }

            set
            {
                defaultBorderWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets default value of the <see cref="TabHasBorder"/>.
        /// </summary>
        public static bool DefaultTabHasBorder { get; set; } = true;

        /// <summary>
        /// Gets or sets default value for the <see cref="UseTabBold"/> property.
        /// </summary>
        public static bool DefaultUseTabBold { get; internal set; } = false;

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
        /// Gets or sets text of the first tab.
        /// </summary>
        [Browsable(false)]
        public override string Text
        {
            get
            {
                if (Tabs.Count == 0)
                    return string.Empty;
                return Tabs[0].HeaderButton.Text;
            }

            set
            {
                if (Tabs.Count == 0)
                    Add(value);
                else
                    Tabs[0].HeaderButton.Text = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="HorizontalAlignment"/> of the tabs.
        /// </summary>
        public HorizontalAlignment TabHorizontalAlignment
        {
            get
            {
                return stackPanel.HorizontalAlignment;
            }

            set
            {
                stackPanel.HorizontalAlignment = value;
            }
        }

        /// <inheritdoc cref="Border.BorderWidth"/>
        public Thickness BorderWidth
        {
            get
            {
                return control.BorderWidth;
            }

            set
            {
                control.BorderWidth = value;
                PerformLayout();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets border padding.
        /// </summary>
        public Thickness BorderPadding
        {
            get
            {
                return control.Padding;
            }

            set
            {
                control.Padding = value;
                PerformLayout();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets border margin.
        /// </summary>
        public Thickness BorderMargin
        {
            get
            {
                return control.Margin;
            }

            set
            {
                control.Margin = value;
                PerformLayout();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="HorizontalAlignment"/> of the tab group.
        /// </summary>
        public HorizontalAlignment TabGroupHorizontalAlignment
        {
            get
            {
                return control.HorizontalAlignment;
            }

            set
            {
                control.HorizontalAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="VerticalAlignment"/> of the tabs.
        /// </summary>
        public VerticalAlignment TabVerticalAlignment
        {
            get
            {
                return stackPanel.VerticalAlignment;
            }

            set
            {
                stackPanel.VerticalAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="VerticalAlignment"/> of the tab group.
        /// </summary>
        public VerticalAlignment TabGroupVerticalAlignment
        {
            get
            {
                return control.VerticalAlignment;
            }

            set
            {
                control.VerticalAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the dimension by which header
        /// buttons are stacked.
        /// </summary>
        public StackPanelOrientation Orientation
        {
            get => stackPanel.Orientation;

            set => stackPanel.Orientation = value;
        }

        /// <summary>
        /// Gets whether to set bold style for the title of active tab.
        /// </summary>
        public bool UseTabBold
        {
            get
            {
                return useTabBold;
            }

            internal set
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
        public bool UseTabForegroundColor
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
        public bool UseTabBackgroundColor
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
        [Browsable(false)]
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
        [Browsable(false)]
        public IReadOnlyList<CardPanelHeaderItem> Tabs => tabs;

        /// <summary>
        /// Gets or sets whether tabs have border.
        /// </summary>
        public bool TabHasBorder
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        /// Gets or sets background color of the active tab.
        /// </summary>
        public Color? BackgroundColorActiveTab
        {
            get
            {
                return ActiveTabColors?.BackgroundColor;
            }

            set
            {
                FontAndColor.ChangeColor(ref activeTabColors, value, true, UpdateTabs);
            }
        }

        /// <summary>
        /// Gets or sets background color of the inactive tab.
        /// </summary>
        public Color? BackgroundColorInactiveTab
        {
            get
            {
                return InactiveTabColors?.BackgroundColor;
            }

            set
            {
                FontAndColor.ChangeColor(ref inactiveTabColors, value, true, UpdateTabs);
            }
        }

        /// <summary>
        /// Gets or sets foreground color of the active tab.
        /// </summary>
        public Color? ForegroundColorActiveTab
        {
            get
            {
                return ActiveTabColors?.ForegroundColor;
            }

            set
            {
                FontAndColor.ChangeColor(ref activeTabColors, value, false, UpdateTabs);
            }
        }

        /// <summary>
        /// Gets or sets foreground color of the inactive tab.
        /// </summary>
        public Color? ForegroundColorInactiveTab
        {
            get
            {
                return InactiveTabColors?.ForegroundColor;
            }

            set
            {
                FontAndColor.ChangeColor(ref inactiveTabColors, value, false, UpdateTabs);
            }
        }

        /// <summary>
        /// Gets or sets size of the additional space which is added when
        /// <see cref="UpdateCardsMode"/> is not <see cref="WindowSizeToContentMode.None"/>.
        /// </summary>
        [Browsable(false)]
        public SizeD AdditionalSpace
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

        /// <inheritdoc/>
        public override Color? BackgroundColor
        {
            get => base.BackgroundColor;

            set
            {
                base.BackgroundColor = value;
                control.BackgroundColor = value;
            }
        }

        /// <inheritdoc/>
        public override Color? ForegroundColor
        {
            get => base.ForegroundColor;

            set
            {
                base.ForegroundColor = value;
                control.ForegroundColor = value;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override bool IsBold
        {
            get => false;

            set
            {
            }
        }

        /// <summary>
        /// Gets selected tab.
        /// </summary>
        [Browsable(false)]
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

                if (UpdateCardsVisible) SetCardsVisible();
                CardsWidthToMax(UpdateCardsMode);
            }
        }

        /// <summary>
        /// Gets or sets selected tab index.
        /// </summary>
        /// <remarks>
        /// Returns <c>null</c> if no tab is selected.
        /// </remarks>
        [Browsable(false)]
        public int? SelectedTabIndex
        {
            get
            {
                if (selectedTab == null)
                    return null;
                var result = tabs.IndexOfOrNull(selectedTab);
                return result;
            }

            set
            {
                if (value >= Tabs.Count || value < 0 || value is null)
                    return;
                SelectedTab = Tabs[value.Value];
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

            cardPanel?.SuspendLayout();
            try
            {
                if (id is null)
                    cardPanel?.Hide();

                SelectedTab?.CardControl?.Show();

                if (id is not null)
                {
                    cardPanel?.Show();
                    cardPanel?.SelectCard(id);
                }
            }
            finally
            {
                cardPanel?.ResumeLayout();
            }
        }

        /// <summary>
        /// Calculates maximal width of cards.
        /// </summary>
        public virtual SizeD GetMaxCardSize()
        {
            return SizeD.MaxWidthHeight(GetCardSizes());
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
        /// Selects the last tab if it exists.
        /// </summary>
        public virtual void SelectLastTab()
        {
            if (Tabs.Count > 0)
                SelectedTab = Tabs[Tabs.Count - 1];
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
                    update = SizeD.AnyIsNaN(parent.SuggestedSize);
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
                        parent.SuggestedSize = new SizeD(newWidth, newHeight);
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
        /// <returns>
        /// Created item index.
        /// </returns>
        public virtual int Add(string text, Control? cardControl = null)
        {
            var control = CreateHeaderButton();

            control.Text = text;
            control.Margin = DefaultTabMargin;
            control.HasBorder = TabHasBorder;
            control.HorizontalAlignment = HorizontalAlignment.Center;
            control.Parent = stackPanel;
            control.Click += Item_Click;

            var item = new CardPanelHeaderItem(control)
            {
                CardControl = cardControl,
            };

            tabs.Add(item);

            UpdateTab(item);

            return tabs.Count - 1;
        }

        /// <summary>
        /// Creates header button control inherited from <see cref="SpeedButton"/>.
        /// </summary>
        /// <remarks>
        /// By default creates <see cref="SpeedButton"/>
        /// </remarks>
        public virtual SpeedButton CreateHeaderButton()
        {
            return CreateButton?.Invoke() ?? Fn();

            static SpeedButton Fn()
            {
                var result = new SpeedButton();
                result.TextVisible = true;
                return result;
            }
        }

        /// <summary>
        /// Adds new item to the control.
        /// </summary>
        /// <param name="text">Item title.</param>
        /// <param name="cardId">Associated card unique id.</param>
        /// <returns>
        /// Created item index.
        /// </returns>
        public virtual int Add(string text, ObjectUniqueId cardId)
        {
            var index = Add(text);
            var item = tabs[index];
            item.CardUniqueId = cardId;
            return index;
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
        public virtual SizeD[] GetCardSizes()
        {
            SizeD[] result = new SizeD[tabs.Count];
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
            if (colors is not null)
                return colors;
            Color color = IsDarkBackground ? SystemColors.GrayText :
                SystemColors.GrayText;
            colors = new FontAndColor(color);
            return colors;
        }

        private void UpdateTab(CardPanelHeaderItem item)
        {
            var platform = AllPlatformDefaults.PlatformCurrent;
            var canForeground = platform.AllowButtonForeground;
            var canBackground = platform.AllowButtonBackground;
            var useBold = !canForeground && !canBackground;

            var isSelected = item == selectedTab;

            if(UseTabBold || useBold)
            {
                item.HeaderButton.IsBold = isSelected;
            }

            var colors = GetColors(isSelected);
            if (colors is null)
                return;
            if (UseTabForegroundColor && canForeground)
            {
                item.HeaderButton.ForegroundColor = colors.ForegroundColor;
            }

            if (UseTabBackgroundColor && canBackground)
            {
                item.HeaderButton.BackgroundColor = colors.BackgroundColor;
            }
        }

        private void UpdateTabs()
        {
            foreach (var item in Tabs)
            {
                item.HeaderButton.HasBorder = tabHasBorder;
                UpdateTab(item);
            }

            PerformLayout();
            Refresh();
        }

        private void Tabs_ItemRemoved(object? sender, int index, CardPanelHeaderItem item)
        {
        }

        private void Tabs_ItemInserted(object? sender, int index, CardPanelHeaderItem item)
        {
        }

        private void Item_Click(object? sender, EventArgs e)
        {
            if (sender is not Control control)
                return;
            foreach(var tab in tabs)
            {
                if(control == tab.HeaderButton || control.HasIndirectParent(tab.HeaderButton))
                {
                    var oldSelectedTab = SelectedTab;
                    SelectedTab = tab;
                    if(oldSelectedTab != SelectedTab)
                        TabClick?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
    }
}