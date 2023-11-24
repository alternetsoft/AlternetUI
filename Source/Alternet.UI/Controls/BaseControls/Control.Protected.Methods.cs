using System;
using System.Collections.Generic;
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
        protected virtual void OnSizeChanged(EventArgs e) =>
            SizeChanged?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="LocationChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnLocationChanged(EventArgs e) =>
            LocationChanged?.Invoke(this, e);

        /// <summary>
        /// Called when a <see cref="Control"/> is inserted into
        /// the <see cref="Control.Children"/> or
        /// <see cref="ControlHandler.VisualChildren"/> collection.
        /// </summary>
        protected virtual void OnChildInserted(Control childControl)
        {
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is removed from the
        /// <see cref="Control.Children"/> or
        /// <see cref="ControlHandler.VisualChildren"/> collections.
        /// </summary>
        protected virtual void OnChildRemoved(Control childControl)
        {
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
        /// Called when the application is in idle state.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnIdle(EventArgs e)
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
        protected virtual void OnMouseCaptureLost()
        {
        }

        /// <summary>
        /// Called when the mouse pointer enters the control.
        /// </summary>
        protected virtual void OnMouseEnter()
        {
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            IsMouseLeftButtonDown = true;
            RaiseCurrentStateChanged();
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            IsMouseLeftButtonDown = false;
            RaiseCurrentStateChanged();
        }

        /// <summary>
        /// Called when <see cref="IsMouseOver"/> property is changed.
        /// </summary>
        protected virtual void OnIsMouseOverChanged()
        {
        }

        /// <summary>
        /// Called when <see cref="CurrentStateChanged"/> property is changed.
        /// </summary>
        protected virtual void OnCurrentStateChanged()
        {
        }

        /// <summary>
        /// Called when the mouse pointer leaves the control.
        /// </summary>
        protected virtual void OnMouseLeave()
        {
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
        /// Gets an <see cref="IControlHandlerFactory"/> to use when creating
        /// new control handlers for this control.
        /// </summary>
        protected IControlHandlerFactory GetEffectiveControlHandlerHactory() =>
            ControlHandlerFactory ??
                Application.Current.VisualTheme.ControlHandlerFactory;

        /// <summary>
        /// Creates a handler for the control.
        /// </summary>
        /// <remarks>
        /// You typically should not call the <see cref="CreateHandler"/>
        /// method directly.
        /// The preferred method is to call the
        /// <see cref="EnsureHandlerCreated"/> method, which forces a handler
        /// to be created for the control.
        /// </remarks>
        protected virtual ControlHandler CreateHandler()
        {
            return new GenericControlHandler();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
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
            if (Handler.HasVisualChildren)
                Handler.VisualChildren.Clear();
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

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed && e.Source == this)
            {
                dragEventArgs = e;
                dragEventMousePos = e.GetPosition(this);
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (dragEventArgs is null || e.Source != this)
                return;
            var mousePos = e.GetPosition(this);
            var args = new DragStartEventArgs(dragEventMousePos, mousePos, dragEventArgs, e);
            RaiseDragStart(args);
            if (args.DragStarted || args.Cancel)
                dragEventArgs = null;
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            /*if (e.LeftButton == MouseButtonState.Released && e.Source == this)*/
            dragEventArgs = null;
        }

        /// <summary>
        /// Raises the <see cref="ToolTipChanged"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnToolTipChanged(EventArgs e)
        {
            ToolTipChanged?.Invoke(this, e);
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
        /// Called when the enabled of the <see cref="Enabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnEnabledChanged(EventArgs e)
        {
            RaiseCurrentStateChanged();
        }
    }
}
