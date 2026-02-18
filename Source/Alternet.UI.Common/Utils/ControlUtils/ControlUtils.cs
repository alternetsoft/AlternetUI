using System;
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
        /// Creates a "More actions" button in the specified toolbar which shows the specified context menu when clicked.
        /// </summary>
        /// <param name="toolBar">The <see cref="ToolBar"/> to add the button to.</param>
        /// <param name="contextMenu">The <see cref="ContextMenu"/> to show when the button is clicked.</param>
        public static SpeedButton CreateMoreActionsButton(ToolBar toolBar, ContextMenu contextMenu)
        {
            var button = toolBar.AddSpeedBtnCore(
                null,
                KnownSvgImages.ImgMoreActions,
                "More actions",
                null);
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.DropDownMenu = contextMenu;
            return button;
        }

        /// <summary>
        /// Enables showing context menu on long tap for the specified control.
        /// This is useful for touch devices where right-click is not available.
        /// This method can be called multiple times for the same control.
        /// </summary>
        /// <param name="control">The <see cref="AbstractControl"/> to enable context menu on long tap.</param>
        /// <param name="enable"><c>true</c> to enable context menu on long tap; otherwise, <c>false</c>.</param>
        public static void EnableContextMenuOnLongTap(AbstractControl control, bool enable = true)
        {
            control.CanLongTap = true;

            void OnLongTap(object? s, LongTapEventArgs e)
            {
                control.ShowContextMenu();
            }

            control.LongTap -= OnLongTap;

            if(enable)
                control.LongTap += OnLongTap;
        }

        /// <summary>
        /// Resets the <see cref="AbstractControl.IsMouseLeftButtonDown"/> property of the specified control
        /// and its child controls.
        /// </summary>
        /// <param name="control">The <see cref="AbstractControl"/> to reset.</param>
        /// <param name="recursive"><c>true</c> to reset child controls; otherwise, <c>false</c>. Optional. Default is <c>true</c>.</param>
        public static void ResetIsMouseLeftButtonDown(AbstractControl control, bool recursive = true)
        {
            control.IsMouseLeftButtonDown = false;
            if (recursive)
            {
                control.ForEachChild(
                    (child) =>
                    {
                        ResetIsMouseLeftButtonDown(child, true);
                    });
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
                control.ShowOverlayToolTipSimple(
                    "Tracking painting time...",
                    HVAlignment.Center,
                    OverlayToolTipFlags.Clear);
                KnownRunTimeTrackers.TrackAllModesPainting(control, repeatCount);
                control.Overlays = [];
                control.ShowOverlayToolTipSimple(
                    "Track paint time done. Results in Output.",
                    HVAlignment.Center,
                    OverlayToolTipFlags.ClearAndDismiss);
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
        /// Determines whether the specified DPI scale factor is effectively equal to 1.0 within a small tolerance.
        /// </summary>
        /// <remarks>This method is useful for checking if DPI scaling is effectively disabled or
        /// negligible, accounting for minor floating-point inaccuracies.</remarks>
        /// <param name="dpiScale">The DPI scale factor to evaluate. Typically, a value of 1.0 indicates no scaling.</param>
        /// <returns>true if the scale factor is within 0.001 of 1.0; otherwise, false.</returns>
        public static bool IsScaleFactorCloseToOne(float dpiScale)
        {
            return Math.Abs(dpiScale - 1) < 0.001;
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
                    return false;
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
        /// Schedules an action to be performed on a test control of the specified type after setting focus and
        /// processing pending events.
        /// </summary>
        /// <remarks>This method retrieves a test instance of the specified control type and schedules the
        /// provided action to run on it during the application's idle time. The control is focused before the action is
        /// invoked. If the control cannot be found, the method logs a message and does not invoke the action.</remarks>
        /// <typeparam name="T">The type of control to retrieve and operate on. Must inherit from AbstractControl.</typeparam>
        /// <param name="name">The name of the action to be logged for diagnostic purposes.</param>
        /// <param name="action">The action to perform on the retrieved control. If null, no action is performed.</param>
        public static void RunTestControlAction<T>(string name, Action<T>? action)
            where T : AbstractControl
        {
            var result = ControlUtils.GetControlForTests<T>();
            if (result is null)
            {
                App.Log($"RunTestControlAction: {typeof(T).FullName} not found");
                return;
            }

            App.AddIdleTask(() =>
            {
                App.Log($"RunTestControlAction: {nameof(T)}.{name}");
                result.SetFocusIfPossible();
                App.DoEvents();
                action?.Invoke(result);
            });
        }

        /// <summary>
        /// Retrieves the first visible control of type <typeparamref name="T"/>
        /// that is not marked to be hidden from tests.
        /// </summary>
        /// <remarks>This method is intended for use in test scenarios to locate a control that is not
        /// explicitly excluded from testing by the "HideFromTests" attribute.</remarks>
        /// <typeparam name="T">The type of control to search for. Must inherit from AbstractControl.</typeparam>
        /// <returns>A control of type <typeparamref name="T"/> that is visible and not marked as hidden from tests; otherwise,
        /// <see langword="null"/> if no such control is found.</returns>
        public static T? GetControlForTests<T>()
            where T : AbstractControl
        {
            var results = ControlUtils.EnumVisibleControls<T>();

            foreach (var panel in results)
            {
                var hideFromTests = panel.CustomAttr["HideFromTests"];
                if (hideFromTests is null || !(bool)hideFromTests)
                    return panel;
            }

            return null;
        }

        /// <summary>
        /// Enumerates all visible controls of type <typeparamref name="T"/> from the most recently activated window.
        /// </summary>
        /// <remarks>Only controls from the most recently activated window are included. If no visible
        /// controls of the specified type are found, the returned collection will be empty.</remarks>
        /// <typeparam name="T">The type of control to enumerate. Must derive from <see cref="AbstractControl"/>.</typeparam>
        /// <returns>An enumerable collection of controls of type <typeparamref name="T"/> that are currently visible in the last
        /// activated window. The collection will be empty if no matching controls are found.</returns>
        public static IEnumerable<T> EnumVisibleControls<T>()
            where T : AbstractControl
        {
            var windows = App.Current.LastActivatedWindows;

            List<T> list = new();

            foreach (var window in windows)
            {
                window.ForEachVisibleChild(
                    (control) =>
                    {
                        if (control is T tt)
                            list.Add(tt);
                    },
                    true);

                if (list.Count > 0)
                    break;
            }

            return list;
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
