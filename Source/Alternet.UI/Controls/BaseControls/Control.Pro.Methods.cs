﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        /// <summary>
        /// Ensures that the control <see cref="Handler"/> is created,
        /// creating and attaching it if necessary.
        /// </summary>
        protected internal void EnsureHandlerCreated()
        {
            if (handler == null)
            {
                CreateAndAttachHandler();
            }
        }

        /// <summary>
        /// Disconnects the current control <see cref="Handler"/> from
        /// the control.
        /// This method calls <see cref="ControlHandler.Detach"/>.
        /// </summary>
        protected internal void DetachHandler()
        {
            if (handler == null)
                throw new InvalidOperationException();
            OnHandlerDetaching(EventArgs.Empty);
            handler.Detach();
            handler = null;
        }

        /// <summary>
        /// Raises the <see cref="SizeChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnSizeChanged(EventArgs e)
        {
            SizeChanged?.Invoke(this, e);
            OnResize(e);
        }

        /// <summary>
        /// Called when when the application finishes processing events and is
        /// about to enter the idle state.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnIdle(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnResize(EventArgs e)
        {
            Resize?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Scroll"/> event.
        /// </summary>
        /// <param name="e">A <see cref="ScrollEventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            Scroll?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="LocationChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnLocationChanged(EventArgs e) =>
            LocationChanged?.Invoke(this, e);

        /// <summary>
        /// Called when a <see cref="Control"/> is inserted into
        /// the <see cref="Control.Children"/>.
        /// </summary>
        protected virtual void OnChildInserted(Control childControl)
        {
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is removed from the
        /// <see cref="Control.Children"/> collections.
        /// </summary>
        protected virtual void OnChildRemoved(Control childControl)
        {
        }

        /// <summary>
        /// Called when the conrol's handle is created.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnHandleCreated(EventArgs e)
        {
            if (BackgroundColor is not null)
                NativeControl.BackgroundColor = BackgroundColor;
            if (ForegroundColor is not null)
                NativeControl.ForegroundColor = ForegroundColor;
        }

        /// <summary>
        /// Called when the native conrol size is changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnNativeSizeChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the conrol's handle is destroyed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnHandleDestroyed(EventArgs e)
        {
            if(measureCanvas is not null)
            {
                measureCanvas.Dispose();
                measureCanvas = null;
            }
        }

        /// <summary>
        /// Sets a specified <see cref="ControlStyles" /> flag to either <see langword="true" />
        /// or <see langword="false" />.</summary>
        /// <param name="flag">The <see cref="ControlStyles" /> bit to set.</param>
        /// <param name="value">
        /// <see langword="true" /> to apply the specified style to the control;
        /// otherwise, <see langword="false" />.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void SetStyle(ControlStyles flag, bool value)
        {
            controlStyle = value ? (controlStyle | flag) : (controlStyle & ~flag);
        }

        /// <summary>
        /// Raises the <see cref="KeyPress" /> event.
        /// </summary>
        /// <param name="e">A <see cref="KeyPressEventArgs" /> that contains the event data.</param>
        protected virtual void OnKeyPress(KeyPressEventArgs e)
        {
            KeyPress?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Control.TextChanged" /> event.</summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTextChanged(EventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the control is clicked.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control loses mouse capture.
        /// </summary>
        protected virtual void OnMouseCaptureLost(EventArgs e)
        {
            IsMouseLeftButtonDown = false;
        }

        /// <summary>
        /// Called when the mouse pointer enters the control.
        /// </summary>
        protected virtual void OnMouseEnter(EventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting the left mouse button was pressed
        /// </summary>
        protected virtual void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            IsMouseLeftButtonDown = true;
            RaiseCurrentStateChanged();
            Designer?.RaiseMouseLeftButtonDown(this, e);
            MouseLeftButtonDown?.Invoke(this, e);
        }

        /// <summary>
        ///     Virtual method reporting the left mouse button was released
        /// </summary>
        protected virtual void OnMouseLeftButtonUp(MouseEventArgs e)
        {
            IsMouseLeftButtonDown = false;
            RaiseCurrentStateChanged();
            MouseLeftButtonUp?.Invoke(this, e);
        }

        /// <summary>
        /// Called when <see cref="IsMouseOver"/> property is changed.
        /// </summary>
        protected virtual void OnIsMouseOverChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Title"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnTitleChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="CurrentStateChanged"/> property is changed.
        /// </summary>
        protected virtual void OnCurrentStateChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse pointer leaves the control.
        /// </summary>
        protected virtual void OnMouseLeave(EventArgs e)
        {
            IsMouseLeftButtonDown = false;
        }

        /// <summary>
        /// Called before the current control handler is detached.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
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
        protected virtual void OnMouseRightButtonDown(MouseEventArgs e)
        {
            MouseRightButtonDown?.Invoke(this, e);
            ShowPopupMenu(ContextMenuStrip);
        }

        /// <summary>
        /// Raises the <see cref="QueryContinueDrag" /> event.</summary>
        /// <param name="e">A <see cref="QueryContinueDragEventArgs" /> that
        /// contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            QueryContinueDrag?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="KeyUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="KeyEventArgs" /> that contains the event data.</param>
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="KeyDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="KeyEventArgs" /> that contains the event data.</param>
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
#if DEBUG
            KeyInfo.Run(KnownKeys.ShowDeveloperTools, e, DebugUtils.ShowDeveloperTools);
#endif
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            Designer?.RaiseDisposed(this);
            /*var children = Handler.AllChildren.ToArray();*/

            SuspendLayout();
            if (HasChildren)
                Children.Clear();
            /*if (Handler.HasVisualChildren)
                Handler.VisualChildren.Clear();*/
            ResumeLayout(performLayout: false);

            // TODO
            /* foreach (var child in children) child.Dispose();*/

            DetachHandler();
        }

        /// <summary>
        /// Called when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
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
        protected virtual void OnPaddingChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control is redrawn. See <see cref="Paint"/> for details.
        /// </summary>
        /// <param name="e">An <see cref="PaintEventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnPaint(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Gets default layout in case when <see cref="Layout"/> property
        /// is null.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method returns <see cref="LayoutStyle.Basic"/> in <see cref="Control"/>.
        /// </remarks>
        protected virtual LayoutStyle GetDefaultLayout()
        {
            return LayoutStyle.Basic;
        }

        /// <summary>
        /// Called after a new control handler is attached.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnHandlerAttached(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="DragDrop"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnDragDrop(DragEventArgs e)
        {
            DragDrop?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragStart"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragStartEventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnDragStart(DragStartEventArgs e)
        {
            DragStart?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragOver"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnDragOver(DragEventArgs e)
        {
            DragOver?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragEnter"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnDragEnter(DragEventArgs e)
        {
            DragEnter?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that
        /// contains the event data.</param>
        protected virtual void OnDragLeave(EventArgs e)
        {
            DragLeave?.Invoke(this, e);
        }

        /// <summary>
        /// This method is invoked when the control gets focus.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that
        /// contains the event data.</param>
        protected virtual void OnGotFocus(EventArgs e)
        {
        }

        /// <summary>
        /// This method is invoked when the control lost focus.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that
        /// contains the event data.</param>
        protected virtual void OnLostFocus(EventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting the mouse button was pressed
        /// </summary>
        protected virtual void OnMouseDoubleClick(MouseEventArgs e)
        {
            MouseDoubleClick?.Invoke(this, e);
        }

        /// <summary>
        ///     Virtual method reporting a mouse wheel rotation
        /// </summary>
        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
            MouseWheel?.Invoke(this, e);
        }

        /// <summary>
        ///     Virtual method reporting the right mouse button was released
        /// </summary>
        protected virtual void OnMouseRightButtonUp(MouseEventArgs e)
        {
            MouseRightButtonUp?.Invoke(this, e);
        }

        /// <summary>
        ///     Virtual method reporting the mouse button was pressed
        /// </summary>
        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                dragEventArgs = e;
                dragEventMousePos = e.GetPosition(this);
            }

            MouseDown?.Invoke(this, e);
        }

        /// <summary>
        /// Gets whether child control ignores layout.
        /// </summary>
        /// <param name="control"></param>
        protected virtual bool ChildIgnoresLayout(Control control)
        {
            return !control.Visible || control.IgnoreLayout;
        }

        /// <summary>
        /// Called when an exception need to be processed.
        /// </summary>
        /// <param name="e">An <see cref="ControlExceptionEventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnProcessException(ControlExceptionEventArgs e)
        {
        }

        /// <summary>
        ///     Virtual method reporting a mouse move
        /// </summary>
        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            MouseMove?.Invoke(this, e);
            if (dragEventArgs is null)
                return;
            var mousePos = e.GetPosition(this);
            var args = new DragStartEventArgs(dragEventMousePos, mousePos, dragEventArgs, e);
            RaiseDragStart(args);
            if (args.DragStarted || args.Cancel)
                dragEventArgs = null;
        }

        /// <summary>
        ///     Virtual method reporting the mouse button was released
        /// </summary>
        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            MouseUp?.Invoke(this, e);
            /*if (e.LeftButton == MouseButtonState.Released)*/
            dragEventArgs = null;
        }

        /// <summary>
        /// Raises the <see cref="ToolTipChanged"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnToolTipChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnParentChanged(EventArgs e)
        {
            Designer?.RaiseParentChanged(this);
            ParentChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="HelpRequested" /> event.</summary>
        /// <param name="e">A <see cref="HelpEventArgs" /> that
        /// contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHelpRequested(HelpEventArgs e)
        {
            if (HelpRequested != null)
            {
                HelpRequested(this, e);
                e.Handled = true;
            }

            if (!e.Handled)
                Parent?.OnHelpRequested(e);
        }

        /// <summary>
        /// Called when the enabled of the <see cref="Enabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnEnabledChanged(EventArgs e)
        {
            RaiseCurrentStateChanged();
        }

        /// <summary>
        /// Gets required control handler type.
        /// </summary>
        /// <returns></returns>
        protected virtual HandlerType GetRequiredHandlerType() => HandlerType.Native;
    }
}
