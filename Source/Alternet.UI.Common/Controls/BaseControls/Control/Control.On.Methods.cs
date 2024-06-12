﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class Control
    {
        /// <summary>
        /// Raises the <see cref="CellChanged" /> event.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual void OnCellChanged()
        {
            CellChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the <see cref="SystemColorsChanged" /> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnSystemColorsChanged(EventArgs e)
        {

        }

        /// <summary>
        /// Called when the <see cref="SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnSizeChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when when the application finishes processing events and is
        /// about to enter the idle state.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnIdle(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnResize(EventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnLocationChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is inserted into
        /// the <see cref="Control.Children"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnChildInserted(int index, Control childControl)
        {
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is removed from the
        /// <see cref="Control.Children"/> collections.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnChildRemoved(Control childControl)
        {
        }

        /// <summary>
        /// Called when the conrol's handle is created.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandleCreated(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the native conrol size is changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandlerSizeChanged(EventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnActivated(EventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDeactivated(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the native conrol location is changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandlerLocationChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the conrol's handle is destroyed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandleDestroyed(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="KeyPress" /> event.
        /// </summary>
        /// <param name="e">A <see cref="KeyPressEventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnKeyPress(KeyPressEventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTextChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control is clicked.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnVisibleChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control loses mouse capture.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseCaptureLost(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse pointer enters the control.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseEnter(EventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting the left mouse button was pressed
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseLeftButtonDown(MouseEventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting the left mouse button was released
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseLeftButtonUp(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="IsMouseOver"/> property is changed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnIsMouseOverChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Title"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTitleChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="VisualStateChanged"/> property is changed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnVisualStateChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse pointer leaves the control.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseLeave(EventArgs e)
        {
        }

        /// <summary>
        /// Called before the current control handler is detached.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandlerDetaching(EventArgs e)
        {
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="e">A <see cref="PaintEventArgs" /> that contains information
        /// about the control to paint.</param>
        /// <remarks>
        /// Currently this method is provided for the compatibility and is not called.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPaintBackground(PaintEventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting the right mouse button was pressed
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseRightButtonDown(MouseEventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMarginChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Font"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnFontChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Padding"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPaddingChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called after a new control handler is attached.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandlerAttached(EventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDragDrop(DragEventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDragStart(DragStartEventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDpiChanged(DpiChangedEventArgs e)
        {

        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDragOver(DragEventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDragEnter(DragEventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDragLeave(EventArgs e)
        {
        }

        /// <summary>
        /// This method is invoked when the control gets focus.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that
        /// contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnGotFocus(EventArgs e)
        {
        }

        /// <summary>
        /// This method is invoked when the control lost focus.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that
        /// contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnLostFocus(EventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting the mouse button was pressed
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseDoubleClick(MouseEventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting a mouse wheel rotation
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when an exception need to be processed.
        /// </summary>
        /// <param name="e">An <see cref="ThrowExceptionEventArgs"/> that contains
        /// the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnProcessException(ThrowExceptionEventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting a mouse move
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseMove(MouseEventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting the mouse button was released
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseUp(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="ToolTipChanged"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnToolTipChanged(EventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the enabled of the <see cref="Enabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnEnabledChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="HelpRequested" /> event.</summary>
        /// <param name="e">A <see cref="HelpEventArgs" /> that
        /// contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHelpRequested(HelpEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Touch"/> event is raised.</summary>
        /// <param name="e">A <see cref="TouchEventArgs" /> that
        /// contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTouch(TouchEventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting the right mouse button was released
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseRightButtonUp(MouseEventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting the mouse button was pressed
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseDown(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when the control is redrawn. See <see cref="Paint"/> for details.
        /// </summary>
        /// <param name="e">An <see cref="PaintEventArgs"/> that contains the
        /// event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPaint(PaintEventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
        }
    }
}
