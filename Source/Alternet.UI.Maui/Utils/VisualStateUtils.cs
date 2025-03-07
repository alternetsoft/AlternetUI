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
            Func<T> getFunc)
        {
            var setter = new Setter
            {
                Property = prop,
                Value = getFunc(),
            };

            state.Setters.Add(setter);

            return setter;
        }

        public static Setter AddSetterForBackgroundColor(
            this VisualState state,
            Func<Color> getFunc)
        {
            return AddSetter<Color>(state, VisualElement.BackgroundColorProperty, getFunc);
        }

        public static Setter AddSetterForButtonBorderColor(
            this VisualState state,
            Func<Color> getFunc)
        {
            return AddSetter<Color>(state, Button.BorderColorProperty, getFunc);
        }

        public static Setter AddSetterForButtonTextColor(
            this VisualState state,
            Func<Color> getFunc)
        {
            return AddSetter(state, Button.TextColorProperty, getFunc);
        }

        public static Setter AddSetterForButtonBorderWidth(
            this VisualState state,
            Func<double> getFunc)
        {
            return AddSetter(state, Button.BorderWidthProperty, getFunc);
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
