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
        /// Called after the <see cref="AbstractControl.PreviewKeyDown" /> event is raised.
        /// </summary>
        void AfterPreviewKeyDown(
            AbstractControl sender,
            Key key,
            ModifierKeys modifiers,
            ref bool isInputKey);

        /// <summary>
        /// Called after the <see cref="AbstractControl.LongTap" /> event is raised.
        /// </summary>
        void AfterLongTap(AbstractControl sender, LongTapEventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.SetScrollBarInfo(bool, ScrollBarInfo)" />
        /// method is called.
        /// </summary>
        void AfterSetScrollBarInfo(AbstractControl sender, bool isVertical, ScrollBarInfo value);

        /// <summary>
        /// Called after the <see cref="AbstractControl.Scroll" /> event is raised.
        /// </summary>
        void AfterScroll(AbstractControl sender, ScrollEventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.CellChanged" /> event is raised.
        /// </summary>
        void AfterCellChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.SystemColorsChanged" /> event is raised.
        /// </summary>
        void AfterSystemColorsChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.SizeChanged" /> event.
        /// </summary>
        void AfterSizeChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.Resize" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">Event arguments.</param>
        void AfterResize(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.LocationChanged" /> event is raised.
        /// </summary>
        void AfterLocationChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.BoundsChanged" /> event is raised.
        /// </summary>
        void AfterBoundsChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after a child control is inserted into
        /// the <see cref="AbstractControl.Children"/> of the <paramref name="sender"/>.
        /// </summary>
        void AfterChildInserted(AbstractControl sender, int index, AbstractControl childControl);

        /// <summary>
        /// Called after a <see cref="AbstractControl"/> is removed from the
        /// <see cref="AbstractControl.Children"/> collection of the <paramref name="sender"/>.
        /// </summary>
        void AfterChildRemoved(AbstractControl sender, AbstractControl childControl);

        /// <summary>
        /// Called after the conrol's handle is created.
        /// </summary>
        void AfterHandleCreated(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the native conrol size is changed.
        /// </summary>
        void AfterHandlerSizeChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.Activated" /> event is raised.
        /// </summary>
        void AfterActivated(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.Deactivated" /> event is raised.
        /// </summary>
        void AfterDeactivated(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the native conrol location is changed.
        /// </summary>
        void AfterContainerLocationChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the conrol's handle is destroyed.
        /// </summary>
        void AfterHandleDestroyed(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.KeyPress" /> event is raised.
        /// </summary>
        void AfterKeyPress(AbstractControl sender, KeyPressEventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.TextChanged" /> event is raised.
        /// </summary>
        void AfterTextChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the control is clicked.
        /// </summary>
        void AfterClick(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the value of the <see cref="AbstractControl.Visible"/> property changes.
        /// </summary>
        void AfterVisibleChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the control loses mouse capture.
        /// </summary>
        void AfterMouseCaptureLost(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the mouse pointer enters the control.
        /// </summary>
        void AfterMouseEnter(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the left mouse button was pressed.
        /// </summary>
        void AfterMouseLeftButtonDown(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called after the left mouse button was released.
        /// </summary>
        void AfterMouseLeftButtonUp(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called when <see cref="AbstractControl.IsMouseOver"/> property is changed.
        /// </summary>
        void AfterIsMouseOverChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the value of the <see cref="AbstractControl.Title"/> property changes.
        /// </summary>
        void AfterTitleChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called when <see cref="AbstractControl.VisualStateChanged"/> property is changed.
        /// </summary>
        void AfterVisualStateChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the mouse pointer leaves the control.
        /// </summary>
        void AfterMouseLeave(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called before the current control handler is detached.
        /// </summary>
        void AfterHandlerDetaching(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">A <see cref="PaintEventArgs" /> that contains information
        /// about the control to paint.</param>
        /// <remarks>
        /// Currently this method is provided for the compatibility and is not called.
        /// </remarks>
        void AfterPaintBackground(AbstractControl sender, PaintEventArgs e);

        /// <summary>
        /// Called after the right mouse button was pressed.
        /// </summary>
        void AfterMouseRightButtonDown(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.KeyDown" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="KeyEventArgs" /> that contains the event data.</param>
        void AfterKeyDown(AbstractControl sender, KeyEventArgs e);

        /// <summary>
        /// Called after the value of the <see cref="AbstractControl.Margin"/> property changes.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">Event arguments.</param>
        void AfterMarginChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the value of the <see cref="AbstractControl.Font"/> property changes.
        /// </summary>
        void AfterFontChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the value of the <see cref="AbstractControl.Padding"/> property changes.
        /// </summary>
        void AfterPaddingChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after a new control handler is attached.
        /// </summary>
        void AfterHandlerAttached(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.DragDrop" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="DragEventArgs" /> that contains the event data.</param>
        void AfterDragDrop(AbstractControl sender, DragEventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.DragStart" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="DragStartEventArgs" /> that contains the event data.</param>
        void AfterDragStart(AbstractControl sender, DragStartEventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.DpiChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="DpiChangedEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterDpiChanged(AbstractControl sender, DpiChangedEventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.DragOver" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="DragEventArgs" /> that contains the event data.</param>
        void AfterDragOver(AbstractControl sender, DragEventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.DragEnter" /> event is raised.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="DragEventArgs" /> that contains the event data.</param>
        void AfterDragEnter(AbstractControl sender, DragEventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.DragLeave" /> event is raised.
        /// </summary>
        void AfterDragLeave(AbstractControl sender, EventArgs e);

        /// <summary>
        /// This method is invoked when the control gets focus.
        /// </summary>
        void AfterGotFocus(AbstractControl sender, GotFocusEventArgs e);

        /// <summary>
        /// This method is invoked when the control lost focus.
        /// </summary>
        void AfterLostFocus(AbstractControl sender, LostFocusEventArgs e);

        /// <summary>
        /// Called after the mouse button was double-clicked.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMouseDoubleClick(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.MouseWheel" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMouseWheel(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called when an exception need to be processed.
        /// </summary>
        /// <param name="e">An <see cref="ThrowExceptionEventArgs"/> that contains
        /// the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterProcessException(AbstractControl sender, ThrowExceptionEventArgs e);

        /// <summary>
        /// Called after the mouse is moved.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMouseMove(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called after the mouse button was released.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        void AfterMouseUp(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called before the mouse button was released.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        void BeforeMouseUp(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Raises the <see cref="AbstractControl.ToolTipChanged"/> event.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">Event arguments.</param>
        void AfterToolTipChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called when <see cref="AbstractControl.Parent"/> property is changed.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">Event arguments.</param>
        void AfterParentChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after control is created.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">Event arguments.</param>
        void AfterCreate(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the enabled of the <see cref="AbstractControl.Enabled"/> property changes.
        /// </summary>
        /// <param name="sender">Control which sends the notification.</param>
        /// <param name="e">Event arguments.</param>
        void AfterEnabledChanged(AbstractControl sender, EventArgs e);

        /// <summary>
        /// Called after the <see cref="AbstractControl.HelpRequested" /> event is raised.</summary>
        /// <param name="e">A <see cref="HelpEventArgs" /> that
        /// contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterHelpRequested(AbstractControl sender, HelpEventArgs e);

        /// <summary>
        /// Called when <see cref="AbstractControl.Touch"/> event is raised.</summary>
        /// <param name="e">A <see cref="TouchEventArgs" /> that
        /// contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterTouch(AbstractControl sender, TouchEventArgs e);

        /// <summary>
        /// Called after the right mouse button was released.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMouseRightButtonUp(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called after the mouse button was pressed.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterMouseDown(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called before the mouse button was pressed.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void BeforeMouseDown(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called before the mouse pointer was moved.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void BeforeMouseMove(AbstractControl sender, MouseEventArgs e);

        /// <summary>
        /// Called after the control is redrawn. See <see cref="AbstractControl.Paint"/> for details.
        /// </summary>
        /// <param name="e">An <see cref="PaintEventArgs"/> that contains the
        /// event data.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterPaint(AbstractControl sender, PaintEventArgs e);

        /// <summary>
        /// Occurs during a drag-and-drop operation and enables the drag source
        /// to determine whether the drag-and-drop operation should be canceled.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterQueryContinueDrag(AbstractControl sender, QueryContinueDragEventArgs e);

        /// <summary>
        /// Called when <see cref="AbstractControl.KeyUp"/> event is raised.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <param name="sender">Control which sends the notification.</param>
        void AfterKeyUp(AbstractControl sender, KeyEventArgs e);
    }
}
