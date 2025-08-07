using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides static events which are related to <see cref="AbstractControl"/> and its descendants.
    /// </summary>
    public static class StaticControlEvents
    {
        /// <summary>
        /// Occurs when the control has been disposed and its resources have been released.
        /// </summary>
        /// <remarks>Subscribe to this event to be notified when the control is disposed. After disposal,
        /// further operations on the control may result in exceptions or undefined behavior.</remarks>
        public static event EventHandler? Disposed;

        /// <summary>
        /// Occurs when control's caret is changed.
        /// </summary>
        public static event EventHandler? CaretChanged;

        /// <summary>
        /// Occurs when hovered control is changed. Control is
        /// hovered when mouse pointer is over it.
        /// </summary>
        public static event EventHandler? HoveredChanged;

        /// <summary>
        /// Occurs when focused control is changed.
        /// </summary>
        public static event EventHandler? FocusedChanged;

        /// <summary>
        /// Occurs when the parent of the control changes.
        /// </summary>
        /// <remarks>
        /// This event is raised whenever the parent of a control is set to a new value,
        /// allowing subscribers to respond to changes in the control hierarchy.
        /// </remarks>
        public static event EventHandler? ParentChanged;

        /// <summary>
        /// Occurs when the control changes its <see cref="AbstractControl.Visible"/> property.
        /// </summary>
        /// <remarks>
        /// Subscribe to this event to be notified when the visibility of
        /// the control is updated.
        /// </remarks>
        public static event EventHandler? VisibleChanged;

        /// <summary>
        /// Occurs inside <see cref="AbstractControl.GetPreferredSize(SizeD)"/> method.
        /// </summary>
        /// <remarks>
        /// If default <see cref="AbstractControl.GetPreferredSize(SizeD)"/> call is not needed,
        /// set <see cref="HandledEventArgs.Handled"/>
        /// property to <c>true</c>.
        /// </remarks>
        public static event EventHandler<DefaultPreferredSizeEventArgs>? RequestPreferredSize;

        /// <summary>
        /// Occurs when the control should reposition its child controls.
        /// </summary>
        /// <remarks>
        /// If default layout is not needed, set <see cref="HandledEventArgs.Handled"/>
        /// property to <c>true</c>.
        /// </remarks>
        public static event EventHandler<DefaultLayoutEventArgs>? Layout;

        /// <summary>
        /// Gets a value indicating whether any handlers are registered
        /// for the <c>RequestPreferredSize</c> event.
        /// </summary>
        public static bool HasRequestPreferredSizeHandlers
            => RequestPreferredSize != null;

        /// <summary>
        /// Gets a value indicating whether any handlers are registered
        /// for the <c>Layout</c> event.
        /// </summary>
        public static bool HasLayoutHandlers => Layout != null;

        /// <summary>
        /// Raises the <see cref="RequestPreferredSize"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="DefaultPreferredSizeEventArgs"/> that
        /// contains the event data.</param>
        public static void RaiseRequestPreferredSize(object? sender, DefaultPreferredSizeEventArgs e)
        {
            RequestPreferredSize?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="Layout"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="DefaultLayoutEventArgs"/> that contains the event data.</param>
        public static void RaiseLayout(object? sender, DefaultLayoutEventArgs e)
        {
            Layout?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="CaretChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseCaretChanged(object? sender, EventArgs e)
        {
            CaretChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="HoveredChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseHoveredChanged(object? sender, EventArgs e)
        {
            HoveredChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="FocusedChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseFocusedChanged(object? sender, EventArgs e)
        {
            FocusedChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="Disposed"/> event to notify subscribers
        /// that a control has been disposed.
        /// </summary>
        /// <param name="sender">The source of the event, typically the control being disposed.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing event data.
        /// Typically, this is <see cref="EventArgs.Empty"/>.</param>
        public static void RaiseDisposed(object? sender, EventArgs e)
        {
            Disposed?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseParentChanged(object? sender, EventArgs e)
        {
            ParentChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="VisibleChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public static void RaiseVisibleChanged(object? sender, EventArgs e)
        {
            VisibleChanged?.Invoke(sender, e);
        }
    }
}