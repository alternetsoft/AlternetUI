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

        public virtual ToolBarButton CreateToolBarButton()
        {
            var button = new ToolBarButton();
            return button;
        }

        public virtual ToolBarButton AddStickyButton(
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

        public virtual ToolBarButton AddButton(
            string? text,
            string? toolTip = null,
            Drawing.SvgImage? image = null,
            Action? onClick = null)
        {
            var button = CreateToolBarButton();

            button.Margin = DefaultButtonMargin;

            if (text is not null)
                button.Text = text;

            if (onClick is not null)
                button.Clicked += (s, e) => onClick();

            if (toolTip is not null)
                ToolTipProperties.SetText(button, toolTip);

            Alternet.UI.MauiUtils.SetButtonImage(button, image, GetDefaultImageSize());

            button.InitVisualStates();
            VisualStateManager.GoToState(button, VisualStateUtils.NameNormal);
            Children.Add(button);

            return button;
        }

        private static Color GetRealHotBorderColor(object? control)
        {
            if (IsDark)
                return DefaultHotBorderColorDark;
            return DefaultHotBorderColorLight;
        }

        private static Color GetRealPressedBorderColor(object? control)
        {
            if (IsDark)
                return DefaultPressedBorderColorDark;
            return DefaultPressedBorderColorLight;
        }

        private static Color GetRealTextColor(object? control)
        {
            if (IsDark)
                return DefaultTextColorDark;
            return DefaultTextColorLight;
        }

        private static Color GetTransparent(object? control)
        {
            return Colors.Transparent;
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

        private static double GetButtonBorder(object? control)
        {
            return DefaultButtonBorder;
        }

        public class ToolBarLabel : Label, Alternet.UI.IRaiseSystemColorsChanged
        {
            /// <inheritdoc/>
            public virtual void RaiseSystemColorsChanged()
            {
                InitVisualStates();
                VisualStateManager.GoToState(this, VisualStateUtils.NameNormal);
            }

            public virtual void InitVisualStates()
            {
            }
        }

        public class ToolBarButton : Button, Alternet.UI.IRaiseSystemColorsChanged
        {
            private bool isSticky;

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
                    UpdateVisualStates();
                }
            }

            /// <inheritdoc/>
            public virtual void RaiseSystemColorsChanged()
            {
                UpdateVisualStates();
            }

            public virtual void InitVisualStates()
            {
                var visualStateGroup = VisualStateUtils.CreateCommonStatesGroup();

                var normalState = VisualStateUtils.CreateNormalState();
                if(IsSticky)
                    ButtonPressedState.InitState(this, normalState);
                else
                    ButtonNormalState.InitState(this, normalState);
                visualStateGroup.States.Add(normalState);

                var pointerOverState = VisualStateUtils.CreatePointerOverState();
                ButtonHotState.InitState(this, pointerOverState);
                visualStateGroup.States.Add(pointerOverState);

                var pressedState = VisualStateUtils.CreatePressedState();
                ButtonPressedState.InitState(this, pressedState);
                visualStateGroup.States.Add(pressedState);

                var vsGroups = VisualStateManager.GetVisualStateGroups(this);
                vsGroups.Clear();
                vsGroups.Add(visualStateGroup);
            }

            private void UpdateVisualStates()
            {
                InitVisualStates();
                VisualStateManager.GoToState(this, VisualStateUtils.NameNormal);
            }
        }

        internal class VisualStateSetters
        {
            public Func<object?, Color>? BackgroundColor;

            public virtual void InitState(object? control, VisualState state, bool clear = true)
            {
                if (clear)
                    state.Setters.Clear();
                if (BackgroundColor is not null)
                    state.AddSetterForBackgroundColor(BackgroundColor(control));
            }
        }

        internal class ButtonVisualStateSetters : VisualStateSetters
        {
            public Func<object?, Color>? BorderColor;

            public Func<object?, Color>? TextColor;

            public Func<object?, double>? BorderWidth;

            public override void InitState(object? control, VisualState state, bool clear = true)
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
