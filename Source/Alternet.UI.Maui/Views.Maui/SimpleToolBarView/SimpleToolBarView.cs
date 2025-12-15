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
        public IToolBarItem? FirstItem
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

        internal virtual void OnTabFontChanged()
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