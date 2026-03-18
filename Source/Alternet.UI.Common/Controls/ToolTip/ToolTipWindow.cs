using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a window that displays tooltips in the application, ensuring a single instance is used throughout.
    /// You don't need to create multiple instances of this class; instead, use the <see cref="Instance"/> property
    /// to access the singleton instance.
    /// </summary>
    /// <remarks>Use this class to provide consistent tooltip display functionality across the application.
    /// The singleton instance is created on first access, which enables lazy initialization. Thread safety is not
    /// guaranteed unless the underlying assignment operation is thread-safe.</remarks>
    public partial class ToolTipWindow : Window, IToolTipProvider
    {
        private readonly RichToolTip toolTip = new();
        private readonly ControlSubscriber subscriber = new();

        private static ToolTipWindow? instance;
        private static bool provideToolTipsForGenericControls;

        static ToolTipWindow()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ToolTipWindow class.
        /// </summary>
        public ToolTipWindow()
        {
            toolTip.ToolTipVisibleChanged += (s, e) =>
            {
                Hide();

                if (toolTip.ToolTipVisible)
                {
                    var size = toolTip.LayoutMaxSize ?? SizeD.Empty;

                    if (size == SizeD.Empty)
                    {
                        return;
                    }

                    size += toolTip.Margin.Size;

                    ClientSize = size;

                    toolTip.SuggestedSize = size;

                    var location = toolTip.ToolTipLocation;
                    PointD windowLcation;

                    var toolTipOffset = ToolTipFactory.OverlayToolTipOffset;

                    if (location is null || toolTip.ToolTipOwner is not AbstractControl locationOwner)
                    {
                        var mousePos = Mouse.GetPosition(this.ScaleFactor);

                        windowLcation = mousePos + new PointD(toolTipOffset);
                    }
                    else
                    {
                        windowLcation = locationOwner.PointToScreen(location.Value);
                    }

                    RectD windowRect = new(windowLcation, Size);

                    var display = Instance.GetDisplay();
                    var containerRect = display.ClientAreaDip;

                    var alignedLocation = AlignUtils.FitToolTipIntoContainer(containerRect, windowRect, new(toolTipOffset), ScaleFactor);

                    Location = alignedLocation;

                    PerformLayoutAndInvalidate();
                    Show();
                }
                else
                {
                    toolTip.ToolTipOwner = null;
                    toolTip.ToolTipLocation = null;
                }
            };

            MakeToolWindowWithoutTitleBar();
            StartLocation = WindowStartLocation.Manual;

            toolTip.IsScrollable = false;
            toolTip.Parent = this;

            this.Deactivated += HideToolTip;
            toolTip.MouseDown += HideToolTip;
            subscriber.AfterControlKeyDown += HideToolTip;
            subscriber.AfterControlMouseLeave += HideToolTip;

            subscriber.AfterControlMouseEnter += (s, e) =>
            {
                if (s == toolTip.ToolTipOwner)
                    return;
                HideToolTip(s, e);
            };
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="ToolTipWindow"/> provides tooltips for generic controls.
        /// </summary>
        /// <remarks>When enabled, tooltips will be shown for supported generic controls unless the
        /// application is running in a MAUI environment. Disabling this property removes global tooltip support for
        /// these controls.</remarks>
        public static bool ProvideToolTipsForGenericControls
        {
            get
            {
                return provideToolTipsForGenericControls;
            }

            set
            {
                if (provideToolTipsForGenericControls == value)
                    return;

                provideToolTipsForGenericControls = value;

                if (value)
                {
                    if (!App.IsMaui)
                    {
                        StaticControlEvents.MouseHover += OnGlobalMouseHover;
                    }
                }
                else
                {
                    StaticControlEvents.MouseHover -= OnGlobalMouseHover;
                }
            }
        }

        /// <summary>
        /// Gets the singleton instance of the ToolTipWindow class.
        /// </summary>
        /// <remarks>This property ensures that only one instance of ToolTipWindow is created and reused
        /// throughout the application. The instance is initialized on first access, providing a lazy-loading mechanism.
        /// This property is thread-safe only if the underlying implementation of the null-coalescing assignment is
        /// thread-safe.</remarks>
        public static ToolTipWindow Instance
        {
            get
            {
                return instance ??= new ToolTipWindow();
            }

            set
            {
                instance = value;
            }
        }

        /// <summary>
        /// Gets the rich tooltip associated with this control.
        /// </summary>
        public RichToolTip RichToolTip => toolTip;

        /// <summary>
        /// Hides the currently displayed tooltip, if any is visible.
        /// </summary>
        /// <remarks>Call this method to programmatically dismiss any tooltip that is currently shown. If
        /// no tooltip is visible, this method has no effect.</remarks>
        public static void HideGlobalToolTip()
        {
            instance?.HideToolTip(null, EventArgs.Empty);
        }

        /// <summary>
        /// Determines whether the specified object is a rich tooltip.
        /// </summary>
        /// <param name="value">The object to check.</param>
        /// <returns><see langword="true"/> if the object is a rich tooltip; otherwise, <see langword="false"/>.</returns>
        public static bool IsRichToolTip(object? value)
        {
            return value is RichToolTipParams;
        }

        /// <inheritdoc/>
        IRichToolTip? IToolTipProvider.Get(object? sender)
        {
            if (App.IsMaui)
                return null;
            Hide();
            toolTip.ToolTipOwner = sender;
            toolTip.ToolTipLocation = null;
            return toolTip;
        }

        /// <inheritdoc/>
        override protected void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible)
            {
                BindGlobalEvents();
            }
            else
            {
                UnbindGlobalEvents();
            }
        }

        /// <summary>
        /// Handles the event that occurs when the global focus changes.
        /// </summary>
        /// <param name="sender">The source of the event. This parameter can be null.</param>
        /// <param name="e">An object that contains the event data.</param>
        protected virtual void OnGlobalFocusedChanged(object? sender, EventArgs e)
        {
            if (Visible)
                Hide();
        }

        /// <summary>
        /// Binds global event handlers to enable the class to respond to application-wide changes.
        /// </summary>
        /// <remarks>Override this method in a derived class to customize which global events are handled.
        /// This method is typically called during initialization to ensure that the class can react to relevant global
        /// events.</remarks>
        protected virtual void BindGlobalEvents()
        {
            StaticControlEvents.FocusedChanged += OnGlobalFocusedChanged;
            AddGlobalNotification(subscriber);
        }

        /// <summary>
        /// Unsubscribes the handler from global focus change events to stop receiving notifications.
        /// </summary>
        /// <remarks>Override this method in a derived class to customize how global event handlers are
        /// detached. Typically called during cleanup to prevent memory leaks or unwanted event handling.</remarks>
        protected virtual void UnbindGlobalEvents()
        {
            StaticControlEvents.FocusedChanged -= OnGlobalFocusedChanged;
            RemoveGlobalNotification(subscriber);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            UnbindGlobalEvents();
            base.DisposeManaged();
        }

        /// <summary>
        /// Raises the global mouse hover event.
        /// </summary>
        /// <remarks>Override this method to provide custom handling when a global mouse hover event
        /// occurs. This method is called when the mouse pointer hovers over a relevant UI element.</remarks>
        /// <param name="sender">The source of the event, typically the object that raised the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        private static void OnGlobalMouseHover(object? sender, EventArgs e)
        {
            try
            {
                HideGlobalToolTip();

                if (sender is not AbstractControl control)
                    return;

                var toolTipObject = ToolTipFactory.GetControlToolTip(control);

                if (toolTipObject is null)
                    return;

                if (!IsRichToolTip(toolTipObject))
                {
                    if (control is not GenericControl)
                        return;
                }

                var toolTip = Instance.RichToolTip;
                toolTip.ToolTipOwner = control;
                toolTip.ToolTipLocation = null;

                var display = Instance.GetDisplay();
                var containerRect = display.ClientAreaDip;
                toolTip.MaxTextWidth = Math.Max(RichToolTip.DefaultMaxWidth ?? 0f, containerRect.Width * 0.7f);

                if (toolTipObject is string toolTipStr)
                {
                    if (string.IsNullOrEmpty(toolTipStr))
                        return;

                    Post(() => toolTip.ShowToolTip(toolTipStr));
                    return;
                }

                if (toolTipObject is RichToolTipParams prm)
                {
                    toolTip.SetParams(prm).PostShowToolTip();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in {nameof(ToolTipWindow.OnGlobalMouseHover)}: {ex}");
            }
        }

        private void HideToolTip<T>(object? sender, T e)
        {
            if (!Visible || DisposingOrDisposed)
                return;
            InsideTryCatch(() =>
            {
                toolTip.ToolTipVisible = false;
                Hide();
            });
        }
    }
}
