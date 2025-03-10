using System;
using System.Collections.Generic;
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
    public partial class SimpleToolBarView
        : HorizontalStackLayout
    {
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleToolBarView"/> class.
        /// </summary>
        public SimpleToolBarView()
        {
            Alternet.UI.MauiApplicationHandler.RegisterThemeChangedHandler();
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
            /// Gets or sets a value indicating whether the toolbar item is enabled.
            /// </summary>
            bool IsEnabled { get; set; }

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
            /// Updates the visual states of the item.
            /// </summary>
            /// <param name="setNormalState">If set to <c>true</c>,
            /// the normal state will be set.</param>
            void UpdateVisualStates(bool setNormalState);
        }

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
            Children.Add(button);
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
            var button = new ToolBarLabel();
            InitButtonProps(button, text, toolTip, image, onClick);
            Children.Add(button);
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

            button.UpdateVisualStates(true);
        }

        internal virtual ToolBarButton CreateToolBarButton()
        {
            var button = new ToolBarButton();
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
        public partial class ToolBarLabel : ToolBarButton
        {
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
        /// Represents a button in the toolbar.
        /// </summary>
        public partial class ToolBarButton : Button, Alternet.UI.IRaiseSystemColorsChanged,
            IToolBarItem
        {
            private bool isSticky;
            private bool hasBorder = true;
            private Drawing.SvgImage? svgImage;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolBarButton"/> class.
            /// </summary>
            public ToolBarButton()
            {
            }

            /// <summary>
            /// Occurs when the sticky state of the button changes.
            /// </summary>
            public event EventHandler? StickyChanged;

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
                    UpdateVisualStates(true);
                    StickyChanged?.Invoke(this, EventArgs.Empty);
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
                if(IsSticky && HasBorder)
                    ButtonPressedState.InitState(this, normalState);
                else
                    ButtonNormalState.InitState(this, normalState);
                visualStateGroup.States.Add(normalState);

                var pointerOverState = VisualStateUtils.CreatePointerOverState();
                if(HasBorder)
                    ButtonHotState.InitState(this, pointerOverState);
                else
                    ButtonNormalState.InitState(this, pointerOverState);
                visualStateGroup.States.Add(pointerOverState);

                var pressedState = VisualStateUtils.CreatePressedState();
                if(HasBorder)
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

                if(setNormalState)
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

                if(svgImage is not null)
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
