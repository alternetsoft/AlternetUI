using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Alternet.Maui
{
    /// <summary>
    /// Provides utility methods for working with visual states in a the application.
    /// </summary>
    public static class VisualStateUtils
    {
        /// <summary>
        /// The name of the normal visual state.
        /// </summary>
        public const string NameNormal = "Normal";

        /// <summary>
        /// The name of the pointer over visual state.
        /// </summary>
        public const string NamePointerOver = "PointerOver";

        /// <summary>
        /// The name of the pressed visual state.
        /// </summary>
        public const string NamePressed = "Pressed";

        /// <summary>
        /// The name of the disabled visual state.
        /// </summary>
        public const string NameDisabled = "Disabled";

        /// <summary>
        /// The name of the focused visual state.
        /// </summary>
        public const string NameFocused = "Focused";

        /// <summary>
        /// The name of the selected visual state.
        /// </summary>
        public const string NameSelected = "Selected";

        /// <summary>
        /// The name of the common states visual state group.
        /// </summary>
        public const string GroupNameCommonStates = "CommonStates";

        /// <summary>
        /// Adds a setter to the specified visual state.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="state">The visual state to add the setter to.</param>
        /// <param name="prop">The bindable property to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The created setter.</returns>
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

        /// <summary>
        /// Adds a setter for the background color to the specified visual state.
        /// </summary>
        /// <param name="state">The visual state to add the setter to.</param>
        /// <param name="value">The background color value to set.</param>
        /// <returns>The created setter.</returns>
        public static Setter AddSetterForBackgroundColor(this VisualState state, Color value)
        {
            return AddSetter<Color>(state, VisualElement.BackgroundColorProperty, value);
        }

        /// <summary>
        /// Adds a setter for the button border color to the specified visual state.
        /// </summary>
        /// <param name="state">The visual state to add the setter to.</param>
        /// <param name="value">The button border color value to set.</param>
        /// <returns>The created setter.</returns>
        public static Setter AddSetterForButtonBorderColor(this VisualState state, Color value)
        {
            return AddSetter<Color>(state, Button.BorderColorProperty, value);
        }

        /// <summary>
        /// Adds a setter for the button text color to the specified visual state.
        /// </summary>
        /// <param name="state">The visual state to add the setter to.</param>
        /// <param name="value">The button text color value to set.</param>
        /// <returns>The created setter.</returns>
        public static Setter AddSetterForButtonTextColor(this VisualState state, Color value)
        {
            return AddSetter(state, Button.TextColorProperty, value);
        }

        /// <summary>
        /// Adds a setter for the button border width to the specified visual state.
        /// </summary>
        /// <param name="state">The visual state to add the setter to.</param>
        /// <param name="value">The button border width value to set.</param>
        /// <returns>The created setter.</returns>
        public static Setter AddSetterForButtonBorderWidth(this VisualState state, double value)
        {
            return AddSetter(state, Button.BorderWidthProperty, value);
        }

        /// <summary>
        /// Creates a <see cref="VisualState"/> for the normal state.
        /// </summary>
        /// <returns>The created normal visual state.</returns>
        public static VisualState CreateNormalState()
        {
            var result = new VisualState
            {
                Name = VisualStateUtils.NameNormal,
            };

            return result;
        }

        /// <summary>
        /// Creates a <see cref="VisualState"/> for the pressed state.
        /// </summary>
        /// <returns>The created pressed visual state.</returns>
        public static VisualState CreatePressedState()
        {
            var result = new VisualState
            {
                Name = VisualStateUtils.NamePressed,
            };

            return result;
        }

        /// <summary>
        /// Creates a <see cref="VisualState"/> for the disabled state.
        /// </summary>
        /// <returns>The created disabled visual state.</returns>
        public static VisualState CreateDisabledState()
        {
            var result = new VisualState
            {
                Name = VisualStateUtils.NameDisabled,
            };

            return result;
        }

        /// <summary>
        /// Creates a <see cref="VisualState"/> for the pointer over state.
        /// </summary>
        /// <returns>The created pointer over visual state.</returns>
        public static VisualState CreatePointerOverState()
        {
            var result = new VisualState
            {
                Name = VisualStateUtils.NamePointerOver,
            };

            return result;
        }

        /// <summary>
        /// Creates a visual state group for common states.
        /// </summary>
        /// <returns>The created common states visual state group.</returns>
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
