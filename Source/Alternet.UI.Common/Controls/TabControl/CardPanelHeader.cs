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
    /// on the card title. Also this control is used in the <see cref="TabControl"/> as a part of its template.
    /// </remarks>
    [ControlCategory(KnownControlCategory.Other)]
    public partial class CardPanelHeader : HiddenBorder, ITextProperty
    {
        /// <summary>
        /// Gets or sets function which creates button for the <see cref="CardPanelHeader"/>.
        /// </summary>
        public static Func<SpeedButton>? CreateButton;

        /// <summary>
        /// Gets or sets default value for the tab margin.
        /// </summary>
        /// <remarks>
        /// Do not change it from 1, as current tab interior border will be painted badly.
        /// </remarks>
        public static Thickness DefaultTabMargin = 1;

        /// <summary>
        /// Gets or sets default value for the <see cref="UseRoundedCorners"/> property.
        /// </summary>
        public static bool DefaultUseRoundedCorners = true;

        /// <summary>
        /// Gets or sets default value for the tab padding.
        /// </summary>
        public static Thickness DefaultTabPadding = (6, 5, 6, 5);

        /// <summary>
        /// Gets or sets default value of the <see cref="AdditionalSpace"/> property.
        /// </summary>
        public static SizeD DefaultAdditionalSpace = new(30, 30);

        private readonly BaseCollection<CardPanelHeaderItem> tabs;

        private readonly Panel fillPanel = new()
        {
            ParentBackColor = true,
            ParentForeColor = true,
            ParentFont = true,
        };

        private readonly Panel rightPanel = new()
        {
            ParentBackColor = true,
            ParentForeColor = true,
            ParentFont = true,
        };

        private CardPanelHeaderItem? selectedTab;
        private CardPanel? cardPanel;
        private SpeedButton? closeButton;

        private Thickness? tabMargin;
        private Thickness? tabPadding;
        private bool useTabBold;
        private bool useTabForegroundColor;
        private bool useTabBackgroundColor;
        private SizeD additionalSpace;
        private bool tabHasBorder;
        private bool activeTabHasBorder;
        private IReadOnlyFontAndColor? activeTabColors;
        private IReadOnlyFontAndColor? inactiveTabColors;
        private HorizontalAlignment? tabHorizontalAlignment;
        private SpeedButton.KnownTheme tabTheme;
        private SpeedButton.KnownTheme activeTabTheme;
        private bool hasInteriorBorder;
        private TabAlignment tabAlignment;
        private bool isVerticalText;
        private ImageToText imageToText;
        private bool useRoundedCorners;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanelHeader"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public CardPanelHeader(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanelHeader"/> class.
        /// </summary>
        public CardPanelHeader()
        {
            Layout = LayoutStyle.Horizontal;
            
            fillPanel.HorizontalAlignment = HorizontalAlignment.Fill;
            fillPanel.VerticalAlignment = VerticalAlignment.Fill;
            fillPanel.Layout = LayoutStyle.Horizontal;
            rightPanel.HorizontalAlignment = HorizontalAlignment.Right;
            rightPanel.VerticalAlignment = VerticalAlignment.Fill;
            fillPanel.UserPaint = true;
            rightPanel.Parent = this;
            rightPanel.UserPaint = true;
            fillPanel.Parent = this;

            fillPanel.Paint += (s, e) =>
            {
                if (HasInteriorBorder)
                {
                    TabControl.TabHeaderInteriorDrawParams prm = new()
                    {
                        Control = this,
                        Graphics = e.Graphics,
                        Bounds = e.ClientRectangle,
                        Brush = GetInteriorBorderColor().AsBrush,
                        TabAlignment = TabsAlignment,
                        RoundCorners = UseRoundedCorners,
                    };

                    TabControl.DrawTabHeaderInterior(prm);
                }
            };

            rightPanel.Paint += (s, e) =>
            {
                if (HasInteriorBorder)
                {
                    e.Graphics.DrawBorderWithBrush(
                                GetInteriorBorderColor().AsBrush,
                                e.ClientRectangle,
                                GetRightPanelBorder(TabsAlignment));
                }
            };

            ResetAppearance(invalidate: false);
            TabStop = false;
            CanSelect = false;
            tabs = new(CollectionSecurityFlags.NoNullOrReplace);
            tabs.ItemInserted += OnTabsItemInserted;
            tabs.ItemRemoved += OnTabsItemRemoved;
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
        /// Occurs when the close button is clicked.
        /// </summary>
        /// <remarks>
        /// This event allows subscribers to handle the close button click action.
        /// </remarks>
        public event EventHandler? CloseButtonClick;

        /// <summary>
        /// Enumerates known themes for the control.
        /// </summary>
        public enum KnownTheme
        {
            /// <summary>
            /// Default theme.
            /// Visual related properties are configured so that control looks like a tab control header.
            /// </summary>
            Default,

            /// <summary>
            /// SpeedButton theme.
            /// Visual related properties are configured so that the tabs look like a speed buttons.
            /// Active tab will have a static border and other tabs will have a border on mouse hover.
            /// </summary>
            SpeedButton,

            /// <summary>
            /// PushButton theme.
            /// Visual related properties are configured so that the tabs look like a push buttons.
            /// Active tab will have a static border with accent color and other tabs will have a standard border.
            /// </summary>
            PushButton,
        }

        /// <summary>
        /// Gets or sets default value of the <see cref="ActiveTabHasBorder"/>.
        /// </summary>
        public static bool DefaultActiveTabHasBorder { get; set; } = false;

        /// <summary>
        /// Gets or sets default value of the <see cref="TabHasBorder"/>.
        /// </summary>
        public static bool DefaultTabHasBorder { get; set; } = false;

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
        /// Gets or sets the area of the control (for example, along the top) where
        /// the tabs are aligned.
        /// </summary>
        /// <value>One of the <see cref="TabsAlignment"/> values. The default is
        /// <see cref="TabAlignment.Top"/>.</value>
        public virtual TabAlignment TabsAlignment
        {
            get
            {
                return tabAlignment;
            }

            set
            {
                if (TabsAlignment == value)
                    return;

                fillPanel.DoInsideLayout(() =>
                {
                    tabAlignment = value;

                    switch(tabAlignment)
                    {
                        case TabAlignment.Top:
                            rightPanel.HorizontalAlignment = HorizontalAlignment.Right;
                            rightPanel.VerticalAlignment = VerticalAlignment.Fill;
                            break;
                        case TabAlignment.Bottom:
                            rightPanel.HorizontalAlignment = HorizontalAlignment.Right;
                            rightPanel.VerticalAlignment = VerticalAlignment.Fill;
                            break;
                        case TabAlignment.Left:
                            rightPanel.HorizontalAlignment = HorizontalAlignment.Right;
                            rightPanel.VerticalAlignment = VerticalAlignment.Bottom;
                            break;
                        case TabAlignment.Right:
                            rightPanel.HorizontalAlignment = HorizontalAlignment.Left;
                            rightPanel.VerticalAlignment = VerticalAlignment.Bottom;
                            break;
                    }

                    foreach (var tab in Tabs)
                    {
                        tab.HeaderButton.HorizontalAlignment = GetRealTabHorizontalAlignment();
                    }
                });
            }
        }

        /// <summary>
        /// Gets the "Close" button.
        /// </summary>
        /// <remarks>The <see cref="CloseButton"/> is automatically created and configured with default
        /// alignment settings. It raises the <see cref="CloseButtonClick"/> event when clicked.</remarks>
        [Browsable(false)]
        public virtual SpeedButton CloseButton
        {
            get
            {
                if (closeButton is null)
                {
                    closeButton = CreateCloseButton();
                    closeButton.HorizontalAlignment = HorizontalAlignment.Center;
                    closeButton.VerticalAlignment = VerticalAlignment.Center;

                    closeButton.Click += (s, e) =>
                    {
                        CloseButtonClick?.Invoke(this, EventArgs.Empty);
                    };

                    closeButton.Parent = rightPanel;
                }

                return closeButton;
            }
        }

        /// <summary>
        /// Gets the panel where the tabs are located.
        /// </summary>
        public HiddenBorder FillPanel => fillPanel;

        /// <summary>
        /// Gets the right panel where the <see cref="CloseButton"/> is located.
        /// </summary>
        public HiddenBorder RightPanel => rightPanel;

        /// <summary>
        /// Gets or sets a value indicating whether the close button is visible.
        /// Default value is <c>false</c>.
        /// </summary>
        public virtual bool HasCloseButton
        {
            get
            {
                if (closeButton is null || !closeButton.Visible)
                    return false;
                return closeButton.Visible;
            }

            set
            {
                if (HasCloseButton == value)
                    return;
                if (value)
                {
                    CloseButton.Visible = true;
                }
                else
                {
                    if (closeButton is not null)
                    {
                        closeButton.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the layout relationship between the image and text content
        /// within the tab (vertical or horizontal align).
        /// </summary>
        public virtual ImageToText ImageToText
        {
            get => imageToText;

            set
            {
                if (imageToText == value)
                    return;
                fillPanel.DoInsideLayout(() =>
                {
                    imageToText = value;

                    foreach (var tab in Tabs)
                    {
                        tab.HeaderButton.ImageToText = value;
                    }
                });
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text should be rendered vertically.
        /// </summary>
        /// <remarks>
        /// When this property is set, the layout is refreshed to reflect the vertical text orientation.
        /// </remarks>
        public virtual bool IsVerticalText
        {
            get
            {
                return isVerticalText;
            }

            set
            {
                if (isVerticalText == value)
                    return;

                fillPanel.DoInsideLayout(() =>
                {
                    isVerticalText = value;
                    ImageToText = value ? ImageToText.Vertical : ImageToText.Horizontal;

                    foreach (var tab in Tabs)
                    {
                        tab.HeaderButton.IsVerticalText = value;
                    }
                });
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether interior has a border with rounded corners.
        /// </summary>
        public virtual bool UseRoundedCorners
        {
            get => useRoundedCorners;
            set
            {
                if (useRoundedCorners == value)
                    return;
                useRoundedCorners = value;
                InvalidateInterior();
            }
        }

        /// <summary>
        /// Gets or sets individual tab margin.
        /// </summary>
        public virtual Thickness? TabMargin
        {
            get => tabMargin;
            set
            {
                if (tabMargin == value)
                    return;
                tabMargin = value;
                UpdateTabs();
            }
        }

        /// <summary>
        /// Gets or sets colors and styles theme of the tabs.
        /// </summary>
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
        /// Gets or sets colors and styles theme of the active tab.
        /// </summary>
        public virtual SpeedButton.KnownTheme ActiveTabTheme
        {
            get => activeTabTheme;
            set
            {
                if (activeTabTheme == value)
                    return;
                activeTabTheme = value;
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
                InvalidateInterior();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="HorizontalAlignment"/> of the tabs.
        /// </summary>
        public virtual HorizontalAlignment? TabHorizontalAlignment
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
        /// Gets or sets whether tab buttons have border.
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
        /// Gets or sets whether active tab button has border.
        /// </summary>
        public virtual bool ActiveTabHasBorder
        {
            get
            {
                return activeTabHasBorder;
            }

            set
            {
                if (activeTabHasBorder == value)
                    return;
                activeTabHasBorder = value;
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

        /// <inheritdoc/>
        public override LayoutStyle? Layout
        {
            get => base.Layout;
            
            set
            {
                base.Layout = value;
                fillPanel.Layout = value;
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
        /// Gets or sets whether tab interior border is visible.
        /// </summary>
        public virtual bool HasInteriorBorder
        {
            get => hasInteriorBorder;

            set
            {
                if (hasInteriorBorder == value)
                    return;
                hasInteriorBorder = value;
                InvalidateInterior();
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
                foreach (var tab in tabs)
                {
                    UpdateTab(tab);
                }

                if (UpdateCardsVisible) SetCardsVisible();
                CardsWidthToMax(UpdateCardsMode);
                InvalidateInterior();
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
                if (Tabs.Count == 0)
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
            fillPanel.DoInsideLayout(() =>
            {
                item.HeaderButton.Parent = null;
                tabs.RemoveAt(index!.Value);
            });
            InvalidateInterior();
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
        public virtual int Insert(int? index, string? text, AbstractControl? cardControl = null)
        {
            var button = CreateHeaderButton();

            if (button.MinimumSize.IsEmpty)
                button.MinimumSize = TabControl.DefaultMinTabSize;

            button.Text = text ?? cardControl?.Title ?? string.Empty;
            button.SizeChanged += OnButtonSizeChanged;
            button.Margin = TabMargin ?? DefaultTabMargin;
            button.Padding = TabPadding ?? DefaultTabPadding;
            button.HasBorder = TabHasBorder;
            button.VerticalAlignment = UI.VerticalAlignment.Center;
            button.SetContentHorizontalAlignment(HorizontalAlignment.Left);
            Children.Insert(index ?? Children.Count, button);
            button.Parent = fillPanel;
            button.Click += OnItemClick;

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
        public virtual int Add(string? text, ObjectUniqueId cardId)
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
        public virtual int Insert(int? index, string? text, ObjectUniqueId cardId)
        {
            var realIndex = Insert(index, text);
            var item = tabs[realIndex];
            item.CardUniqueId = cardId;
            return realIndex;
        }

        /// <summary>
        /// Assigns visual related properties so that the tabs look like a speed buttons.
        /// Active tab will have a static border and other tabs will have a border on mouse hover.
        /// </summary>
        public virtual void ApplySpeedButtonTheme()
        {
            ResetAppearance(invalidate: false);

            tabTheme = SpeedButton.KnownTheme.Default;
            activeTabTheme = SpeedButton.KnownTheme.StaticBorder;
            activeTabHasBorder = true;
            tabHasBorder = true;
            hasInteriorBorder = false;

            UpdateTabs();
        }

        /// <summary>
        /// Assigns visual related properties so that control looks like a tab control header.
        /// </summary>
        public virtual void ApplyDefaultTheme()
        {
            ResetAppearance(invalidate: true);
        }

        /// <summary>
        /// Assigns visual related properties so that the item looks like a standard buttons.
        /// Active tab will have a static border with accent color and other tabs will have a standard border.
        /// </summary>
        public virtual void ApplyPushButtonTheme()
        {
            ResetAppearance(invalidate: false);

            tabTheme = SpeedButton.KnownTheme.StaticBorder;
            activeTabTheme = SpeedButton.KnownTheme.CheckBorder;
            activeTabHasBorder = true;
            tabHasBorder = true;
            hasInteriorBorder = false;

            UpdateTabs();
        }

        /// <summary>
        /// Invalidates the control and all tabs.
        /// </summary>
        public virtual void InvalidateInterior()
        {
            Invalidate();
        }

        /// <summary>
        /// Applies known theme to the control.
        /// </summary>
        /// <param name="theme">The known theme to apply.</param>
        public virtual void ApplyKnownTheme(KnownTheme theme)
        {
            switch (theme)
            {
                case KnownTheme.Default:
                    ApplyDefaultTheme();
                    break;
                case KnownTheme.SpeedButton:
                    ApplySpeedButtonTheme();
                    break;
                case KnownTheme.PushButton:
                    ApplyPushButtonTheme();
                    break;
            }
        }

        /// <summary>
        /// Resets appearance of the control to the initial state.
        /// All appearance related properties are set to their default values as it was when the control was created.
        /// </summary>
        public virtual void ResetAppearance(bool invalidate = true)
        {
            tabMargin = null;
            tabPadding = null;
            useTabBold = DefaultUseTabBold;
            useTabForegroundColor = DefaultUseTabForegroundColor;
            useTabBackgroundColor = DefaultUseTabBackgroundColor;
            additionalSpace = DefaultAdditionalSpace;
            tabHasBorder = DefaultTabHasBorder;
            activeTabHasBorder = DefaultActiveTabHasBorder;
            activeTabColors = null;
            inactiveTabColors = null;
            tabHorizontalAlignment = null;
            tabTheme = SpeedButton.KnownTheme.TabControl;
            activeTabTheme = SpeedButton.KnownTheme.TabControl;
            hasInteriorBorder = true;
            tabAlignment = TabAlignment.Top;
            isVerticalText = false;
            imageToText = ImageToText.Horizontal;
            useRoundedCorners = DefaultUseRoundedCorners;

            ParentBackColor = true;
            ParentForeColor = true;
            fillPanel.Layout = LayoutStyle.Horizontal;
            Layout = LayoutStyle.Horizontal;

            if (invalidate)
            {
                UpdateTabs();
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
        public virtual int Add(string? text, AbstractControl? cardControl = null)
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
            return result;

            static SpeedButton Fn()
            {
                var result = new SpeedButton();
                result.TextVisible = true;
                return result;
            }
        }

        /// <summary>
        /// Gets the border width of the right panel.
        /// </summary>
        /// <param name="tabAlignment">The alignment of the tabs.</param>
        /// <returns>The thickness of the right panel border.</returns>
        public virtual Thickness GetRightPanelBorder(TabAlignment tabAlignment)
        {
            switch (tabAlignment)
            {
                case TabAlignment.Top:
                    return new Thickness(0, 0, 0, 1);
                case TabAlignment.Bottom:
                    return new Thickness(0, 1, 0, 0);
                case TabAlignment.Left:
                    return new Thickness(0, 0, 1, 0);
                case TabAlignment.Right:
                    return new Thickness(1, 0, 0, 0);
                default:
                    return Thickness.Empty;
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

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);
        }

        /// <summary>
        /// Gets real tab horizontal alignment which depends on <see cref="TabHorizontalAlignment"/>
        /// and <see cref="TabsAlignment"/>.
        /// </summary>
        /// <returns></returns>
        protected HorizontalAlignment GetRealTabHorizontalAlignment()
        {
            if (TabHorizontalAlignment is not null)
                return TabHorizontalAlignment.Value;

            var isLeft = tabAlignment == TabAlignment.Top || tabAlignment == TabAlignment.Bottom;
            return isLeft ? HorizontalAlignment.Left : HorizontalAlignment.Stretch;
        }

        /// <summary>
        /// Gets interior border color.
        /// </summary>
        /// <returns></returns>
        protected virtual Color GetInteriorBorderColor()
        {
            var color = Borders?.GetObjectOrNull(VisualControlState.Normal)?.Color;
            color ??= ColorUtils.GetTabControlInteriorBorderColor(IsDarkBackground);
            return color;
        }

        /// <summary>
        /// Called when size of the tab button is changed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event parameters.</param>
        protected virtual void OnButtonSizeChanged(object? sender, EventArgs e)
        {
            if (sender is AbstractControl control)
                ButtonSizeChanged?.Invoke(this, new BaseEventArgs<AbstractControl>(control));
        }

        /// <summary>
        /// Gets colors of the active or inactive tab.
        /// </summary>
        /// <param name="isActive">Whether to get colors for the active or inactive item.</param>
        /// <returns></returns>
        protected virtual IReadOnlyFontAndColor GetColors(bool isActive)
        {
            if (isActive)
                return GetActiveColors();
            return GetInactiveColors();
        }

        /// <summary>
        /// Gets colors of the active tab.
        /// </summary>
        /// <returns></returns>
        protected virtual IReadOnlyFontAndColor GetActiveColors()
        {
            var colors = ActiveTabColors ?? DefaultActiveTabColors;
            if (colors is not null)
                return colors;
            Color activeColor = IsDarkBackground ? SystemColors.ControlText :
                SystemColors.ControlText;
            colors = new FontAndColor(activeColor);
            return colors;
        }

        /// <summary>
        /// Gets colors of the inactive tab.
        /// </summary>
        /// <returns></returns>
        protected virtual IReadOnlyFontAndColor GetInactiveColors()
        {
            var colors = InactiveTabColors ?? DefaultInactiveTabColors;
            if (colors is not null)
                return colors;
            Color color = IsDarkBackground ? SystemColors.GrayText :
                SystemColors.GrayText;
            colors = new FontAndColor(color);
            return colors;
        }

        /// <summary>
        /// Updates tab properties.
        /// </summary>
        /// <param name="item">The item which properties to update.</param>
        protected virtual void UpdateTab(CardPanelHeaderItem item)
        {
            if (item is null)
                return;

            var canForeground = true;
            var canBackground = true;
            var useBold = !canForeground && !canBackground;

            var isSelected = item == selectedTab;

            if (UseTabBold || useBold)
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
            item.HeaderButton.HorizontalAlignment = GetRealTabHorizontalAlignment();
            item.HeaderButton.IsVerticalText = IsVerticalText;
            item.HeaderButton.ImageToText = ImageToText;

            if (isSelected)
            {
                item.HeaderButton.UseTheme = activeTabTheme;
            }
            else
            {
                item.HeaderButton.UseTheme = tabTheme;
            }
        }

        /// <summary>
        /// Updates all tabs properties and refreshes the control.
        /// </summary>
        protected virtual void UpdateTabs()
        {
            DoInsideLayout(Fn);

            void Fn()
            {
                foreach (var item in Tabs)
                {
                    var isSelected = item == selectedTab;

                    if (isSelected)
                    {
                        item.HeaderButton.HasBorder = activeTabHasBorder;
                    }
                    else
                    {
                        item.HeaderButton.HasBorder = tabHasBorder;
                    }

                    UpdateTab(item);
                }
            }

            InvalidateInterior();
        }

        /// <summary>
        /// Called when tab is removed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="index">The index of the tab.</param>
        /// <param name="item">The tab which is removed.</param>
        protected virtual void OnTabsItemRemoved(object? sender, int index, CardPanelHeaderItem item)
        {
        }

        /// <summary>
        /// Called when tab is inserted.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="index">The index of the tab.</param>
        /// <param name="item">The tab which is inserted.</param>
        protected virtual void OnTabsItemInserted(object? sender, int index, CardPanelHeaderItem item)
        {
        }

        /// <summary>
        /// Creates a new instance of a close button with default settings.
        /// </summary>
        /// <remarks>This method initializes a <see cref="SpeedButton"/> configured as a close button.
        /// Subclasses can override this method to customize the creation of the close button.</remarks>
        /// <returns>A <see cref="SpeedButton"/> instance representing a close button.</returns>
        protected virtual SpeedButton CreateCloseButton()
        {
            SpeedButton result = new();
            result.SetSvgImage(null, KnownButton.Close);
            return result;
        }

        /// <summary>
        /// Called when tab is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnItemClick(object? sender, EventArgs e)
        {
            if (sender is not AbstractControl control)
                return;

            if (BeforeTabClick is not null)
            {
                BaseCancelEventArgs beforeArgs = new();
                BeforeTabClick(this, beforeArgs);
                if (beforeArgs.Cancel)
                    return;
            }

            foreach (var tab in tabs)
            {
                if (control == tab.HeaderButton || control.HasIndirectParent(tab.HeaderButton))
                {
                    var oldSelectedTab = SelectedTab;
                    SelectedTab = tab;
                    if (oldSelectedTab != SelectedTab)
                        TabClick?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
    }
}