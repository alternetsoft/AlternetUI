using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Alternet.Maui
{
    public static class VisualStateUtils
    {
        public const string NameNormal = "Normal";

        public const string NamePointerOver = "PointerOver";

        public const string NamePressed = "Pressed";

        public const string NameDisabled = "Disabled";

        public const string NameFocused = "Focused";

        public const string NameSelected = "Selected";

        public const string GroupNameCommonStates = "CommonStates";

        public static Setter AddSetter<T>(
            this VisualState state,
            BindableProperty prop,
            T value)
        {
            var setter = new Setter
            {
                Property = prop,
                Value = value,
            };

            state.Setters.Add(setter);

            return setter;
        }

        public static Setter AddSetterForBackgroundColor(this VisualState state, Color value)
        {
            return AddSetter<Color>(state, VisualElement.BackgroundColorProperty, value);
        }

        public static Setter AddSetterForButtonBorderColor(this VisualState state, Color value)
        {
            return AddSetter<Color>(state, Button.BorderColorProperty, value);
        }

        public static Setter AddSetterForButtonTextColor(this VisualState state, Color value)
        {
            return AddSetter(state, Button.TextColorProperty, value);
        }

        public static Setter AddSetterForButtonBorderWidth(this VisualState state, double value)
        {
            return AddSetter(state, Button.BorderWidthProperty, value);
        }

        public static VisualState CreateNormalState()
        {
            var result = new VisualState
            {
                Name = VisualStateUtils.NameNormal,
            };

            return result;
        }

        public static VisualState CreatePressedState()
        {
            var result = new VisualState
            {
                Name = VisualStateUtils.NamePressed,
            };

            return result;
        }

        public static VisualState CreatePointerOverState()
        {
            var result = new VisualState
            {
                Name = VisualStateUtils.NamePointerOver,
            };

            return result;
        }

        public static VisualStateGroup CreateCommonStatesGroup()
        {
            var result = new VisualStateGroup
            {
                Name = VisualStateUtils.GroupNameCommonStates,
            };

            return result;
        }
    }
}
