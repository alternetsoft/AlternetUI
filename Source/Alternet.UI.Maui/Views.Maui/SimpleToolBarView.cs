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
        : HorizontalStackLayout, Alternet.UI.IRaiseSystemColorsChanged
    {
        public static int DefaultButtonMargin = 1;

        /// <summary>
        /// Gets ot sets default size of the button image on mobile platform.
        /// </summary>
        public static int DefaultImageButtonSize = 24;

        /// <summary>
        /// Gets ot sets default size of the button image on desktop platform.
        /// </summary>
        public static int DefaultImageButtonSizeDesktop = 16;

        public static Color DefaultHotBorderColorDark = Color.FromRgb(112, 112, 112);

        public static Color DefaultTextColorDark = Color.FromRgb(214, 214, 214);

        public static Color DefaultPressedBorderColorDark = Colors.DarkGray;

        public static double DefaultButtonBorder = 1;

        public static Color DefaultHotBorderColorLight = Color.FromRgb(0, 108, 190);

        public static Color DefaultTextColorLight = Colors.Black;

        public static Color DefaultPressedBorderColorLight = Colors.DarkGray;

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

        public virtual Button AddButton(
            string? text,
            string? toolTip = null,
            Drawing.SvgImage? image = null,
            Action? onClick = null)
        {
            var button = new Button
            {
                Margin = DefaultButtonMargin,
            };

            if (text is not null)
                button.Text = text;

            if (onClick is not null)
                button.Clicked += (s, e) => onClick();

            if(toolTip is not null)
                ToolTipProperties.SetText(button, toolTip);

            Alternet.UI.MauiUtils.SetButtonImage(button, image, GetDefaultImageSize());

            Children.Add(button);
            InitVisualStates(button);
            VisualStateManager.GoToState(button, VisualStateUtils.NameNormal);

            return button;
        }

        protected virtual bool IsDark
        {
            get
            {
                return Alternet.UI.SystemSettings.AppearanceIsDark;
            }
        }

        protected virtual Color GetRealHotBorderColor()
        {
            if (IsDark)
                return DefaultHotBorderColorDark;
            return DefaultHotBorderColorLight;
        }

        protected virtual Color GetRealPressedBorderColor()
        {
            if (IsDark)
                return DefaultPressedBorderColorDark;
            return DefaultPressedBorderColorLight;
        }

        protected virtual Color GetRealTextColor()
        {
            if (IsDark)
                return DefaultTextColorDark;
            return DefaultTextColorLight;
        }

        protected virtual void InitVisualStates(Button button)
        {
            var visualStateGroup = VisualStateUtils.CreateCommonStatesGroup();

            var normalState = VisualStateUtils.CreateNormalState();
            normalState.AddSetterForBackgroundColor(() => Colors.Transparent);
            normalState.AddSetterForButtonBorderColor(() => Colors.Transparent);
            normalState.AddSetterForButtonTextColor(GetRealTextColor);
            normalState.AddSetterForButtonBorderWidth(() => DefaultButtonBorder);
            visualStateGroup.States.Add(normalState);

            var pointerOverState = VisualStateUtils.CreatePointerOverState();
            pointerOverState.AddSetterForBackgroundColor(() => Colors.Transparent);
            pointerOverState.AddSetterForButtonBorderColor(GetRealHotBorderColor);
            pointerOverState.AddSetterForButtonTextColor(GetRealTextColor);
            pointerOverState.AddSetterForButtonBorderWidth(() => DefaultButtonBorder);
            visualStateGroup.States.Add(pointerOverState);

            var pressedState = VisualStateUtils.CreatePressedState();
            pressedState.AddSetterForBackgroundColor(() => Colors.Transparent);
            pressedState.AddSetterForButtonBorderColor(GetRealPressedBorderColor);
            pressedState.AddSetterForButtonTextColor(GetRealTextColor);
            pressedState.AddSetterForButtonBorderWidth(() => DefaultButtonBorder);
            visualStateGroup.States.Add(pressedState);

            var vsGroups = VisualStateManager.GetVisualStateGroups(button);
            vsGroups.Clear();
            vsGroups.Add(visualStateGroup);
        }

        /// <inheritdoc/>
        public virtual void RaiseSystemColorsChanged()
        {
            foreach (var control in Children)
            {
                if(control is Button button)
                {
                    InitVisualStates(button);
                    VisualStateManager.GoToState(button, VisualStateUtils.NameNormal);
                }
            }
        }
    }
}
