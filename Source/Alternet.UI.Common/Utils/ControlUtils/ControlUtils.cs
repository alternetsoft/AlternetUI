﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties which are <see cref="AbstractControl"/> related.
    /// </summary>
    public static class ControlUtils
    {
        private static Control? empty;

        /// <summary>
        /// Gets an empty control for the debug purposes.
        /// </summary>
        public static Control Empty
        {
            get
            {
                return empty ??= new Control();
            }
        }

        /// <summary>
        /// Creates a menu item that tracks the painting time for all modes of the specified control.
        /// </summary>
        /// <remarks>After tracking the painting time, the control's overlays are cleared, and
        /// a tooltip is displayed to indicate that the operation is complete.</remarks>
        /// <param name="control">The <see cref="UserControl"/> for which painting
        /// time will be tracked.</param>
        /// <param name="repeatCount">The number of times the painting operation
        /// should be repeated for tracking purposes. Must be a positive integer.</param>
        /// <returns>A <see cref="MenuItem"/> that, when executed, tracks the painting
        /// time and displays the results in the Output panel.</returns>
        public static MenuItem CreateMenuItemTrackPaintingTime(UserControl control, int repeatCount = 250)
        {
            MenuItem result = new("Track painting time", () =>
            {
                KnownRunTimeTrackers.TrackAllModesPainting(control, repeatCount);
                control.Overlays = [];
                control.ShowOverlayToolTipSimple(
                    "Track paint time done. Results in Output.",
                    null,
                    HVAlignment.BottomLeft);
            });

            return result;
        }

        /// <summary>
        /// Creates a menu item that allows the user to select the rendering mode for the specified control.
        /// </summary>
        /// <remarks>The menu includes options for "Software", "OpenGL", and "SkiaSharp" rendering modes.
        /// Selecting an option updates the rendering mode of the specified control and displays
        /// a tooltip indicating
        /// the selected mode. The menu dynamically updates the checked state of its items based
        /// on the current
        /// rendering mode when opened.</remarks>
        /// <param name="control">The <see cref="UserControl"/> for which the rendering
        /// mode can be selected.</param>
        /// <returns>A <see cref="MenuItem"/> containing options to switch between
        /// different rendering modes.</returns>
        public static MenuItem CreateMenuItemRenderingModeSelector(UserControl control)
        {
            var renderingModeMenu = new MenuItem("Rendering Mode");

            var softwareItem = renderingModeMenu.Add("Software", () =>
            {
                ControlUtils.SetRenderingMode(control, ControlRenderingMode.SoftwareDoubleBuffered);

                control.Overlays = [];
                control.ShowOverlayToolTipSimple(
                    "Software rendering mode is selected.",
                    null,
                    HVAlignment.BottomLeft);
            });

            var openGLItem = renderingModeMenu.Add("OpenGL", () =>
            {
                ControlUtils.SetRenderingMode(control, ControlRenderingMode.SkiaSharpWithOpenGL);

                control.Overlays = [];
                control.ShowOverlayToolTipSimple(
                    "OpenGL rendering mode is selected.",
                    null,
                    HVAlignment.BottomLeft);
            });

            var skiaSharpItem = renderingModeMenu.Add("SkiaSharp", () =>
            {
                ControlUtils.SetRenderingMode(control, ControlRenderingMode.SkiaSharp);

                control.Overlays = [];
                control.ShowOverlayToolTipSimple(
                    "SkiaSharp rendering mode is selected.",
                    null,
                    HVAlignment.BottomLeft);
            });

            renderingModeMenu.Opened += (s, e) =>
            {
                var mode = ControlUtils.GetRenderingMode(control);
                softwareItem.Checked = mode == ControlRenderingMode.SoftwareDoubleBuffered;
                openGLItem.Checked = mode == ControlRenderingMode.SkiaSharpWithOpenGL;
                skiaSharpItem.Checked = mode == ControlRenderingMode.SkiaSharp;
            };

            return renderingModeMenu;
        }

        /// <summary>
        /// Configures the rendering mode for the specified control.
        /// </summary>
        /// <remarks>This method updates the rendering flags of the specified control based
        /// on the provided rendering mode. Supported modes include
        /// <see cref="ControlRenderingMode.SkiaSharpWithOpenGL"/>,
        /// <see cref="ControlRenderingMode.SkiaSharp"/>, and
        /// <see cref="ControlRenderingMode.SoftwareDoubleBuffered"/>.
        /// If an unsupported mode is provided, the method will return <see langword="false"/>
        /// without modifying the control.</remarks>
        /// <param name="control">The control whose rendering mode is to be set.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="mode">The desired rendering mode to apply.
        /// Must be a valid value of <see cref="ControlRenderingMode"/>.</param>
        /// <returns><see langword="true"/> if the rendering mode was successfully set;
        /// otherwise, <see langword="false"/> if the
        /// specified mode is unsupported.</returns>
        public static bool SetRenderingMode(AbstractControl control, ControlRenderingMode mode)
        {
            var renderingFlags = control.RenderingFlags;

            switch (mode)
            {
                case ControlRenderingMode.SkiaSharpWithOpenGL:
                    renderingFlags |= ControlRenderingFlags.UseSkiaSharpWithOpenGL;
                    control.RenderingFlags = renderingFlags;
                    return true;
                case ControlRenderingMode.SkiaSharp:
                    renderingFlags |= ControlRenderingFlags.UseSkiaSharp;
                    renderingFlags &= ~ControlRenderingFlags.UseOpenGL;
                    control.RenderingFlags = renderingFlags;
                    return true;
                case ControlRenderingMode.SoftwareDoubleBuffered:
                    renderingFlags &= ~ControlRenderingFlags.UseSkiaSharpWithOpenGL;
                    control.RenderingFlags = renderingFlags;
                    return true;
                case ControlRenderingMode.SoftwareClipped:
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines the rendering mode to be used for the specified control based
        /// on its rendering flags.
        /// </summary>
        /// <param name="control">The control for which the rendering mode is to be determined.
        /// Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="ControlRenderingMode"/> value indicating the rendering mode.</returns>
        public static ControlRenderingMode GetRenderingMode(AbstractControl control)
        {
            if (control.RenderingFlags.HasFlag(ControlRenderingFlags.UseSkiaSharpWithOpenGL))
                return ControlRenderingMode.SkiaSharpWithOpenGL;
            if (control.RenderingFlags.HasFlag(ControlRenderingFlags.UseSkiaSharp))
                return ControlRenderingMode.SkiaSharp;
            return ControlRenderingMode.SoftwareDoubleBuffered;
        }

        /// <summary>
        /// Retrieves the parent platform-specific user control
        /// of the specified <see cref="AbstractControl"/>.
        /// </summary>
        /// <remarks>A platform-specific user control is identified
        /// as an <see cref="AbstractControl"/> that has
        /// <see cref="AbstractControl.IsPlatformControl"/> set to
        /// <see langword="true"/> and is of type <see cref="UserControl"/>.
        /// The method traverses the parent hierarchy of the
        /// specified <paramref name="control"/> to locate such a control.</remarks>
        /// <param name="control">The <see cref="AbstractControl"/> for which
        /// to find the parent platform-specific <see cref="UserControl"/>.
        /// Can be <see langword="null"/>.</param>
        /// <returns>The parent platform-specific <see cref="UserControl"/> if found;
        /// otherwise, <see langword="null"/>.</returns>
        public static UserControl? GetParentPlatformUserControl(AbstractControl? control)
        {
            if (control is null)
                return null;

            AbstractControl? overlayParent = control.Parent;
            while (true)
            {
                if (overlayParent is null)
                    break;
                if (overlayParent.IsPlatformControl && overlayParent is UserControl userControl)
                    return userControl;

                overlayParent = overlayParent.Parent;
            }

            return null;
        }

        /// <summary>
        /// Finds visible control of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the control to find.</typeparam>
        /// <returns></returns>
        public static T? FindVisibleControl<T>()
            where T : AbstractControl
        {
            var windows = App.Current.LastActivatedWindows;

            if (AssemblyUtils.TypeEqualsOrDescendant(typeof(T), typeof(Window)))
            {
                return windows.FirstOrDefault() as T;
            }

            T? result = null;

            foreach(var window in windows)
            {
                window.ForEachVisibleChild(
                    (control) =>
                    {
                        if (control is T tt)
                            result = tt;
                    },
                    true);

                if (result is not null)
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets control for measure purposes in a safe way. Do not change
        /// any properties of the returned control.
        /// </summary>
        /// <param name="obj">The object which is returned if it is a control.</param>
        /// <returns></returns>
        public static AbstractControl SafeControl(object? obj)
        {
            if (obj is AbstractControl control)
                return control;
            return App.SafeWindow ?? Empty;
        }

        /// <summary>
        /// Increases width or height specified in <paramref name="currentValue"/>
        /// if it is less than value specified in <paramref name="minValueAtLeast"/>.
        /// </summary>
        /// <param name="currentValue">Current width or height.</param>
        /// <param name="minValueAtLeast">Minimal value of the result will be at least.</param>
        /// <returns></returns>
        public static Coord GrowCoord(Coord? currentValue, Coord minValueAtLeast)
        {
            minValueAtLeast = Math.Max(0, minValueAtLeast);

            if (currentValue is null || currentValue <= 0)
                return minValueAtLeast;
            var result = Math.Max(currentValue.Value, minValueAtLeast);
            return result;
        }

        /// <summary>
        /// Increases size specified in <paramref name="currentSize"/>
        /// if it is less than size specified in <paramref name="minSizeAtLeast"/>.
        /// Width and height are increased individually.
        /// </summary>
        /// <param name="currentSize">Current size value.</param>
        /// <param name="minSizeAtLeast">Minimal size of the result will be at least.</param>
        /// <returns></returns>
        public static SizeD GrowSize(SizeD? currentSize, SizeD minSizeAtLeast)
        {
            var newWidth = GrowCoord(currentSize?.Width, minSizeAtLeast.Width);
            var newHeight = GrowCoord(currentSize?.Height, minSizeAtLeast.Height);
            return (newWidth, newHeight);
        }
    }
}
