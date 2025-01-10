﻿using System;
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
    internal partial class CardPanelHeader : Control, ITextProperty
    {
        /// <summary>
        /// Gets or sets function which creates button for the <see cref="CardPanelHeader"/>.
        /// </summary>
        public static Func<SpeedButton>? CreateButton = null;

        /// <summary>
        /// Gets or sets default value for the tab margin.
        /// </summary>
        // Do not change it from 1, as current tab interior border will be painted badly.
        public static Thickness DefaultTabMargin = 1;

        /// <summary>
        /// Gets or sets default value for the tab padding.
        /// </summary>
        public static Thickness DefaultTabPadding = (6, 5, 6, 5);

        /// <summary>
        /// Gets or sets default value of the <see cref="AdditionalSpace"/> property.
        /// </summary>
        public static SizeD DefaultAdditionalSpace = new(30, 30);

        internal static Color DefaultUnderlineColorLight = Color.FromRgb(0, 80, 197);

        private readonly Collection<CardPanelHeaderItem> tabs = new();

        private Thickness? tabMargin;
        private Thickness? tabPadding;
        private bool useTabBold = DefaultUseTabBold;
        private bool useTabForegroundColor = DefaultUseTabForegroundColor;
        private bool useTabBackgroundColor = DefaultUseTabBackgroundColor;
        private SizeD additionalSpace = DefaultAdditionalSpace;
        private CardPanelHeaderItem? selectedTab;
        private bool tabHasBorder = DefaultTabHasBorder;
        private CardPanel? cardPanel;
        private IReadOnlyFontAndColor? activeTabColors;
        private IReadOnlyFontAndColor? inactiveTabColors;
        private HorizontalAlignment tabHorizontalAlignment = HorizontalAlignment.Left;
        private SpeedButton.KnownTheme tabTheme = SpeedButton.KnownTheme.TabControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanelHeader"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public CardPanelHeader(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanelHeader"/> class.
        /// </summary>
        public CardPanelHeader()
        {
            ParentBackColor = true;
            ParentForeColor = true;
            Layout = LayoutStyle.Horizontal;
            tabs.ThrowOnNullAdd = true;
            tabs.ItemInserted += Tabs_ItemInserted;
            tabs.ItemRemoved += Tabs_ItemRemoved;
        }

        /// <summary>
        /// Occurs when size of any button is changed.
        /// </summary>
        public event EventHandler<BaseEventArgs<AbstractControl>>? ButtonSizeChanged;

        /// <summary>
        /// Occurs before the tab is clicked.
        /// </summary>
        public event EventHandler<BaseCancelEventArgs>? BeforeTabClick;

        /// <summary>
        /// Occurs when the tab is clicked.
        /// </summary>
        public event EventHandler? TabClick;

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
        /// Gets or sets individual tab margin.
        /// </summary>
        public virtual Thickness? TabMargin
        {
            get => tabMargin;
            set
            {
                if(tabMargin == value)
                    return;
                tabMargin = value;
                UpdateTabs();
            }
        }

        /// <summary>
        /// Gets or sets colors and styles theme of the tabs.
        /// </summary>
        [Browsable(false)]
        public virtual SpeedButton.KnownTheme TabTheme
        {
            get => tabTheme;
            set
            {
                if (tabTheme == value)
                    return;
                tabTheme = value;
                UpdateTabs();
            }
        }

        /// <summary>
        /// Gets or sets individual tab padding.
        /// </summary>
        public virtual Thickness? TabPadding
        {
            get => tabPadding;
            set
            {
                if (tabPadding == value)
                    return;
                tabPadding = value;
                UpdateTabs();
            }
        }

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
        public virtual HorizontalAlignment TabHorizontalAlignment
        {
            get
            {
                return tabHorizontalAlignment;
            }

            set
            {
                if (tabHorizontalAlignment == value)
                    return;
                tabHorizontalAlignment = value;
                UpdateTabs();
            }
        }

        /// <summary>
        /// Gets whether to set bold style for the title of active tab.
        /// </summary>
        public virtual bool UseTabBold
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
        public virtual bool UseTabForegroundColor
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
        public virtual bool UseTabBackgroundColor
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
        public virtual CardPanel? CardPanel
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
        public virtual bool TabHasBorder
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
        public virtual IReadOnlyFontAndColor? ActiveTabColors
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
        public virtual IReadOnlyFontAndColor? InactiveTabColors
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
        public virtual Color? BackgroundColorActiveTab
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
        public virtual Color? BackgroundColorInactiveTab
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
        public virtual Color? ForegroundColorActiveTab
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
        public virtual Color? ForegroundColorInactiveTab
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
        public virtual SizeD AdditionalSpace
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
        /// Gets or sets whether to update size of the cards when selected tab is changed.
        /// </summary>
        public WindowSizeToContentMode UpdateCardsMode { get; set; } =
            WindowSizeToContentMode.WidthAndHeight;

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
        public virtual CardPanelHeaderItem? SelectedTab
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
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets selected tab index.
        /// </summary>
        /// <remarks>
        /// Returns <c>null</c> if no tab is selected.
        /// </remarks>
        [Browsable(false)]
        public virtual int? SelectedTabIndex
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
                if(Tabs.Count == 0)
                {
                    SelectedTab = null;
                    return;
                }

                var newValue = value ?? 0;
                newValue = MathUtils.ApplyMinMax(newValue, 0, Tabs.Count - 1);
                SelectedTab = Tabs[newValue];
            }
        }

        /// <summary>
        /// Updates visibility of the cards.
        /// </summary>
        public virtual void SetCardsVisible()
        {
            if (tabs.Count == 0)
                return;

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
            return SizeD.MaxWidthHeights(GetCardSizes());
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
        /// Gets item with the specified index.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns></returns>
        public virtual CardPanelHeaderItem? GetTab(int? index)
        {
            if (index is null || index < 0 || index > tabs.Count)
                return null;
            var result = Tabs[index.Value];
            return result;
        }

        /// <summary>
        /// Removes item at the specified index.
        /// </summary>
        /// <param name="index">Index of the item</param>
        /// <returns></returns>
        public virtual bool RemoveAt(int? index)
        {
            var item = GetTab(index);
            if (item is null)
                return false;
            DoInsideLayout(() =>
            {
                item.HeaderButton.Parent = null;
                tabs.RemoveAt(index!.Value);
            });
            Invalidate();
            return true;
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
            if (mode == WindowSizeToContentMode.None || tabs.Count == 0)
                return;

            var parent = GetCardsParent();

            if (parent is null)
                return;

            var update = false;

            switch (mode)
            {
                case WindowSizeToContentMode.Width:
                    update = Coord.IsNaN(parent.SuggestedWidth);
                    break;
                case WindowSizeToContentMode.Height:
                    update = Coord.IsNaN(parent.SuggestedHeight);
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
                    maxSize.Height + Bounds.Height + Margin.Vertical + additionalSpace.Height,
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
        /// Gets item with the specified control.
        /// </summary>
        /// <param name="control">Control attached to the item.</param>
        public int? IndexOfCardControl(AbstractControl? control)
        {
            if (control is null)
                return null;
            for (int i = 0; i < Tabs.Count; i++)
            {
                if (Tabs[i].CardControl == control)
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Inserts new item to the control at the specified position.
        /// </summary>
        /// <param name="index">Item position.</param>
        /// <param name="text">Item title.</param>
        /// <param name="cardControl">Associated card control.</param>
        /// <returns>
        /// Created item index.
        /// </returns>
        public virtual int Insert(int? index, string text, AbstractControl? cardControl = null)
        {
            var button = CreateHeaderButton();

            button.Text = text;
            button.SizeChanged += Button_SizeChanged;
            button.Margin = TabMargin ?? DefaultTabMargin;
            button.Padding = TabPadding ?? DefaultTabPadding;
            button.HasBorder = TabHasBorder;
            button.HorizontalAlignment = UI.HorizontalAlignment.Center;
            Children.Insert(index ?? Children.Count, button);
            button.Parent = this;
            button.Click += Item_Click;

            var item = new CardPanelHeaderItem(button)
            {
                CardControl = cardControl,
            };

            if (index is null)
                tabs.Add(item);
            else
                tabs.Insert(index.Value, item);

            UpdateTab(item);

            if (index is null)
                return tabs.Count - 1;
            else
                return index.Value;
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
            return Insert(null, text, cardId);
        }

        /// <summary>
        /// Inserts new item to the control at the specified position.
        /// </summary>
        /// <param name="index">Item position.</param>
        /// <param name="text">Item title.</param>
        /// <param name="cardId">Associated card unique id.</param>
        /// <returns>
        /// Created item index.
        /// </returns>
        public virtual int Insert(int? index, string text, ObjectUniqueId cardId)
        {
            var realIndex = Insert(index, text);
            var item = tabs[realIndex];
            item.CardUniqueId = cardId;
            return realIndex;
        }

        /// <summary>
        /// Adds new item to the control.
        /// </summary>
        /// <param name="text">Item title.</param>
        /// <param name="cardControl">Associated card control.</param>
        /// <returns>
        /// Created item index.
        /// </returns>
        public virtual int Add(string text, AbstractControl? cardControl = null)
        {
            return Insert(null, text, cardControl);
        }

        /// <summary>
        /// Creates header button control inherited from <see cref="SpeedButton"/>.
        /// </summary>
        /// <remarks>
        /// By default creates <see cref="SpeedButton"/>
        /// </remarks>
        public virtual SpeedButton CreateHeaderButton()
        {
            var result = CreateButton?.Invoke() ?? Fn();
            if (result.MinimumSize.IsEmpty)
                result.MinimumSize = TabControl.DefaultMinTabSize;
            return result;

            static SpeedButton Fn()
            {
                var result = new SpeedButton();
                result.TextVisible = true;
                return result;
            }
        }

        /// <summary>
        /// Gets parent control of the cards.
        /// </summary>
        public virtual AbstractControl? GetCardsParent()
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

        private void Button_SizeChanged(object? sender, EventArgs e)
        {
            if(sender is AbstractControl control)
                ButtonSizeChanged?.Invoke(this, new BaseEventArgs<AbstractControl>(control));
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
            if (item is null)
                return;

            var canForeground = true;
            var canBackground = true;
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

            item.HeaderButton.Margin = TabMargin ?? DefaultTabMargin;
            item.HeaderButton.Padding = TabPadding ?? DefaultTabPadding;
            item.HeaderButton.HorizontalAlignment = TabHorizontalAlignment;
            item.HeaderButton.UseTheme = tabTheme;
        }

        private void UpdateTabs()
        {
            DoInsideLayout(Fn);

            void Fn()
            {
                foreach (var item in Tabs)
                {
                    item.HeaderButton.HasBorder = tabHasBorder;
                    UpdateTab(item);
                }
            }

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
            if (sender is not AbstractControl control)
                return;

            if(BeforeTabClick is not null)
            {
                BaseCancelEventArgs beforeArgs = new();
                BeforeTabClick(this, beforeArgs);
                if (beforeArgs.Cancel)
                    return;
            }

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