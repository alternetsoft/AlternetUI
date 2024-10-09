using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which are called when control events are raised
    /// or control methods are called. Implement this interface or use <see cref="ControlNotification"/>
    /// descendant in order to extend control behavior.
    /// </summary>
    public interface IControlNotification : IDisposable
    {
        /// <summary>
        /// Called after the <see cref="Control.PreviewKeyDown" /> event is raised.
        /// </summary>
        void AfterPreviewKeyDown(Control sender, Key key, ModifierKeys modifiers, ref bool isInputKey);

        /// <summary>
        /// Called after the <see cref="Control.LongTap" /> event is raised.
        /// </summary>
        void AfterLongTap(Control sender, LongTapEventArgs e);

        /// <summary>
        /// Called after the <see cref="Control.SetScrollBarInfo(bool, ScrollBarInfo)" />
        /// method is called.
        /// </summary>
        void AfterSetScrollBarInfo(Control sender, bool isVertical, ScrollBarInfo value);

        /// <summary>
        /// Called after the <see cref="Control.Scroll" /> event is raised.
        /// </summary>
        void AfterScroll(Control sender, ScrollEventArgs e);

        /// <summary>
        /// Called after the <see cref="Control.CellChanged" /> event is raised.
        /// </summary>
        void AfterCellChanged(Control sender);

        /// <summary>
        /// Called after the <see cref="Control.SystemColorsChanged" /> event is raised.
        /// </summary>
        void AfterSystemColorsChanged(Control sender);

        /// <summary>
        /// Called after the <see cref="Control.SizeChanged" /> event.
        /// </summary>
        void AfterSizeChanged(Control sender);

        /// <summary>
        /// Called when when the application finishes processing events and is
        /// about to enter the idle state.
        /// </summary>
        void AfterIdle(Control sender);

        /// <summary>
        /// Called after the <see cref="Control.Resize" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterResize(Control sender);

        /// <summary>
        /// Called after the <see cref="Control.LocationChanged" /> event is raised.
        /// </summary>
        void AfterLocationChanged(Control sender);

        /// <summary>
        /// Called after a child control is inserted into
        /// the <see cref="Control.Children"/> of the <paramref name="sender"/>.
        /// </summary>
        void AfterChildInserted(Control sender, int index, Control childControl);

        /// <summary>
        /// Called after a <see cref="Control"/> is removed from the
        /// <see cref="Control.Children"/> collection of the <paramref name="sender"/>.
        /// </summary>
        void AfterChildRemoved(Control sender, Control childControl);

        /// <summary>
        /// Called after the conrol's handle is created.
        /// </summary>
        void AfterHandleCreated(Control sender);

        /// <summary>
        /// Called after the native conrol size is changed.
        /// </summary>
        void AfterHandlerSizeChanged(Control sender);

        /// <summary>
        /// Called after the <see cref="Control.Activated" /> event is raised.
        /// </summary>
        void AfterActivated(Control sender);

        /// <summary>
        /// Called after the <see cref="Control.Deactivated" /> event is raised.
        /// </summary>
        void AfterDeactivated(Control sender);

        /// <summary>
        /// Called after the native conrol location is changed.
        /// </summary>
        void AfterHandlerLocationChanged(Control sender);

        /// <summary>
        /// Called after the conrol's handle is destroyed.
        /// </summary>
        void AfterHandleDestroyed(Control sender);

        /// <summary>
        /// Called after the <see cref="Control.KeyPress" /> event is raised.
        /// </summary>
        void AfterKeyPress(Control sender, KeyPressEventArgs e);

        /// <summary>
        /// Called after the <see cref="Control.TextChanged" /> event is raised.
        /// </summary>
        void AfterTextChanged(Control sender);

        /// <summary>
        /// Called after the control is clicked.
        /// </summary>
        void AfterClick(Control sender);

        /// <summary>
        /// Called after the value of the <see cref="Control.Visible"/> property changes.
        /// </summary>
        void AfterVisibleChanged(Control sender);

        /// <summary>
        /// Called after the control loses mouse capture.
        /// </summary>
        void AfterMouseCaptureLost(Control sender);

        /// <summary>
        /// Called after the mouse pointer enters the control.
        /// </summary>
        void AfterMouseEnter(Control sender);

        /// <summary>
        /// Called after the left mouse button was pressed.
        /// </summary>
        void AfterMouseLeftButtonDown(Control sender, MouseEventArgs e);

        /// <summary>
        /// Called after the left mouse button was released.
        /// </summary>
        void AfterMouseLeftButtonUp(Control sender, MouseEventArgs e);

        /// <summary>
        /// Called when <see cref="Control.IsMouseOver"/> property is changed.
        /// </summary>
        void AfterIsMouseOverChanged(Control sender);

        /// <summary>
        /// Called after the value of the <see cref="Control.Title"/> property changes.
        /// </summary>
        void AfterTitleChanged(Control sender);

        /// <summary>
        /// Called when <see cref="Control.VisualStateChanged"/> property is changed.
        /// </summary>
        void AfterVisualStateChanged(Control sender);

        /// <summary>
        /// Called after the mouse pointer leaves the control.
        /// </summary>
        void AfterMouseLeave(Control sender);

        /// <summary>
        /// Called before the current control handler is detached.
        /// </summary>
        void AfterHandlerDetaching(Control sender);

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">A <see cref="PaintEventArgs" /> that contains information
        /// about the control to paint.</param>
        /// <remarks>
        /// Currently this method is provided for the compatibility and is not called.
        /// </remarks>
        void AfterPaintBackground(Control sender, PaintEventArgs e);

        /// <summary>
        /// Called after the right mouse button was pressed.
        /// </summary>
        void AfterMouseRightButtonDown(Control sender, MouseEventArgs e);

        /// <summary>
        /// Called after the <see cref="Control.KeyDown" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="KeyEventArgs" /> that contains the event data.</param>
        void AfterKeyDown(Control sender, KeyEventArgs e);

        /// <summary>
        /// Called after the value of the <see cref="Control.Margin"/> property changes.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMarginChanged(Control sender);

        /// <summary>
        /// Called after the value of the <see cref="Control.Font"/> property changes.
        /// </summary>
        void AfterFontChanged(Control sender);

        /// <summary>
        /// Called after the value of the <see cref="Control.Padding"/> property changes.
        /// </summary>
        void AfterPaddingChanged(Control sender);

        /// <summary>
        /// Called after a new control handler is attached.
        /// </summary>
        void AfterHandlerAttached(Control sender);

        /// <summary>
        /// Called after the <see cref="Control.DragDrop" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="DragEventArgs" /> that contains the event data.</param>
        void AfterDragDrop(Control sender, DragEventArgs e);

        /// <summary>
        /// Called after the <see cref="Control.DragStart" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="DragStartEventArgs" /> that contains the event data.</param>
        void AfterDragStart(Control sender, DragStartEventArgs e);

        /// <summary>
        /// Called after the <see cref="Control.DpiChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="DpiChangedEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterDpiChanged(Control sender, DpiChangedEventArgs e);

        /// <summary>
        /// Called after the <see cref="Control.DragOver" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="DragEventArgs" /> that contains the event data.</param>
        void AfterDragOver(Control sender, DragEventArgs e);

        /// <summary>
        /// Called after the <see cref="Control.DragEnter" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="DragEventArgs" /> that contains the event data.</param>
        void AfterDragEnter(Control sender, DragEventArgs e);

        /// <summary>
        /// Called after the <see cref="Control.DragLeave" /> event is raised.
        /// </summary>
        void AfterDragLeave(Control sender);

        /// <summary>
        /// This method is invoked when the control gets focus.
        /// </summary>
        void AfterGotFocus(Control sender);

        /// <summary>
        /// This method is invoked when the control lost focus.
        /// </summary>
        void AfterLostFocus(Control sender);

        /// <summary>
        /// Called after the mouse button was double-clicked.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMouseDoubleClick(Control sender, MouseEventArgs e);

        /// <summary>
        /// Called after the <see cref="Control.MouseWheel" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMouseWheel(Control sender, MouseEventArgs e);

        /// <summary>
        /// Called when an exception need to be processed.
        /// </summary>
        /// <param name="e">An <see cref="ThrowExceptionEventArgs"/> that contains
        /// the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterProcessException(Control sender, ThrowExceptionEventArgs e);

        /// <summary>
        /// Called after the mouse is moved.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMouseMove(Control sender, MouseEventArgs e);

        /// <summary>
        /// Called after the mouse button was released.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        void AfterMouseUp(Control sender, MouseEventArgs e);

        /// <summary>
        /// Raises the <see cref="Control.ToolTipChanged"/> event.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterToolTipChanged(Control sender);

        /// <summary>
        /// Called when <see cref="Control.Parent"/> property is changed.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterParentChanged(Control sender);

        /// <summary>
        /// Called after the enabled of the <see cref="Control.Enabled"/> property changes.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterEnabledChanged(Control sender);

        /// <summary>
        /// Called after the <see cref="Control.HelpRequested" /> event is raised.</summary>
        /// <param name="e">A <see cref="HelpEventArgs" /> that
        /// contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterHelpRequested(Control sender, HelpEventArgs e);

        /// <summary>
        /// Called when <see cref="Control.Touch"/> event is raised.</summary>
        /// <param name="e">A <see cref="TouchEventArgs" /> that
        /// contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterTouch(Control sender, TouchEventArgs e);

        /// <summary>
        /// Called after the right mouse button was released.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMouseRightButtonUp(Control sender, MouseEventArgs e);

        /// <summary>
        /// Called after the mouse button was pressed.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMouseDown(Control sender, MouseEventArgs e);

        /// <summary>
        /// Called after the control is redrawn. See <see cref="Control.Paint"/> for details.
        /// </summary>
        /// <param name="e">An <see cref="PaintEventArgs"/> that contains the
        /// event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterPaint(Control sender, PaintEventArgs e);

        /// <summary>
        /// Occurs during a drag-and-drop operation and enables the drag source
        /// to determine whether the drag-and-drop operation should be canceled.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterQueryContinueDrag(Control sender, QueryContinueDragEventArgs e);

        /// <summary>
        /// Called when <see cref="Control.KeyUp"/> event is raised.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterKeyUp(Control sender, KeyEventArgs e);
    }
}
