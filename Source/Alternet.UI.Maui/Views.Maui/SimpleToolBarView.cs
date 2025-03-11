﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class SimpleToolBarView : ContentView, Alternet.UI.IRaiseSystemColorsChanged
    {
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
        /// Gets ot sets default hot border color for dark theme.
        /// </summary>
        public static Color DefaultHotBorderColorDark = Color.FromRgb(112, 112, 112);

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
        /// Gets ot sets default pressed border color for dark theme.
        /// </summary>
        public static Color DefaultPressedBorderColorDark = Colors.DarkGray;

        /// <summary>
        /// Gets ot sets default button border width.
        /// </summary>
        public static double DefaultButtonBorder = 1;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleToolBarView"/> class.
        /// </summary>
        public SimpleToolBarView()
        {
            buttons.Orientation = StackOrientation.Horizontal;
            Alternet.UI.MauiApplicationHandler.RegisterThemeChangedHandler();
            grid.Add(buttons);
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
            /// Paints the full underline of the sticky button.
            /// </summary>
            UnderlineFull,

            /// <summary>
            /// Paints a partial underline of the sticky button.
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
            public event EventHandler? Clicked;

            /// <summary>
            /// Gets or sets the attributes provider for the toolbar item.
            /// </summary>
            Alternet.UI.IBaseObjectWithAttr AttributesProvider { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the toolbar item is enabled.
            /// </summary>
            bool IsEnabled { get; set; }

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
            /// Gets or sets the horizontal layout options of the toolbar item.
            /// </summary>
            LayoutOptions HorizontalOptions { get; set; }

            /// <summary>
            /// Gets or sets the vertical layout options of the toolbar item.
            /// </summary>
            LayoutOptions VerticalOptions { get; set; }

            /// <summary>
            /// Updates the visual states of the item.
            /// </summary>
            /// <param name="setNormalState">If set to <c>true</c>,
            /// the normal state will be set.</param>
            void UpdateVisualStates(bool setNormalState);
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

        public IList<IView> Buttons => buttons.Children;

        /// <summary>
        /// Gets a value indicating whether the current theme is dark.
        /// </summary>
        private static bool IsDark
        {
            get
            {
                return Alternet.UI.SystemSettings.AppearanceIsDark;
            }
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
        public virtual void RaiseSystemColorsChanged()
        {
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
        /// Adds a button to the toolbar.
        /// </summary>
        /// <param name="text">The text to display on the button.</param>
        /// <param name="toolTip">The tooltip text to display when the mouse
        /// hovers over the button.</param>
        /// <param name="image">The image to display on the button.</param>
        /// <param name="onClick">The action to perform when the button is clicked.</param>
        /// <returns>The created button item.</returns>
        public virtual IToolBarItem AddButton(
            string? text,
            string? toolTip = null,
            Drawing.SvgImage? image = null,
            Action? onClick = null)
        {
            var button = CreateToolBarButton();
            InitButtonProps(button, text, toolTip, image, onClick);
            var container = new ToolbarButtonContainer(button);
            buttons.Children.Add(container);
            return button;
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
            var container = new ToolbarButtonContainer(button);
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

        internal virtual void InitButtonProps(
            ToolBarButton button,
            string? text,
            string? toolTip = null,
            Drawing.SvgImage? image = null,
            Action? onClick = null)
        {
            button.Margin = DefaultButtonMargin;

            if (text is not null)
                button.Text = text;

            if (onClick is not null)
                button.Clicked += (s, e) => onClick();

            if (toolTip is not null)
                ToolTipProperties.SetText(button, toolTip);

            button.SvgImage = image;
            button.StickyStyle = StickyStyle;

            button.UpdateVisualStates(true);
        }

        internal virtual ToolBarButton CreateToolBarButton()
        {
            var button = new ToolBarButton(this);
            return button;
        }

        private static double GetButtonBorder(View control)
        {
            return DefaultButtonBorder;
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

        /// <summary>
        /// Represents a label in the toolbar.
        /// </summary>
        internal partial class ToolBarLabel : ToolBarButton
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarLabel"/> class.
            /// </summary>
            public ToolBarLabel(SimpleToolBarView toolBar)
                : base(toolBar)
            {
            }

            /// <inheritdoc/>
            public override bool HasBorder
            {
                get
                {
                    return false;
                }

                set
                {
                }
            }
        }

        /// <summary>
        /// Represents a separator in the toolbar.
        /// </summary>
        internal partial class ToolBarSeparator
            : BoxView, Alternet.UI.IRaiseSystemColorsChanged, IToolBarItem
        {
            private Alternet.UI.IBaseObjectWithAttr? attributesProvider;
            private SimpleToolBarView toolbar;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarSeparator"/> class.
            /// </summary>
            public ToolBarSeparator(SimpleToolBarView toolbar)
            {
                this.toolbar = toolbar;
                WidthRequest = DefaultSeparatorWidth;
                Margin = DefaultSeparatorMargin;
                BackgroundColor = GetLineColor();
            }

            /// <inheritdoc/>
            public event EventHandler? Clicked;

            /// <inheritdoc/>
            public virtual Alternet.UI.IBaseObjectWithAttr AttributesProvider
            {
                get => attributesProvider ??= new Alternet.UI.BaseObjectWithAttr();

                set
                {
                    attributesProvider = value;
                }
            }

            /// <inheritdoc/>
            public StickyButtonStyle StickyStyle
            {
                get => StickyButtonStyle.Border;

                set
                {
                }
            }

            /// <inheritdoc/>
            public bool IsSticky
            {
                get => false;

                set
                {
                }
            }

            /// <inheritdoc/>
            public bool HasBorder
            {
                get => true;

                set
                {
                }
            }

            /// <inheritdoc/>
            public string Text
            {
                get => string.Empty;
                set
                {
                }
            }

            /// <summary>
            /// Gets the parent toolbar view.
            /// </summary>
            [Browsable(false)]
            internal SimpleToolBarView ToolBar => toolbar;

            /// <inheritdoc/>
            public virtual void RaiseSystemColorsChanged()
            {
                UpdateVisualStates(false);
            }

            /// <summary>
            /// Gets color of the separator line.
            /// </summary>
            /// <returns></returns>
            public virtual Color GetLineColor()
            {
                Color color;

                if (ToolBar is null)
                {
                    if (IsDark)
                        color = DefaultSeparatorColorDark;
                    else
                        color = DefaultSeparatorColorLight;
                }
                else
                    color = ToolBar.GetSeparatorColor();

                return color;
            }

            /// <inheritdoc/>
            public virtual void UpdateVisualStates(bool setNormalState)
            {
                BackgroundColor = GetLineColor();
            }
        }

        /// <summary>
        /// Represents a button in the toolbar.
        /// </summary>
        internal partial class ToolBarButton
            : Button, Alternet.UI.IRaiseSystemColorsChanged, IToolBarItem
        {
            private bool isSticky;
            private bool hasBorder = true;
            private Drawing.SvgImage? svgImage;
            private Alternet.UI.IBaseObjectWithAttr? attributesProvider;
            private StickyButtonStyle stickyStyle = StickyButtonStyle.Border;
            private SimpleToolBarView toolBar;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarButton"/> class.
            /// </summary>
            public ToolBarButton(SimpleToolBarView toolBar)
            {
                this.toolBar = toolBar;
            }

            /// <summary>
            /// Occurs when the sticky state of the button changes.
            /// </summary>
            public event EventHandler? StickyChanged;

            /// <summary>
            /// Gets or sets the style of the sticky button.
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
                }
            }

            /// <summary>
            /// Gets or sets the SVG image associated with the toolbar button.
            /// </summary>
            public virtual Drawing.SvgImage? SvgImage
            {
                get
                {
                    return svgImage;
                }

                set
                {
                    if (svgImage == value)
                        return;
                    svgImage = value;
                    UpdateImage();
                }
            }

            /// <inheritdoc/>
            public virtual bool HasBorder
            {
                get
                {
                    return hasBorder;
                }

                set
                {
                    if (hasBorder == value)
                        return;
                    hasBorder = value;
                    UpdateVisualStates(true);
                }
            }

            /// <inheritdoc/>
            public virtual Alternet.UI.IBaseObjectWithAttr AttributesProvider
            {
                get => attributesProvider ??= new Alternet.UI.BaseObjectWithAttr();

                set
                {
                    attributesProvider = value;
                }
            }

            /// <inheritdoc/>
            public virtual bool IsSticky
            {
                get
                {
                    return isSticky;
                }

                set
                {
                    if (isSticky == value)
                        return;
                    isSticky = value;

                    if (value && !AllowMultipleSticky)
                    {
                        foreach (var sibling in ToolBar.Buttons)
                        {
                            if (sibling is not IToolBarItem buttonSibling)
                                continue;
                            if (buttonSibling.AttributesProvider.UniqueId == AttributesProvider.UniqueId)
                                buttonSibling.IsSticky = true;
                            else
                                buttonSibling.IsSticky = false;
                        }
                    }

                    UpdateVisualStates(true);

                    StickyChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            /// <summary>
            /// Gets a value indicating whether the toolbar (in which button is inserted)
            /// allows multiple sticky buttons.
            /// </summary>
            [Browsable(false)]
            internal bool AllowMultipleSticky => ToolBar.AllowMultipleSticky;

            /// <summary>
            /// Gets the parent toolbar view.
            /// </summary>
            [Browsable(false)]
            internal SimpleToolBarView ToolBar
            {
                get
                {
                    return toolBar;
                }
            }

            /// <inheritdoc/>
            public virtual void RaiseSystemColorsChanged()
            {
                UpdateVisualStates(true);
            }

            /// <inheritdoc/>
            public virtual void UpdateVisualStates(bool setNormalState)
            {
                var visualStateGroup = VisualStateUtils.CreateCommonStatesGroup();

                var normalState = VisualStateUtils.CreateNormalState();
                if (IsSticky && HasBorder && StickyStyle == StickyButtonStyle.Border)
                {
                    ButtonPressedState.InitState(this, normalState);
                }
                else
                {
                    ButtonNormalState.InitState(this, normalState);
                }

                visualStateGroup.States.Add(normalState);

                var pointerOverState = VisualStateUtils.CreatePointerOverState();
                if (HasBorder)
                    ButtonHotState.InitState(this, pointerOverState);
                else
                    ButtonNormalState.InitState(this, pointerOverState);
                visualStateGroup.States.Add(pointerOverState);

                var pressedState = VisualStateUtils.CreatePressedState();
                if (HasBorder)
                    ButtonPressedState.InitState(this, pressedState);
                else
                    ButtonNormalState.InitState(this, pressedState);
                visualStateGroup.States.Add(pressedState);

                var disabledState = VisualStateUtils.CreateDisabledState();
                ButtonDisabledState.InitState(this, disabledState);
                visualStateGroup.States.Add(disabledState);

                var vsGroups = VisualStateManager.GetVisualStateGroups(this);
                vsGroups.Clear();
                vsGroups.Add(visualStateGroup);

                if (setNormalState)
                    VisualStateManager.GoToState(this, VisualStateUtils.NameNormal);
            }

            internal void UpdateImage()
            {
                Alternet.UI.MauiUtils.SetButtonImage(this, svgImage, GetDefaultImageSize(), !IsEnabled);
            }

            /// <inheritdoc/>
            protected override void OnPropertyChanged(string propertyName)
            {
                base.OnPropertyChanged(propertyName);

                if (svgImage is not null)
                {
                    if (propertyName == IsEnabledProperty.PropertyName)
                    {
                        UpdateImage();
                    }
                }
            }
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

        internal partial class ToolbarButtonContainer
            : VerticalStackLayout, Alternet.UI.IRaiseSystemColorsChanged, IToolBarItem
        {
            private readonly ToolBarButton button;
            private readonly BoxView underline = new();

            public ToolbarButtonContainer(ToolBarButton button)
            {
                this.button = button;

                Padding = 5;

                underline.IsVisible = false;
                underline.Margin = DefaultStickyUnderlineMargin;
                underline.Color = Colors.Transparent;
                underline.HeightRequest = DefaultStickyUnderlineSize.Height;

                Children.Add(button);
                Children.Add(underline);
            }

            public event EventHandler? Clicked
            {
                add => button.Clicked += value;
                remove => button.Clicked -= value;
            }

            public virtual UI.IBaseObjectWithAttr AttributesProvider
            {
                get => button.AttributesProvider;
                set => button.AttributesProvider = value;
            }

            public virtual StickyButtonStyle StickyStyle
            {
                get => button.StickyStyle;

                set
                {
                    if (StickyStyle == value)
                        return;
                    button.StickyStyle = value;
                    UpdateVisualStates(false);
                }
            }

            public virtual bool IsSticky
            {
                get => button.IsSticky;

                set
                {
                    button.IsSticky = value;
                    UpdateVisualStates(false);
                }
            }

            public virtual bool HasBorder
            {
                get => button.HasBorder;
                set => button.HasBorder = value;
            }

            public virtual string Text
            {
                get => button.Text;
                set => button.Text = value;
            }

            public virtual void RaiseSystemColorsChanged()
            {
                UpdateVisualStates(false);
            }

            public virtual void UpdateVisualStates(bool setNormalState)
            {
                Color color;

                if (IsDark)
                    color = DefaultStickyUnderlineColorDark;
                else
                    color = DefaultStickyUnderlineColorLight;

                underline.Color = IsSticky ? color : Colors.Transparent;
                underline.IsVisible = StickyStyle == StickyButtonStyle.UnderlineFull ||
                    StickyStyle == StickyButtonStyle.UnderlinePartial;
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