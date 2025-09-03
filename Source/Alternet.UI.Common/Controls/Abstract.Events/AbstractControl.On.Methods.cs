using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Called when the <see cref="CellChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnCellChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="ContextMenuCreated" /> event is raised.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnContextMenuCreated(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="AbstractControl.PreviewKeyDown" /> event is raised.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPreviewKeyDown(
            Key key,
            ModifierKeys modifiers,
            ref bool isInputKey)
        {
        }

        /// <summary>
        /// Called when the <see cref="LongTap" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnLongTap(LongTapEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="SystemColorsChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnSystemColorsChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnSizeChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="Resize" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnResize(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="BoundsChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBoundsChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="LocationChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnLocationChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when a <see cref="AbstractControl"/> is inserted into
        /// the <see cref="AbstractControl.Children"/>.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnChildInserted(int index, AbstractControl childControl)
        {
        }

        /// <summary>
        /// Called when a <see cref="AbstractControl"/> is removed from the
        /// <see cref="AbstractControl.Children"/> collections.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnChildRemoved(AbstractControl childControl)
        {
        }

        /// <summary>
        /// Called when the control's handle is created.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandleCreated(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the native control size is changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandlerSizeChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="Activated" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnActivated(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="Deactivated" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDeactivated(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="BeforeHandleDestroyed" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBeforeHandleDestroyed(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the native control location is changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandlerLocationChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control's handle is destroyed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandleDestroyed(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="KeyPress" /> event is raised.
        /// </summary>
        /// <param name="e">A <see cref="KeyPressEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnKeyPress(KeyPressEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="MouseCaptureChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseCaptureChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="Invalidated" /> event is raised.
        /// </summary>
        /// <param name="e">A <see cref="InvalidateEventArgs" /> that contains
        /// the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        protected virtual void OnInvalidated(InvalidateEventArgs e)
        {
        }

        /// <summary>
        /// Called from the constructor. This method is added for the compatibility
        /// with legacy code.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnCreateControl()
        {
        }

        /// <summary>
        /// Called when the <see cref="TextChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTextChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control is clicked.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnVisibleChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control loses mouse capture.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseCaptureLost(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse pointer enters the control.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseEnter(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the left mouse button was pressed.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseLeftButtonDown(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when the left mouse button was released.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseLeftButtonUp(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="IsMouseOver"/> property is changed.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnIsMouseOverChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Title"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTitleChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="VisualStateChanged"/> property is changed.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnVisualStateChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse pointer leaves the control.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseLeave(EventArgs e)
        {
        }

        /// <summary>
        /// Called before the current control handler is detached.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandlerDetaching(EventArgs e)
        {
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="e">
        /// A <see cref="PaintEventArgs" /> that contains information about
        /// the control to paint.
        /// </param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        /// <remarks>
        /// This method is not called for all the controls.
        /// It is up to the control to decide whether and how to call this method.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPaintBackground(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Called when the right mouse button was pressed.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseRightButtonDown(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="KeyDown" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="KeyEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMarginChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Font"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnFontChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Padding"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPaddingChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called after a new control handler is attached.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandlerAttached(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="DragDrop" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="DragEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDragDrop(DragEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="DragStart" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="DragStartEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDragStart(DragStartEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="DpiChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="DpiChangedEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDpiChanged(DpiChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="DragOver" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="DragEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDragOver(DragEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="DragEnter" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="DragEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDragEnter(DragEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="DragLeave" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDragLeave(EventArgs e)
        {
        }

        /// <summary>
        /// This method is invoked when the control gets focus.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that
        /// contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnGotFocus(EventArgs e)
        {
        }

        /// <summary>
        /// This method is invoked when the control lost focus.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that
        /// contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnLostFocus(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse button was double-clicked.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseDoubleClick(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="MouseWheel" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when an exception need to be processed.
        /// </summary>
        /// <param name="e">An <see cref="ThrowExceptionEventArgs"/> that contains
        /// the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnProcessException(ThrowExceptionEventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse is moved.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseMove(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse button was released.
        /// </summary>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseUp(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="ToolTipChanged"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnToolTipChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Parent"/> property is changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Handles the event triggered when the visibility of a sibling control changes.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnSiblingVisibleChanged(AbstractControl sibling)
        {
        }

        /// <summary>
        /// Called when the enabled of the <see cref="Enabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnEnabledChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="HelpRequested" /> event is raised.</summary>
        /// <param name="e">A <see cref="HelpEventArgs" /> that
        /// contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHelpRequested(HelpEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Touch"/> event is raised.</summary>
        /// <param name="e">A <see cref="TouchEventArgs" /> that
        /// contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTouch(TouchEventArgs e)
        {
        }

        /// <summary>
        /// Called when the right mouse button was released.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseRightButtonUp(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse pointer hovers over the control.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMouseHover(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse button was pressed.
        /// </summary>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        /// <param name="e">An <see cref="MouseEventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseDown(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when the control is redrawn. See <see cref="Paint"/> for details.
        /// </summary>
        /// <param name="e">An <see cref="PaintEventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPaint(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Occurs during a drag-and-drop operation and enables the drag source
        /// to determine whether the drag-and-drop operation should be canceled.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="KeyUp"/> event is raised.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <remarks>Derived classes can override this method to handle the event without
        /// attaching a delegate.</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
        }
    }
}
