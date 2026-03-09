using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods for managing visual states and properties of user controls.
    /// </summary>
    /// <remarks>This static class contains methods to retrieve background brushes, background actions, and
    /// border settings based on the visual state of an AbstractControl. It also includes functionality to refresh the
    /// control based on changes in visual state, allowing for dynamic updates to the control's appearance.</remarks>
    public static class UserControlHelper
    {
        /// <summary>
        /// Retrieves the background brush for the specified control based on its visual state.
        /// </summary>
        /// <remarks>If the control has a specific background override for the given state, that will be
        /// returned. Otherwise, the method checks the default theme and the control's background color.</remarks>
        /// <param name="control">The control for which the background brush is being retrieved. This parameter cannot be null.</param>
        /// <param name="state">The visual state that influences the background brush selection, such as 'Normal' or 'Hovered'.</param>
        /// <returns>A Brush object representing the background for the control in the specified state, or null if no suitable
        /// background is found.</returns>
        public static Brush? GetBackground(AbstractControl control, VisualControlState state)
        {
            var overrideValue = control.Backgrounds?.GetObjectOrNormal(state);
            if (overrideValue is not null)
                return overrideValue;

            var result = control.GetDefaultTheme()?.DarkOrLight(control.IsDarkBackground);
            var brush = result?.Backgrounds?.GetObjectOrNormal(state);
            brush ??= control.BackgroundColor?.AsBrush;
            return brush;
        }

        /// <summary>
        /// Retrieves the background paint actions associated with the specified control and visual state.
        /// </summary>
        /// <remarks>If the control defines custom background actions for the given state, those are
        /// returned. Otherwise, the method attempts to retrieve background actions from the control's default
        /// theme.</remarks>
        /// <param name="control">The control for which to obtain background paint actions. This parameter cannot be null.</param>
        /// <param name="state">The visual state that determines which background actions are applicable.</param>
        /// <returns>A PaintEventHandler representing the background actions for the specified control and state, or null if no
        /// actions are defined.</returns>
        public static PaintEventHandler? GetBackgroundActions(AbstractControl control, VisualControlState state)
        {
            var overrideValue = control.BackgroundActions?.GetObjectOrNormal(state);
            if (overrideValue is not null)
                return overrideValue;

            var theme = control.GetDefaultTheme()?.DarkOrLight(control.IsDarkBackground);
            var result = theme?.BackgroundActions?.GetObjectOrNormal(state);
            return result;
        }

        /// <summary>
        /// Retrieves the border settings for the specified control and visual state.
        /// </summary>
        /// <remarks>If the control defines border settings for the given state, those settings are
        /// returned. Otherwise, the method attempts to retrieve the default theme's border settings based on the
        /// control's background appearance.</remarks>
        /// <param name="control">The control for which to obtain border settings. This parameter cannot be null.</param>
        /// <param name="state">The visual state of the control that determines which border settings to retrieve.</param>
        /// <returns>A <see cref="BorderSettings"/> instance containing the border settings for the specified control and state,
        /// or <see langword="null"/> if no settings are found.</returns>
        public static BorderSettings? GetBorderSettings(AbstractControl control, VisualControlState state)
        {
            var overrideValue = control.Borders?.GetObjectOrNull(state);
            if (overrideValue is not null)
                return overrideValue;

            var result = control.GetDefaultTheme()?.DarkOrLight(control.IsDarkBackground);
            return result?.Borders?.GetObjectOrNull(state);
        }

        /// <summary>
        /// Handles changes in the visual state of the specified control and refreshes the control based on the provided
        /// refresh options.
        /// </summary>
        /// <remarks>If the refresh options include <see cref="ControlRefreshOptions.RefreshOnState"/>,
        /// the control is refreshed immediately. Otherwise, the control is refreshed only if the specified conditions
        /// for border, image, color, or background changes are met. Use this method to ensure that the control's
        /// appearance accurately reflects its current state.</remarks>
        /// <param name="control">The control whose visual state has changed and may require a refresh.</param>
        /// <param name="refreshOptions">A set of options that determine the conditions under which the control should be refreshed, such as changes
        /// to state, border, image, color, or background.</param>
        public static void OnVisualStateChanged(AbstractControl control, ControlRefreshOptions refreshOptions)
        {
            var options = refreshOptions;

            if (options.HasFlag(ControlRefreshOptions.RefreshOnState))
            {
                control.Refresh();
                return;
            }

            var data = control.StateObjects;
            if (data is null)
                return;

            bool RefreshOnBorder() => options.HasFlag(ControlRefreshOptions.RefreshOnBorder) &&
                data.HasOtherBorders;
            bool RefreshOnImage() => options.HasFlag(ControlRefreshOptions.RefreshOnImage) &&
                data.HasOtherImages;
            bool RefreshOnColor() => options.HasFlag(ControlRefreshOptions.RefreshOnColor) &&
                data.HasOtherColors;
            bool RefreshOnBackground() => options.HasFlag(ControlRefreshOptions.RefreshOnBackground) &&
                data.HasOtherBackgrounds;

            if (RefreshOnBorder() || RefreshOnImage() || RefreshOnBackground()
                || RefreshOnColor())
                control.Refresh();
        }
    }
}
