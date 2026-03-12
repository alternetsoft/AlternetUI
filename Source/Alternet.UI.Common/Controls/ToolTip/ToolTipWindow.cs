using System;
using System.Collections.Generic;
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
        private static ToolTipWindow? instance;

        private readonly RichToolTip toolTip = new();
        private readonly ControlSubscriber subscriber = new();

        /// <summary>
        /// Initializes a new instance of the ToolTipWindow class.
        /// </summary>
        public ToolTipWindow()
        {
            toolTip.ToolTipVisibleChanged += (s, e) =>
            {
                if (toolTip.ToolTipVisible)
                {
                    ClientSize = toolTip.LayoutMaxSize ?? SizeD.Empty;

                    var location = toolTip.ToolTipLocation;

                    if (location is null || toolTip.ToolTipOwner is not AbstractControl locationOwner)
                    {
                        var mousePos = Mouse.GetPosition(this.ScaleFactor);

                        Location = mousePos + new PointD(ToolTipFactory.OverlayToolTipOffset);
                    }
                    else
                    {
                        Location = locationOwner.PointToScreen(location.Value);
                    }

                    Show();
                }
                else
                {
                    Hide();
                    toolTip.ToolTipOwner = null;
                    toolTip.ToolTipLocation = null;
                }
            };

            MakeToolWindowWithoutTitleBar();
            StartLocation = WindowStartLocation.Manual;

            toolTip.Parent = this;

            this.Deactivated += (s, e) =>
            {
                Hide();
            };

            toolTip.MouseDown += (s, e) =>
            {
                toolTip.ToolTipVisible = false;
            };

            subscriber.AfterControlKeyDown += (s, e) =>
            {
                if (!Visible)
                    return;
                Hide();
            };

            subscriber.AfterControlMouseLeave += (s, e) =>
            {
                if (!Visible)
                    return;
                Hide();
            };

            subscriber.AfterControlMouseEnter += (s, e) =>
            {
                if (!Visible || s == toolTip.ToolTipOwner)
                    return;
                Hide();
            };
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

        /// <inheritdoc/>
        IRichToolTip? IToolTipProvider.Get(object? sender)
        {
            toolTip.ToolTipOwner = sender;
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
    }
}
