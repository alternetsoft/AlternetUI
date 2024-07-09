using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class Control
    {
        /// <summary>
        /// Raises the <see cref="CellChanged" /> event and <see cref="OnCellChanged"/> method.
        /// </summary>
        public void RaiseCellChanged()
        {
            CellChanged?.Invoke(this, EventArgs.Empty);
            OnCellChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="Idle"/> event and calls
        /// <see cref="OnIdle(EventArgs)"/>.
        /// See <see cref="Idle"/> event description for more details.
        /// </summary>
        public void RaiseIdle()
        {
            OnIdle(EventArgs.Empty);
            Idle?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="MouseMove" /> event and <see cref="OnMouseMove"/> method.
        /// </summary>
        public void RaiseMouseMove(MouseEventArgs e)
        {
            HoveredControl = this;
            MouseMove?.Invoke(this, e);
            if (dragEventArgs is null)
                return;
            var mousePos = Mouse.GetPosition(this);
            var args = new DragStartEventArgs(dragEventMousePos, mousePos, dragEventArgs, e);
            RaiseDragStart(args);
            if (args.DragStarted || args.Cancel)
                dragEventArgs = null;
            OnMouseMove(e);
        }

        /// <summary>
        /// Raises the <see cref="HelpRequested" /> event and <see cref="OnHelpRequested"/> method.
        /// </summary>
        public void RaiseHelpRequested(HelpEventArgs e)
        {
            OnHelpRequested(e);
            HelpRequested?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/>event and <see cref="OnParentChanged"/> .
        /// </summary>
        public void RaiseParentChanged()
        {
            Designer?.RaiseParentChanged(this);
            ParentChanged?.Invoke(this, EventArgs.Empty);
            OnParentChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="MouseUp" /> event and <see cref="OnMouseUp"/> method.
        /// </summary>
        public void RaiseMouseUp(MouseEventArgs e)
        {
            HoveredControl = this;

            MouseUp?.Invoke(this, e);
            dragEventArgs = null;

            OnMouseUp(e);

            if (e.ChangedButton == MouseButton.Left)
            {
                RaiseMouseLeftButtonUp(e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                RaiseMouseRightButtonUp(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseLeftButtonUp" /> event and <see cref="OnMouseLeftButtonUp"/> method.
        /// </summary>
        public void RaiseMouseLeftButtonUp(MouseEventArgs e)
        {
            IsMouseLeftButtonDown = false;
            RaiseVisualStateChanged();
            MouseLeftButtonUp?.Invoke(this, e);
            OnMouseLeftButtonUp(e);
        }

        /// <summary>
        /// Raises the <see cref="MouseRightButtonUp" /> event
        /// and <see cref="OnMouseRightButtonUp"/> method.
        /// </summary>
        public void RaiseMouseRightButtonUp(MouseEventArgs e)
        {
            MouseRightButtonUp?.Invoke(this, e);
            OnMouseRightButtonUp(e);
        }

        /// <summary>
        /// Raises the <see cref="MouseDown" /> event and <see cref="OnMouseDown"/> method.
        /// </summary>
        public void RaiseMouseDown(MouseEventArgs e)
        {
            HoveredControl = this;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                dragEventArgs = e;
                dragEventMousePos = Mouse.GetPosition(this);
            }

            MouseDown?.Invoke(this, e);

            OnMouseDown(e);

            if (e.ChangedButton == MouseButton.Left)
            {
                RaiseMouseLeftButtonDown(e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                RaiseMouseRightButtonDown(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="GotFocus" /> event and <see cref="OnGotFocus"/> method.
        /// </summary>
        public void RaiseGotFocus()
        {
            FocusedControl = this;
            OnGotFocus(EventArgs.Empty);
            GotFocus?.Invoke(this, EventArgs.Empty);
            Designer?.RaiseGotFocus(this);
            RaiseVisualStateChanged();

            if (CaretInfo is not null)
            {
                CaretInfo.ControlFocused = true;
                InvalidateCaret();
            }
        }

        /// <summary>
        /// Raises the <see cref="LostFocus" /> event and <see cref="OnLostFocus"/> method.
        /// </summary>
        public void RaiseLostFocus()
        {
            if (FocusedControl == this)
                FocusedControl = null;
            OnLostFocus(EventArgs.Empty);
            LostFocus?.Invoke(this, EventArgs.Empty);
            RaiseVisualStateChanged();

            if (CaretInfo is not null)
            {
                CaretInfo.ControlFocused = false;
                Invalidate();
            }
        }

        /// <summary>
        /// Raises the <see cref="Activated" /> event and <see cref="OnActivated"/> method.
        /// </summary>
        public void RaiseActivated()
        {
            OnActivated(EventArgs.Empty);
            Activated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="OnHandlerLocationChanged" /> and <see cref="ReportBoundsChanged"/>
        /// methods.
        /// </summary>
        public void RaiseHandlerLocationChanged()
        {
            OnHandlerLocationChanged(EventArgs.Empty);
            ReportBoundsChanged();
        }

        /// <summary>
        /// Raises the <see cref="Deactivated" /> event and <see cref="OnDeactivated"/> method.
        /// </summary>
        public void RaiseDeactivated()
        {
            OnDeactivated(EventArgs.Empty);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="SystemColorsChanged" /> event
        /// and <see cref="OnSystemColorsChanged"/> method.
        /// </summary>
        public void RaiseSystemColorsChanged()
        {
            SystemColorsChanged?.Invoke(this, EventArgs.Empty);
            OnSystemColorsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="QueryContinueDrag" /> event
        /// and <see cref="OnQueryContinueDrag"/> method.
        /// </summary>
        public void RaiseQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            QueryContinueDrag?.Invoke(this, e);
            OnQueryContinueDrag(e);
        }

        /// <summary>
        /// Raises the <see cref="HandleCreated" /> event and <see cref="OnHandleCreated"/> method.
        /// </summary>
        public void RaiseHandleCreated()
        {
            if (BackgroundColor is not null)
                Handler.BackgroundColor = BackgroundColor;
            if (ForegroundColor is not null)
                Handler.ForegroundColor = ForegroundColor;
            OnHandleCreated(EventArgs.Empty);
            HandleCreated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="HandleDestroyed" /> event and <see cref="OnHandleDestroyed"/> method.
        /// </summary>
        public void RaiseHandleDestroyed()
        {
            ResetScaleFactor();
            OnHandleDestroyed(EventArgs.Empty);
            HandleDestroyed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="MouseCaptureLost" /> event and <see cref="OnMouseCaptureLost"/> method.
        /// </summary>
        public void RaiseMouseCaptureLost()
        {
            if (HoveredControl == this)
                HoveredControl = null;
            IsMouseLeftButtonDown = false;
            OnMouseCaptureLost(EventArgs.Empty);
            MouseCaptureLost?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="TextChanged" /> event.</summary>
        public void RaiseTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
            OnTextChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="SizeChanged"/>, <see cref="Resize"/> events and
        /// <see cref="OnSizeChanged"/>, <see cref="OnResize"/> methods.
        /// </summary>
        public void RaiseSizeChanged()
        {
            OnSizeChanged(EventArgs.Empty);
            SizeChanged?.Invoke(this, EventArgs.Empty);
            Resize?.Invoke(this, EventArgs.Empty);
            OnResize(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="MouseEnter" /> event and <see cref="OnMouseEnter"/> method.
        /// </summary>
        public void RaiseMouseEnter()
        {
            IsMouseOver = true;
            HoveredControl = this;
            RaiseIsMouseOverChanged();
            OnMouseEnter(EventArgs.Empty);
            MouseEnter?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="VisualStateChanged" /> event
        /// and <see cref="OnVisualStateChanged"/> method.
        /// </summary>
        public void RaiseVisualStateChanged()
        {
            OnVisualStateChanged(EventArgs.Empty);
            VisualStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseIsMouseOverChanged()
        {
            OnIsMouseOverChanged(EventArgs.Empty);
            IsMouseOverChanged?.Invoke(this, EventArgs.Empty);
            RaiseVisualStateChanged();
        }

        public void RaiseMouseLeave()
        {
            IsMouseOver = false;
            if (HoveredControl == this)
                HoveredControl = null;
            RaiseIsMouseOverChanged();
            IsMouseLeftButtonDown = false;
            OnMouseLeave(EventArgs.Empty);
            MouseLeave?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseChildInserted(int index, Control childControl)
        {
            OnChildInserted(index, childControl);
            Handler.OnChildInserted(childControl);
            ChildInserted?.Invoke(this, new BaseEventArgs<Control>(childControl));
        }

        public void RaiseChildRemoved(Control childControl)
        {
            OnChildRemoved(childControl);
            Handler.OnChildRemoved(childControl);
            ChildRemoved?.Invoke(this, new BaseEventArgs<Control>(childControl));
        }

        public void RaisePaint(PaintEventArgs e)
        {
            OnPaint(e);
            Paint?.Invoke(this, e);

            PaintCaret(e);

            PlessMouse.DrawTestMouseRect(this, e.Graphics);
        }

        /// <summary>
        /// Raises the <see cref="LocationChanged"/> event.
        /// </summary>
        public void RaiseLocationChanged()
        {
            OnLocationChanged(EventArgs.Empty);
            LocationChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="DragStart"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragStartEventArgs"/> that contains the
        /// event data.</param>
        public void RaiseDragStart(DragStartEventArgs e)
        {
            OnDragStart(e);
            DragStart?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragDrop"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> that contains the
        /// event data.</param>
        public void RaiseDragDrop(DragEventArgs e)
        {
            OnDragDrop(e);
            DragDrop?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragOver"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> that contains the
        /// event data.</param>
        public void RaiseDragOver(DragEventArgs e)
        {
            OnDragOver(e);
            DragOver?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DpiChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DpiChangedEventArgs"/> that contains the
        /// event data.</param>
        public void RaiseDpiChanged(DpiChangedEventArgs e)
        {
            ResetScaleFactor();
            OnDpiChanged(e);
            DpiChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragEnter"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseDragEnter(DragEventArgs e)
        {
            OnDragEnter(e);
            DragEnter?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragLeave"/> event.
        /// </summary>
        public void RaiseDragLeave()
        {
            DragLeave?.Invoke(this, EventArgs.Empty);
            OnDragLeave(EventArgs.Empty);
        }

        public void RaiseHandlerSizeChanged()
        {
            OnHandlerSizeChanged(EventArgs.Empty);
            ReportBoundsChanged();
        }

        public void RaiseMouseLeftButtonDown(MouseEventArgs e)
        {
            IsMouseLeftButtonDown = true;
            RaiseVisualStateChanged();
            Designer?.RaiseMouseLeftButtonDown(this, e);
            MouseLeftButtonDown?.Invoke(this, e);
            OnMouseLeftButtonDown(e);
        }

        public void RaiseMouseRightButtonDown(MouseEventArgs e)
        {
            MouseRightButtonDown?.Invoke(this, e);
            ShowPopupMenu(ContextMenuStrip);
            OnMouseRightButtonDown(e);
        }

        public void RaiseKeyUp(KeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
            OnKeyUp(e);
        }

        public void RaiseKeyPress(KeyPressEventArgs e)
        {
            KeyPress?.Invoke(this, e);
            OnKeyPress(e);
        }

        public void RaiseMouseWheel(MouseEventArgs e)
        {
            HoveredControl = this;
            OnMouseWheel(e);
            MouseWheel?.Invoke(this, e);
        }

        public void RaiseMouseDoubleClick(MouseEventArgs e)
        {
            LastDoubleClickTimestamp = e.Timestamp;
            OnMouseDoubleClick(e);
            MouseDoubleClick?.Invoke(this, e);
        }

        public void RaiseKeyDown(KeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
            OnKeyDown(e);
#if DEBUG
            if (!e.Handled)
                KeyInfo.Run(KnownShortcuts.ShowDeveloperTools, e, DialogFactory.ShowDeveloperTools);
#endif
        }

        /// <summary>
        /// Raises the <see cref="Click"/> event and calls
        /// <see cref="OnClick(EventArgs)"/>.
        /// See <see cref="Click"/> event description for more details.
        /// </summary>
        public void RaiseClick()
        {
            OnClick(EventArgs.Empty);
            Click?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="Touch"/> event and calls
        /// <see cref="OnTouch"/> method.
        /// </summary>
        /// <param name="e">An <see cref="TouchEventArgs"/> that contains the event
        /// data.</param>
        public void RaiseTouch(TouchEventArgs e)
        {
            TouchToMouseEvents(e);
            OnTouch(e);
            Touch?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="TitleChanged"/> event and calls
        /// <see cref="OnTitleChanged(EventArgs)"/>.
        /// </summary>
        public void RaiseTitleChanged()
        {
            OnTitleChanged(EventArgs.Empty);
            TitleChanged?.Invoke(this, EventArgs.Empty);
            Parent?.OnChildPropertyChanged(this, nameof(Title));
        }

        public void RaiseMouseEnterOnTarget()
        {
            var currentTarget = UI.Control.GetMouseTargetControl(this);
            currentTarget?.RaiseMouseEnter();
        }

        public void RaiseMouseLeaveOnTarget()
        {
            var currentTarget = UI.Control.GetMouseTargetControl(this);
            currentTarget?.RaiseMouseLeave();
        }
    }
}
