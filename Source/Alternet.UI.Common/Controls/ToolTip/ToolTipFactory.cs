using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties which allow to change tooltips behavior.
    /// </summary>
    public static class ToolTipFactory
    {
        private static IToolTipFactoryHandler? handler;
        private static WeakReferenceValue<UserControl> lastContainerWithToolTip = new();
        private static WeakReferenceValue<AbstractControl> lastControlWithToolTip = new();
        private static ObjectUniqueId? lastToolTipId;
        private static IControlNotification? notification;

        /// <summary>
        /// Occurs when an overlay tooltip is about to be displayed, allowing
        /// subscribers to modify, suppress or show the tooltip.
        /// </summary>
        /// <remarks>This event is triggered before an overlay tooltip is shown.
        /// Subscribers can handle the event to customize the tooltip's content,
        /// show it in a different way, or prevent
        /// it from being displayed by setting the <see cref="HandledEventArgs.Handled"/>
        /// property to <see langword="true"/>.</remarks>
        public static event EventHandler<HandledEventArgs<ShowInOverlayParams>>? OverlayToolTipShowing;

        /// <summary>
        /// Occurs when the overlay tooltip should be hidden.
        /// </summary>
        /// <remarks>This event is triggered to notify subscribers that the overlay tooltip
        /// should be removed from view. Subscribers can handle this event to perform
        /// any necessary cleanup or UI updates related to the tooltip.</remarks>
        public static event EventHandler? OverlayToolTipHide;

        /// <summary>
        /// Gets or sets font used for overlay tooltips.
        /// If <see langword="null"/> (default) is specified,
        /// <see cref="AbstractControl.DefaultFont"/> is used.
        /// </summary>
        public static Font? OverlayToolTipFont { get; set; }

        /// <summary>
        /// Gets or sets the offset, in dips, applied to the position of overlay tooltips.
        /// </summary>
        public static int OverlayToolTipOffset { get; set; } = 16;

        /// <summary>
        /// Gets or sets the last <see cref="UserControl"/> that displayed a tooltip.
        /// Typically, this property is managed internally and should not be changed manually.
        /// </summary>
        public static UserControl? LastContainerWithToolTip
        {
            get => lastContainerWithToolTip.Value;
            set => lastContainerWithToolTip.Value = value;
        }

        /// <summary>
        /// Gets or sets the last control that displayed a tooltip.
        /// Typically, this property is managed internally and should not be changed manually.
        /// </summary>
        public static AbstractControl? LastControlWithToolTip
        {
            get => lastControlWithToolTip.Value;
            set => lastControlWithToolTip.Value = value;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the last tooltip displayed.
        /// Typically, this property is managed internally and should not be changed manually.
        /// </summary>
        public static ObjectUniqueId? LastToolTipId
        {
            get => lastToolTipId;
            set => lastToolTipId = value;
        }

        /// <summary>
        /// Gets or sets handler.
        /// </summary>
        public static IToolTipFactoryHandler Handler
        {
            get => handler ??= App.Handler.CreateToolTipFactoryHandler();

            set => handler = value;
        }

        /// <summary>
        /// Hides the last tooltip displayed in the overlay, if one is currently shown.
        /// </summary>
        /// <remarks>This method removes the overlay associated with the last shown tooltip
        /// and resets the internal state tracking the tooltip and its associated control.
        /// If no tooltip is currently shown, the method has no effect.</remarks>
        public static void HideLastShownInOverlay()
        {
            OverlayToolTipHide?.Invoke(null, EventArgs.Empty);

            var lastControl = LastContainerWithToolTip;
            lastControl?.RemoveOverlay(LastToolTipId);
            LastContainerWithToolTip = null;
            LastControlWithToolTip = null;
            LastToolTipId = null;
        }

        /// <summary>
        /// Binds tooltips to overlay controls, enabling the display of tooltips
        /// for controls within an overlay container.
        /// </summary>
        /// <remarks>This method sets up a global notification mechanism to handle
        /// tooltip display for controls. When a control is hovered over, the tooltip
        /// is displayed in an overlay if the control is part of an overlay container.
        /// If the control is not part of an overlay container, any previously shown tooltip in the
        /// overlay is hidden.</remarks>
        public static void BindOverlayToolTips()
        {
            if(notification != null)
                return;

            var subscriber = new ControlSubscriber();
            notification = subscriber;

            subscriber.AfterControlMouseMove += (s, e) =>
            {
                if (LastControlWithToolTip == s)
                    return;
                HideLastShownInOverlay();
            };

            subscriber.AfterControlMouseHover += (s, e) =>
            {
                if (s is not AbstractControl control)
                    return;

                var overlayParent = ControlUtils.GetParentPlatformUserControl(control);

                if (overlayParent is null)
                {
                    HideLastShownInOverlay();
                }
                else
                {
                    ShowInOverlay(control, overlayParent, control.GetRealToolTip());
                }
            };

            AbstractControl.AddGlobalNotification(notification);
        }

        /// <summary>
        /// Unbinds and removes any active overlay tooltips from the user interface.
        /// </summary>
        /// <remarks>This method hides the currently displayed overlay tooltip,
        /// if any, and removes the global notification associated with overlay tooltips.
        /// After calling this method, no overlay tooltips will be displayed
        /// until they are explicitly re-bound.</remarks>
        public static void UnbindOverlayToolTips()
        {
            HideLastShownInOverlay();

            if (notification != null)
            {
                AbstractControl.RemoveGlobalNotification(notification);
                notification = null;
            }
        }

        /// <summary>
        /// Displays the specified tooltip in an overlay.
        /// </summary>
        /// <param name="control">The control for which tooltip is displayed.</param>
        /// <param name="overlayParent">The control to use as overlay parent.
        /// Must be direct or indirect parent of <paramref name="control"/>.</param>
        /// <param name="tooltip">The text to display as a tooltip for the control.
        /// Can be <see langword="null"/> or empty if no tooltip is needed.</param>
        public static bool ShowInOverlay(
            AbstractControl? control,
            UserControl overlayParent,
            object? tooltip)
        {
            HideLastShownInOverlay();
            if (control is null || tooltip is null)
                return false;
            var tooltipStr = tooltip.ToString()?.Trim() ?? string.Empty;
            if (tooltipStr.Length == 0)
                return false;

            OverlayToolTipParams CreateData()
            {
                var font = OverlayToolTipFont ?? AbstractControl.DefaultFont;
                var pos = Mouse.GetPosition(overlayParent);

                OverlayToolTipFlags defaultOptions = OverlayToolTipFlags.DismissAfterInterval
                    | OverlayToolTipFlags.UseSystemColors | OverlayToolTipFlags.FitIntoContainer;

                var ofs = Math.Abs(OverlayToolTipOffset);

                OverlayToolTipParams data = new()
                {
                    Location = new PointD(pos.X + ofs, pos.Y + ofs),
                    LocationOffset = new PointD(ofs, ofs),
                    Text = tooltipStr,
                    Font = font,
                    Options = defaultOptions,
                    AssociatedControl = control,
                    ToolTip = tooltip,
                };

                return data;
            }

            var data = CreateData();

            if (OverlayToolTipShowing is not null)
            {
                ShowInOverlayParams initialParams = new(
                    control,
                    overlayParent,
                    tooltip,
                    data);
                var args = new HandledEventArgs<ShowInOverlayParams>(initialParams);
                OverlayToolTipShowing(null, args);
                if (args.Handled)
                    return true;
            }

            if (control.HasIndirectParent(overlayParent))
            {
                LastToolTipId = overlayParent.ShowOverlayToolTip(data);
                LastContainerWithToolTip = overlayParent;
                LastControlWithToolTip = control;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the delay between subsequent tooltips to appear.
        /// </summary>
        /// <param name="msecs">Delay in milliseconds.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        public static bool SetReshow(long msecs)
        {
            return Handler.SetReshow(msecs);
        }

        /// <summary>
        /// Enables or disables tooltips globally.
        /// </summary>
        /// <remarks>
        /// May not be supported on all platforms.
        /// </remarks>
        /// <param name="flag">Enables or disables tooltips.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        public static bool SetEnabled(bool flag)
        {
            return Handler.SetEnabled(flag);
        }

        /// <summary>
        /// Sets the delay after which the tooltip disappears or how long a tooltip remains visible.
        /// </summary>
        /// <remarks>
        /// May not be supported on all platforms (eg. wxCocoa, GTK).
        /// </remarks>
        /// <param name="msecs">Delay in milliseconds.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        public static bool SetAutoPop(long msecs)
        {
            return Handler.SetAutoPop(msecs);
        }

        /// <summary>
        /// Sets the delay after which the tooltip appears.
        /// </summary>
        /// <param name="msecs">Delay in milliseconds.</param>
        /// <remarks>
        /// May not be supported on all platforms.
        /// </remarks>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        public static bool SetDelay(long msecs)
        {
            return Handler.SetDelay(msecs);
        }

        /// <summary>
        /// Sets tooltip maximal width in pixels.
        /// </summary>
        /// <remarks>
        /// By default, tooltips are wrapped at a suitably
        /// chosen width. You can pass -1 as width to disable wrapping them completely,
        /// 0 to restore the default behavior or an arbitrary positive value to wrap
        /// them at the given width. Notice that this function does not change the width of
        /// the tooltips created before calling it. Currently this function is Windows-only.
        /// </remarks>
        /// <param name="width">ToolTip width in pixels.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        public static bool SetMaxWidth(int width)
        {
            return Handler.SetMaxWidth(width);
        }

        /// <summary>
        /// Gets tooltip using <see cref="IToolTipProvider"/>
        /// specified in the control or in one of its parents.
        /// </summary>
        public static IRichToolTip? GetToolTip(AbstractControl? control)
        {
            return control?.GetToolTipProvider()?.Get(control);
        }

        /// <summary>
        /// Shows tooltip with the specified parameters using <see cref="IToolTipProvider"/>
        /// specified in the control or in one of its parents.
        /// </summary>
        public static IRichToolTip? ShowToolTip(
            AbstractControl? control,
            object? title,
            string? message,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null,
            PointD? location = null)
        {
            return GetToolTip(control)?.ShowToolTip(title, message, icon, timeoutMilliseconds, location);
        }

        /// <summary>
        /// Represents parameters for showing a tooltip in an overlay.
        /// </summary>
        public class ShowInOverlayParams
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ShowInOverlayParams"/>
            /// class with the specified control, overlay parent, and tooltip.
            /// </summary>
            /// <param name="control">The control for which the tooltip is displayed.
            /// This parameter can be <see langword="null"/> if no control
            /// is specified.</param>
            /// <param name="overlayParent">The parent <see cref="UserControl"/> that hosts
            /// the overlay. This parameter cannot be <see langword="null"/>.</param>
            /// <param name="toolTip">The tooltip text to display for the overlay.
            /// This parameter can be <see langword="null"/> if no tooltip is required.</param>
            /// <param name="data">The parameters used to configure the overlay tooltip.</param>
            public ShowInOverlayParams(
                AbstractControl? control,
                UserControl overlayParent,
                object? toolTip,
                OverlayToolTipParams data)
            {
                Control = control;
                OverlayParent = overlayParent;
                Text = toolTip;
                ToolTip = data;
            }

            /// <summary>
            /// Gets or sets the control for which the tooltip is displayed.
            /// </summary>
            public AbstractControl? Control { get; }

            /// <summary>
            /// Gets or sets the parent control to use as the overlay parent.
            /// Must be a direct or indirect parent of <see cref="Control"/>.
            /// </summary>
            public UserControl OverlayParent { get; }

            /// <summary>
            /// Gets or sets the text to display as a tooltip for the control.
            /// Can be <see langword="null"/> or empty if no tooltip is needed.
            /// </summary>
            public object? Text { get; }

            /// <summary>
            /// Gets or sets the parameters used to configure the overlay tooltip.
            /// </summary>
            public OverlayToolTipParams ToolTip { get; set; }
        }
    }
}