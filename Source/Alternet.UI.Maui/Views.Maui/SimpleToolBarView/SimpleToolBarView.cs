using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a simple toolbar view with speed buttons and other controls.
    /// </summary>
    public partial class SimpleToolBarView : BaseContentView, Alternet.UI.IRaiseSystemColorsChanged
    {
        /// <summary>
        /// Represents the default padding applied to button containers.
        /// </summary>
        /// <remarks>This value is used to define the spacing around button containers in the user
        /// interface. It can be adjusted to customize the layout or appearance of buttons.</remarks>
        public static int DefaultButtonContainerPadding = 2;

        /// <summary>
        /// Represents the default padding applied to button.
        /// </summary>
        /// <remarks>This value is used to define the spacing around button contents in the user
        /// interface. It can be adjusted to customize the layout or appearance of buttons.</remarks>
        public static int DefaultButtonPadding = 5;

        /// <summary>
        /// Gets or sets the default width of the toolbar border.
        /// </summary>
        public static int DefaultBorderWidth = 1;

        /// <summary>
        /// Gets or sets the default width of the toolbar separator.
        /// </summary>
        public static int DefaultSeparatorWidth = 1;

        /// <summary>
        /// Gets or sets the default margin of the toolbar separator.
        /// </summary>
        public static Thickness DefaultSeparatorMargin = new(2, 4);

        /// <summary>
        /// Gets or sets the default toolbar separator color in dark theme.
        /// </summary>
        public static Color DefaultSeparatorColorDark = Color.FromRgb(61, 61, 61);

        /// <summary>
        /// Gets or sets the default toolbar separator color in light theme.
        /// </summary>
        public static Color DefaultSeparatorColorLight = Color.FromRgb(204, 206, 219);

        /// <summary>
        /// Gets or sets the default underline color for sticky buttons in dark theme.
        /// </summary>
        public static Color DefaultStickyUnderlineColorDark = Color.FromRgb(76, 194, 255);

        /// <summary>
        /// Gets or sets the default underline color for sticky buttons in light theme.
        /// </summary>
        public static Color DefaultStickyUnderlineColorLight = Color.FromRgb(0, 120, 212);

        /// <summary>
        /// Gets or sets the default size of the sticky button underline.
        /// </summary>
        public static Size DefaultStickyUnderlineSize = new(16, 3);

        /// <summary>
        /// Gets or sets the default margin of the sticky button underline.
        /// </summary>
        public static Thickness DefaultStickyUnderlineMargin = new (0, 5, 0, 0);

        /// <summary>
        /// Gets or sets the default margin for buttons.
        /// </summary>
        public static int DefaultButtonMargin = 1;

        /// <summary>
        /// Gets ot sets default size of the button image on mobile platform.
        /// </summary>
        public static int DefaultImageButtonSize = 24;

        /// <summary>
        /// Gets ot sets default size of the button image on desktop platform.
        /// </summary>
        public static int DefaultImageButtonSizeDesktop = 16;

        /// <summary>
        /// Gets ot sets default text color for dark theme.
        /// </summary>
        public static Color DefaultTextColorDark = Color.FromRgb(214, 214, 214);

        /// <summary>
        /// Gets ot sets default disabled text color for dark theme.
        /// </summary>
        public static Color DefaultDisabledTextColorDark = Colors.Gray;

        /// <summary>
        /// Gets ot sets default disabled text color for light theme.
        /// </summary>
        public static Color DefaultDisabledTextColorLight = Colors.Gray;

        /// <summary>
        /// Gets ot sets default hot border color for dark theme.
        /// </summary>
        public static Color DefaultHotBorderColorDark = Colors.DarkGray;

        /// <summary>
        /// Gets ot sets default pressed border color for dark theme.
        /// </summary>
        public static Color DefaultPressedBorderColorDark = Color.FromRgb(61, 61, 61);

        /// <summary>
        /// Gets ot sets default button border width.
        /// </summary>
        public static double DefaultButtonBorderWidth = 1;

        /// <summary>
        /// Gets ot sets default hot border color for light theme.
        /// </summary>
        public static Color DefaultHotBorderColorLight = Color.FromRgb(0, 108, 190);

        /// <summary>
        /// Gets ot sets default text color for light theme.
        /// </summary>
        public static Color DefaultTextColorLight = Colors.Black;

        /// <summary>
        /// Gets ot sets default pressed border color for light theme.
        /// </summary>
        public static Color DefaultPressedBorderColorLight = Colors.DarkGray;

        /// <summary>
        /// Gets or sets the default SVG image for the 'Next Tab' button.
        /// </summary>
        public static Alternet.Drawing.SvgImage? DefaultNextTabImage { get; set; }

        /// <summary>
        /// Gets or sets the default SVG image for the 'Previous Tab' button.
        /// </summary>
        public static Alternet.Drawing.SvgImage? DefaultPreviousTabImage { get; set; }

        internal static ButtonVisualStateSetters ButtonNormalState = new()
        {
            BackgroundColor = GetTransparent,
            BorderColor = GetTransparent,
            TextColor = GetRealTextColor,
            BorderWidth = GetButtonBorder,
        };

        internal static ButtonVisualStateSetters ButtonDisabledState = new()
        {
            BackgroundColor = GetTransparent,
            BorderColor = GetTransparent,
            TextColor = GetRealDisabledTextColor,
            BorderWidth = GetButtonBorder,
        };

        internal static ButtonVisualStateSetters ButtonStickyDisabledState = new()
        {
            BackgroundColor = GetTransparent,
            BorderColor = GetRealPressedBorderColor,
            TextColor = GetRealDisabledTextColor,
            BorderWidth = GetButtonBorder,
        };

        internal static ButtonVisualStateSetters ButtonPressedState = new()
        {
            BackgroundColor = GetTransparent,
            BorderColor = GetRealPressedBorderColor,
            TextColor = GetRealTextColor,
            BorderWidth = GetButtonBorder,
        };

        internal static ButtonVisualStateSetters ButtonNormalStickyState = new()
        {
            BackgroundColor = GetTransparent,
            BorderColor = GetRealPressedBorderColor,
            TextColor = GetRealTextColor,
            BorderWidth = GetButtonBorder,
        };

        internal static ButtonVisualStateSetters ButtonHotState = new()
        {
            BackgroundColor = GetTransparent,
            BorderColor = GetRealHotBorderColor,
            TextColor = GetRealTextColor,
            BorderWidth = GetButtonBorder,
        };

        private readonly StackLayout buttons = new();
        private readonly Grid grid = new();

        private bool allowMultipleSticky = true;
        private StickyButtonStyle stickyStyle = StickyButtonStyle.Border;
        private bool isBottomBorderVisible;
        private bool isTopBorderVisible;
        private BoxView? topBorder;
        private BoxView? bottomBorder;
        private bool isBoldWhenSticky;
        private string? tabFontFamily;
        private double? tabFontSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleToolBarView"/> class.
        /// </summary>
        public SimpleToolBarView()
        {
            grid.RowDefinitions =
            [
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
            ];

            buttons.Orientation = StackOrientation.Horizontal;
            Alternet.UI.MauiApplicationHandler.RegisterThemeChangedHandler();
            grid.Add(buttons, 0, 1);
            Content = grid;
        }

        /// <summary>
        /// Occurs when the system colors change.
        /// </summary>
        public event EventHandler? SystemColorsChanged;

        /// <summary>
        /// Specifies the paint mode for sticky buttons.
        /// </summary>
        public enum StickyButtonStyle
        {
            /// <summary>
            /// Paints the border of the sticky button.
            /// </summary>
            Border,

            /// <summary>
            /// Paints the full underline of the sticky button. Currently disabled.
            /// </summary>
            UnderlineFull,

            /// <summary>
            /// Paints a partial underline of the sticky button. Currently disabled.
            /// </summary>
            UnderlinePartial,
        }

        /// <summary>
        /// Represents an item in the toolbar.
        /// </summary>
        public interface IToolBarItem
        {
            /// <summary>
            /// Occurs when the button is clicked/tapped.
            /// </summary>
            event EventHandler? Clicked;

            /// <summary>
            /// Gets or sets the action which is invoked when the button is clicked/tapped.
            /// </summary>
            Action? ClickedAction { get; set; }

            /// <summary>
            /// Gets or sets the SVG image associated with this button.
            /// </summary>
            Drawing.SvgImage? SvgImage { get; set; }

            /// <summary>
            /// Gets or sets the attributes provider for the toolbar item.
            /// </summary>
            Alternet.UI.IBaseObjectWithAttr AttributesProvider { get; }

            /// <summary>
            /// Gets or sets a value indicating whether the text should be displayed
            /// in bold when item is marked as sticky.
            /// </summary>
            bool IsBoldWhenSticky { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the toolbar item is enabled.
            /// </summary>
            bool IsEnabled { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the toolbar item is visible.
            /// </summary>
            bool IsVisible { get; set; }

            /// <summary>
            /// Gets or sets the style of the sticky button.
            /// </summary>
            StickyButtonStyle StickyStyle { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the item is in a sticky state
            /// (remains pressed even if mouse is not pressed over the item).
            /// </summary>
            bool IsSticky { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the item has border.
            /// </summary>
            bool HasBorder { get; set; }

            /// <summary>
            /// Gets or sets the text associated with the toolbar item.
            /// </summary>
            string Text { get; set; }

            /// <summary>
            /// Gets or sets the font attributes applied to the text.
            /// </summary>
            FontAttributes FontAttributes { get; set; }

            /// <summary>
            /// Gets or sets the font family name used for text rendering.
            /// </summary>
            /// <remarks>Setting this property to an invalid or unsupported font family name may
            /// result in a fallback to the default system font.</remarks>
            string FontFamily { get; set; }

            /// <summary>
            /// Gets or sets the font size for text rendering.
            /// </summary>
            double FontSize { get; set; }

            /// <summary>
            /// Gets or sets the horizontal layout options of the toolbar item.
            /// </summary>
            LayoutOptions HorizontalOptions { get; set; }

            /// <summary>
            /// Gets or sets the vertical layout options of the toolbar item.
            /// </summary>
            LayoutOptions VerticalOptions { get; set; }

            /// <summary>
            /// Gets the button view associated with this instance.
            /// </summary>
            View? Button { get; }

            /// <summary>
            /// Gets the container view that holds the button.
            /// </summary>
            View? ButtonContainer { get; }

            /// <summary>
            /// Updates the visual states of the item.
            /// </summary>
            /// <param name="setNormalState">If set to <c>true</c>,
            /// the normal state will be set.</param>
            void UpdateVisualStates(bool setNormalState);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the top border is visible.
        /// </summary>
        public virtual bool IsTopBorderVisible
        {
            get
            {
                return isTopBorderVisible;
            }

            set
            {
                if (isTopBorderVisible == value)
                    return;
                isTopBorderVisible = value;
                if(topBorder is null)
                {
                    topBorder = CreateToolBarBorder();
                    grid.Add(topBorder);
                }
                else
                {
                    topBorder.IsVisible = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bottom border is visible.
        /// </summary>
        public virtual bool IsBottomBorderVisible
        {
            get
            {
                return isBottomBorderVisible;
            }

            set
            {
                if (isBottomBorderVisible == value)
                    return;
                isBottomBorderVisible = value;
                if (bottomBorder is null)
                {
                    bottomBorder = CreateToolBarBorder();
                    Grid.SetRow(bottomBorder, 2);
                    grid.Add(bottomBorder);
                }
                else
                {
                    bottomBorder.IsVisible = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the style of the sticky buttons.
        /// </summary>
        public virtual StickyButtonStyle StickyStyle
        {
            get
            {
                return stickyStyle;
            }

            set
            {
                if (stickyStyle == value)
                    return;
                stickyStyle = value;

                foreach (var child in Buttons)
                {
                    if (child is not IToolBarItem item)
                        continue;
                    item.StickyStyle = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether toolbar items should
        /// appear bold when in a sticky state.
        /// </summary>
        public virtual bool IsBoldWhenSticky
        {
            get => isBoldWhenSticky;

            set
            {
                if (isBoldWhenSticky == value)
                    return;
                isBoldWhenSticky = value;

                foreach (var child in Buttons)
                {
                    if (child is not IToolBarItem item)
                        continue;
                    item.IsBoldWhenSticky = value;
                }
            }
        }

        /// <summary>
        /// Gets pr sets a value indicating whether multiple sticky buttons are
        /// allowed in the toolbar.
        /// </summary>
        public virtual bool AllowMultipleSticky
        {
            get
            {
                return allowMultipleSticky;
            }

            set
            {
                if (allowMultipleSticky == value)
                    return;
                allowMultipleSticky = value;

                var hasSticky = false;

                foreach (var child in buttons.Children)
                {
                    if (child is not IToolBarItem item)
                        continue;
                    if (!item.IsSticky)
                        continue;
                    if (!hasSticky)
                    {
                        hasSticky = true;
                        continue;
                    }

                    item.IsSticky = false;
                }
            }
        }

        /// <summary>
        /// Gets the collection of buttons in the toolbar.
        /// </summary>
        public IList<IView> Buttons => buttons.Children;

        /// <summary>
        /// Gets the first item in the toolbar, or <see langword="null"/> if the toolbar is empty.
        /// </summary>
        public virtual IToolBarItem? FirstItem
        {
            get
            {
                if (Buttons.Count == 0)
                    return null;
                var item = Buttons[0] as IToolBarItem;
                return item;
            }
        }

        /// <summary>
        /// Gets the last item in the toolbar, or <see langword="null"/> if the toolbar is empty.
        /// </summary>
        public virtual IToolBarItem? LastItem
        {
            get
            {
                if (Buttons.Count == 0)
                    return null;
                var item = Buttons[Buttons.Count - 1] as IToolBarItem;
                return item;
            }
        }

        /// <summary>
        /// Gets the first non-special item in the toolbar, or <see langword="null"/> if none found.
        /// </summary>
        public virtual IToolBarItem? FirstNonSpecialItem
        {
            get
            {
                if (Buttons.Count == 0)
                    return null;
                int idx = GetFirstNonSpecialButtonIndex();
                if (idx < 0)
                    return null;
                var item = Buttons[idx] as IToolBarItem;
                return item;
            }
        }

        /// <summary>
        /// Gets or sets the font family name used for tab text rendering.
        /// </summary>
        public virtual string? TabFontFamily
        {
            get => tabFontFamily;

            set
            {
                if (tabFontFamily == value)
                    return;
                tabFontFamily = value;
                OnTabFontChanged();
            }
        }

        /// <summary>
        /// Gets or sets the font size used for tab text rendering.
        /// </summary>
        public virtual double? TabFontSize
        {
            get => tabFontSize;

            set
            {
                if (tabFontSize == value)
                    return;
                tabFontSize = value;
                OnTabFontChanged();
            }
        }

        /// <summary>
        /// Gets the first item in the toolbar as <see cref="View"/>,
        /// or <see langword="null"/> if the toolbar is empty.
        /// </summary>
        public View? FirstItemAsView
        {
            get => FirstItem as View;
        }

        /// <summary>
        /// Gets the default size of the button image based on the device type.
        /// </summary>
        /// <returns>The default size of the button image.</returns>
        public static int GetDefaultImageSize()
        {
            int size;

            if (Alternet.UI.App.IsDesktopDevice)
            {
                size = DefaultImageButtonSizeDesktop;
            }
            else
            {
                size = DefaultImageButtonSize;
            }

            return size;
        }

        /// <summary>
        /// Gets the button border width.
        /// </summary>
        /// <returns>The button border width.</returns>
        public double GetButtonBorder()
        {
            return GetButtonBorder(this);
        }

        /// <summary>
        /// Gets the hot border color based on the current theme.
        /// </summary>
        /// <returns>The hot border color.</returns>
        public Color GetHotBorderColor()
        {
            return GetRealHotBorderColor(this);
        }

        /// <summary>
        /// Gets the pressed border color based on the current theme.
        /// </summary>
        /// <returns>The pressed border color.</returns>
        public Color GetPressedBorderColor()
        {
            return GetRealPressedBorderColor(this);
        }

        /// <summary>
        /// Gets the default toolbar separator color based on the current theme.
        /// </summary>
        public virtual Color GetSeparatorColor()
        {
            if (IsDark)
                return DefaultSeparatorColorDark;
            return DefaultSeparatorColorLight;
        }

        /// <summary>
        /// Gets the disabled text color based on the current theme.
        /// </summary>
        /// <returns>The disabled text color.</returns>
        public Color GetDisabledTextColor()
        {
            return GetRealDisabledTextColor(this);
        }

        /// <inheritdoc/>
        public override void RaiseSystemColorsChanged()
        {
            if(bottomBorder is not null)
            {
                bottomBorder.BackgroundColor = GetSeparatorColor();
            }

            if (topBorder is not null)
            {
                topBorder.BackgroundColor = GetSeparatorColor();
            }

            SystemColorsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the text color based on the current theme.
        /// </summary>
        /// <returns>The text color.</returns>
        public Color GetTextColor()
        {
            return GetRealTextColor(this);
        }

        /// <summary>
        /// Adds an expanding space to the toolbar.
        /// </summary>
        /// <returns>The created expanding space view.</returns>
        public virtual View AddExpandingSpace()
        {
            var spacer = new BoxView
            {
#pragma warning disable
                HorizontalOptions = LayoutOptions.FillAndExpand,
#pragma warning restore
                Color = Colors.Transparent,
            };

            buttons.Children.Add(spacer);

            return spacer;
        }

        /// <summary>
        /// Adds a sticky button to the toolbar.
        /// </summary>
        /// <param name="text">The text to display on the button.</param>
        /// <param name="toolTip">The tooltip text to display when the mouse
        /// hovers over the button.</param>
        /// <param name="image">The image to display on the button.</param>
        /// <param name="onClick">The action to perform when the button is clicked.</param>
        /// <returns>The created sticky button item.</returns>
        public virtual IToolBarItem AddStickyButton(
            string? text,
            string? toolTip = null,
            Drawing.SvgImage? image = null,
            Action? onClick = null)
        {
            var result = AddButton(text, toolTip, image, onClick);
            result.Clicked += (s, e) =>
            {
                result.IsSticky = !result.IsSticky;
            };
            return result;
        }

        /// <summary>
        /// Inserts a button into the toolbar at the specified index.
        /// </summary>
        /// <remarks>The method creates a new toolbar button, initializes its properties,
        /// and inserts it into the toolbar at the specified index.
        /// If the index is out of range, an exception will be thrown.</remarks>
        /// <param name="index">The zero-based index at which the button should be inserted.
        /// Must be within the valid range of the toolbar's items.</param>
        /// <param name="text">The text to display on the button.
        /// Can be <see langword="null"/> if no text is required.</param>
        /// <param name="toolTip">The tooltip text to display when the user hovers
        /// over the button. Can be <see langword="null"/> if no tooltip is required.</param>
        /// <param name="image">An optional SVG image to display on the button.
        /// Can be <see langword="null"/> if no image is required.</param>
        /// <param name="onClick">An optional action to execute when the button is clicked.
        /// Can be <see langword="null"/> if no action is required.</param>
        /// <returns>The newly created toolbar button.</returns>
        public virtual IToolBarItem InsertButton(
            int index,
            string? text,
            string? toolTip = null,
            Drawing.SvgImage? image = null,
            Action? onClick = null)
        {
            var button = CreateToolBarButton();
            InitButtonProps(button, text, toolTip, image, onClick);
            var container = new ToolBarButtonContainer(button);
            buttons.Children.Insert(index, container);
            return button;
        }

        /// <summary>
        /// Specifies options for adding Next and Previous tab buttons.
        /// </summary>
        public enum AddNextAndPreviousTabButtonsFlags
        {
            /// <summary>
            /// No special behavior.
            /// </summary>
            None = 0,

            /// <summary>
            /// Makes the first content button sticky after 'Next Tab' or 'Previous Tab' button was clicked.
            /// </summary>
            MakeSticky = 1,
        }

        /// <summary>
        /// Parameters for adding 'Next Tab' and 'Previous Tab' buttons.
        /// </summary>
        public struct NextAndPreviousTabButtonsParams
        {
            /// <summary>
            /// The index at which to insert the buttons.
            /// If negative, the buttons will be added at the end.
            /// Default is 0 (buttons will be added at the beginning).
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// The action to perform when the 'Next Tab' button is clicked.
            /// If not specified, the default behavior is used.
            /// </summary>
            public Action? NextTabClick { get; set; }

            /// <summary>
            /// The action to perform when the 'Previous Tab' button is clicked.
            /// If not specified, the default behavior is used.
            /// </summary>
            public Action? PreviousTabClick { get; set; }
        }

        /// <summary>
        /// Adds 'Next Tab' and 'Previous Tab' buttons to the toolbar.
        /// These buttons allow navigation between tabs which is useful on mobile devices.
        /// </summary>
        /// <param name="flags">Flags to customize the behavior of the added buttons.</param>
        /// <param name="prm">Parameters for adding 'Next Tab' and 'Previous Tab' buttons.</param>
        public virtual void AddNextAndPreviousTabButtons(
            AddNextAndPreviousTabButtonsFlags flags = AddNextAndPreviousTabButtonsFlags.None,
            NextAndPreviousTabButtonsParams? prm = null)
        {
            var idx = prm?.Index ?? 0;
            var onPreviousTabClick = prm?.PreviousTabClick;
            var onNextTabClick = prm?.NextTabClick;

            if (idx < 0 || idx > buttons.Children.Count)
                idx = buttons.Children.Count;

            void MakeFirstItemSticky()
            {
                if (flags.HasFlag(AddNextAndPreviousTabButtonsFlags.MakeSticky))
                {
                    FirstNonSpecialItem?.IsSticky = true;
                }
            }

            var previousTabButton = InsertButton(
                idx,
                null,
                Alternet.UI.Localization.CommonStrings.Default.ToolBarPreviousTabToolTip,
                DefaultPreviousTabImage ?? Alternet.UI.KnownSvgImages.ImgAngleLeft,
                () =>
                {
                    if (onPreviousTabClick is null)
                    {
                        LastToFirst();
                        MakeFirstItemSticky();
                    }
                    else
                        onPreviousTabClick();
                });

            var nextTabButton = InsertButton(
                idx + 1,
                null,
                Alternet.UI.Localization.CommonStrings.Default.ToolBarNextTabToolTip,
                DefaultNextTabImage ?? Alternet.UI.KnownSvgImages.ImgAngleRight,
                () =>
                {
                    if (onNextTabClick is null)
                    {
                        FirstToLast();
                        MakeFirstItemSticky();
                        return;
                    }
                    onNextTabClick?.Invoke();
                });

            var btnPrevious = previousTabButton as ToolBarButton;
            var btnNext = nextTabButton as ToolBarButton;

            if (btnPrevious is not null)
            {
                btnPrevious.CanBeSticky = false;
                btnPrevious.Kind = ToolBarButton.ButtonKind.PreviousTab;
            }

            if (btnNext is not null)
            {
                btnNext.CanBeSticky = false;
                btnNext.Kind = ToolBarButton.ButtonKind.NextTab;
            }
        }

        /// <summary>
        /// Gets the index of the last non-special button in the toolbar.
        /// </summary>
        /// <returns>The index of the last non-special button, or -1 if none is found.</returns>
        public virtual int GetLastNonSpecialButtonIndex()
        {
            for (int i = buttons.Children.Count - 1; i >= 0; i--)
            {
                var child = buttons.Children[i];
                if (child is not ToolBarButtonContainer container)
                    continue;
                if (container.Button is not ToolBarButton button)
                    continue;
                if (button.Kind == ToolBarButton.ButtonKind.Normal)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns the index of the first button in the toolbar that is not marked as special.
        /// </summary>
        /// <remarks>A non-special button is defined as a button whose kind is set to Normal. If no such
        /// button exists in the toolbar, the method returns -1.</remarks>
        /// <returns>The zero-based index of the first non-special button if found; otherwise, -1.</returns>
        public virtual int GetFirstNonSpecialButtonIndex()
        {
            for (int i = 0; i < buttons.Children.Count; i++)
            {
                var child = buttons.Children[i];
                if (child is not ToolBarButtonContainer container)
                    continue;
                if (container.Button is not ToolBarButton button)
                    continue;
                if (button.Kind == ToolBarButton.ButtonKind.Normal)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Moves the last button in the toolbar to the first position.
        /// This method ignores special buttons (e.g., 'Next Tab' and 'Previous Tab' buttons).
        /// </summary>
        public virtual void LastToFirst()
        {
            if (buttons.Children.Count == 0)
                return;
            var lastIndex = GetLastNonSpecialButtonIndex();
            if (lastIndex < -1)
                return;

            var firstIndex = GetFirstNonSpecialButtonIndex();
            if(firstIndex == lastIndex)
                return;

            if (firstIndex < 0)
                firstIndex = 0;

            var last = buttons.Children[lastIndex];
            buttons.Children.RemoveAt(lastIndex);
            buttons.Children.Insert(firstIndex, last);
        }

        /// <summary>
        /// Gets the index of the specified toolbar item.
        /// </summary>
        /// <param name="item">The toolbar item to find.</param>
        /// <returns>The zero-based index of the toolbar item if found; otherwise, -1.</returns>
        public virtual int GetItemIndex(IToolBarItem? item)
        {
            if (buttons.Children.Count == 0 || item is null)
                return -1;
            for (int i = 0; i < buttons.Children.Count; i++)
            {
                var child = buttons.Children[i];
                if (child is not ToolBarButtonContainer container)
                    continue;
                if (container.Button is not ToolBarButton button)
                    continue;
                if (button.AttributesProvider.UniqueId == item.AttributesProvider.UniqueId)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Moves the specified toolbar item to the first position.
        /// </summary>
        /// <param name="item">The toolbar item to move.</param>
        public virtual void MakeItemFirst(IToolBarItem? item)
        {
            int itemIndex = GetItemIndex(item);
            if (itemIndex < 0)
                return;
            var firstIndex = GetFirstNonSpecialButtonIndex();

            if (firstIndex == itemIndex)
                return;

            if (firstIndex < 0)
                firstIndex = 0;
            var targetItem = buttons.Children[itemIndex];
            buttons.Children.RemoveAt(itemIndex);
            buttons.Children.Insert(firstIndex, targetItem);
        }

        /// <summary>
        /// Moves the first button in the toolbar to the last position.
        /// This method ignores special buttons (e.g., 'Next Tab' and 'Previous Tab' buttons).
        /// </summary>
        public virtual void FirstToLast()
        {
            if (buttons.Children.Count == 0)
                return;

            var firstIndex = GetFirstNonSpecialButtonIndex();
            if (firstIndex < 0)
                return;

            var lastIndex = GetLastNonSpecialButtonIndex();
            if (lastIndex < 0)
                return;
            if (lastIndex == firstIndex)
                return;

            var first = buttons.Children[firstIndex];
            buttons.Children.RemoveAt(firstIndex);

            buttons.Children.Insert(lastIndex, first);
        }

        /// <summary>
        /// Adds a button to the toolbar.
        /// </summary>
        /// <param name="text">The text to display on the button.</param>
        /// <param name="toolTip">The tooltip text to display when the mouse
        /// hovers over the button.</param>
        /// <param name="image">The image to display on the button.</param>
        /// <param name="onClick">The action to perform when the button is clicked.</param>
        /// <returns>The created button item.</returns>
        public IToolBarItem AddButton(
            string? text,
            string? toolTip = null,
            Drawing.SvgImage? image = null,
            Action? onClick = null)
        {
            return InsertButton(
                buttons.Children.Count,
                text,
                toolTip,
                image,
                onClick);
        }

        /// <summary>
        /// Gets a button from the toolbar by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the button.</param>
        /// <returns>The toolbar button with the specified unique identifier,
        /// or <c>null</c> if no button is found.</returns>
        public virtual IToolBarItem? GetButton(UI.ObjectUniqueId id)
        {
            foreach (var btn in Buttons)
            {
                if (btn is not IToolBarItem button)
                    continue;
                if (button.AttributesProvider.UniqueId == id)
                    return button;
            }

            return null;
        }

        /// <summary>
        /// Removes a toolbar item.
        /// </summary>
        /// <param name="item">The toolbar item to remove.</param>
        public virtual void Remove(IToolBarItem? item)
        {
            if (item is null)
                return;

            var id = item.AttributesProvider.UniqueId;

            foreach (var btn in Buttons)
            {
                if (btn is not IToolBarItem button)
                    continue;
                if (button.AttributesProvider.UniqueId == id)
                {
                    Buttons.Remove(btn);
                    return;
                }
            }
        }

        /// <summary>
        /// Adds a button with the border in the normal state to the toolbar.
        /// </summary>
        /// <param name="text">The text to display on the button.</param>
        /// <param name="toolTip">The tooltip text to display when the mouse
        /// hovers over the button.</param>
        /// <param name="image">The image to display on the button.</param>
        /// <param name="onClick">The action to perform when the button is clicked.</param>
        /// <returns>The created button item.</returns>
        public virtual IToolBarItem AddDialogButton(
            string? text,
            string? toolTip = null,
            Drawing.SvgImage? image = null,
            Action? onClick = null)
        {
            var result = AddButton(text, toolTip, image, onClick);
            result.IsSticky = true;
            return result;
        }

        /// <summary>
        /// Adds an 'Ok' button to the toolbar.
        /// </summary>
        /// <param name="onClick">The action to perform when the button is clicked.</param>
        /// <returns>The created button item.</returns>
        public virtual IToolBarItem AddButtonOk(Action? onClick = null)
        {
            var result = AddDialogButton(
                Alternet.UI.Localization.CommonStrings.Default.ButtonOk,
                null,
                null,
                onClick);
            return result;
        }

        /// <summary>
        /// Adds a 'Cancel' button to the toolbar.
        /// </summary>
        /// <param name="onClick">The action to perform when the button is clicked.</param>
        /// <returns>The created button item.</returns>
        public virtual IToolBarItem AddButtonCancel(Action? onClick = null)
        {
            var result = AddDialogButton(
                Alternet.UI.Localization.CommonStrings.Default.ButtonCancel,
                null,
                null,
                onClick);
            return result;
        }

        /// <summary>
        /// Adds a label to the toolbar.
        /// </summary>
        /// <param name="text">The text to display on the label.</param>
        /// <param name="toolTip">The tooltip text to display when the mouse
        /// hovers over the label.</param>
        /// <param name="image">The image to display on the label.</param>
        /// <param name="onClick">The action to perform when the label is clicked.</param>
        /// <returns>The created label item.</returns>
        public virtual IToolBarItem AddLabel(
            string? text,
            string? toolTip = null,
            Drawing.SvgImage? image = null,
            Action? onClick = null)
        {
            var button = new ToolBarLabel(this);
            InitButtonProps(button, text, toolTip, image, onClick);
            var container = new ToolBarButtonContainer(button);
            buttons.Children.Add(container);
            return button;
        }

        /// <summary>
        /// Adds a separator to the toolbar.
        /// </summary>
        public virtual IToolBarItem AddSeparator()
        {
            var button = new ToolBarSeparator(this);
            buttons.Children.Add(button);
            button.UpdateVisualStates(true);
            return button;
        }

        /// <summary>
        /// Creates a visual border for a toolbar.
        /// </summary>
        /// <returns>A <see cref="BoxView"/> configured to serve as the toolbar border.</returns>
        public virtual BoxView CreateToolBarBorder()
        {
            var result = new BoxView
            {
                HeightRequest = DefaultBorderWidth,
                BackgroundColor = GetSeparatorColor(),
            };

            return result;
        }

        /// <summary>
        /// Called when the tab font properties change.
        /// </summary>
        protected virtual void OnTabFontChanged()
        {
            foreach (var child in Buttons)
            {
                if (child is not IToolBarItem item)
                    continue;
                if (TabFontFamily is not null)
                    item.FontFamily = this.TabFontFamily;
                if (TabFontSize is not null)
                    item.FontSize = this.TabFontSize.Value;
            }
        }

        internal virtual ToolBarButton CreateToolBarButton()
        {
            var button = new ToolBarButton(this);
            return button;
        }

        internal virtual void InitButtonProps(
            ToolBarButton button,
            string? text,
            string? toolTip = null,
            Drawing.SvgImage? image = null,
            Action? onClick = null)
        {
            button.Margin = DefaultButtonMargin;
            button.ClickedAction = onClick;

            if (text is not null)
                button.Text = text;

            button.Clicked += (s, e) => button.RaiseClickedAction();

            if (toolTip is not null)
                ToolTipProperties.SetText(button, toolTip);

            try
            {
                button.SvgImage = image;
            }
            catch (Exception ex)
            {
                try
                {
                    button.SvgImage = Alternet.UI.KnownColorSvgImages.ImgError;
                }
                catch
                {
                }

                Debug.WriteLine($"Error setting button SVG image: {ex.Message}");
            }

            button.StickyStyle = StickyStyle;
            button.IsBoldWhenSticky = IsBoldWhenSticky;

            if (TabFontFamily is not null)
                button.FontFamily = this.TabFontFamily;

            if (TabFontSize is not null)
                button.FontSize = this.TabFontSize.Value;

            button.UpdateVisualStates(true);
        }

        private static double GetButtonBorder(View control)
        {
            return DefaultButtonBorderWidth;
        }

        private static Color GetRealHotBorderColor(View control)
        {
            if (IsDark)
                return DefaultHotBorderColorDark;
            return DefaultHotBorderColorLight;
        }

        private static Color GetRealPressedBorderColor(View control)
        {
            if (IsDark)
                return DefaultPressedBorderColorDark;
            return DefaultPressedBorderColorLight;
        }

        private static Color GetRealDisabledTextColor(View control)
        {
            if (IsDark)
                return DefaultDisabledTextColorDark;
            return DefaultDisabledTextColorLight;
        }

        private static Color GetRealTextColor(View control)
        {
            if (IsDark)
                return DefaultTextColorDark;
            return DefaultTextColorLight;
        }

        private static Color GetTransparent(View control)
        {
            return Colors.Transparent;
        }

        internal class VisualStateSetters
        {
            public Func<View, Color>? BackgroundColor;

            public virtual void InitState(View control, VisualState state, bool clear = true)
            {
                if (clear)
                    state.Setters.Clear();
                if (BackgroundColor is not null)
                    state.AddSetterForBackgroundColor(BackgroundColor(control));
            }
        }

        internal class ButtonVisualStateSetters : VisualStateSetters
        {
            public Func<View, Color>? BorderColor;

            public Func<View, Color>? TextColor;

            public Func<View, double>? BorderWidth;

            public override void InitState(View control, VisualState state, bool clear = true)
            {
                base.InitState(control, state);

                if (BorderColor is not null)
                    state.AddSetterForButtonBorderColor(BorderColor(control));
                if (TextColor is not null)
                    state.AddSetterForButtonTextColor(TextColor(control));
                if (BorderWidth is not null)
                    state.AddSetterForButtonBorderWidth(BorderWidth(control));
            }
        }
    }
}