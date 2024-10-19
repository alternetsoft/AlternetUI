using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Raises the <see cref="FontChanged" /> event
        /// and <see cref="OnFontChanged"/> method.
        /// </summary>
        public void RaiseFontChanged()
        {
            PerformLayoutAndInvalidate(() =>
            {
                OnFontChanged(EventArgs.Empty);
                FontChanged?.Invoke(this, EventArgs.Empty);

                foreach (var child in Children)
                {
                    if (child.ParentFont)
                        child.Font = Font?.WithStyle(child.fontStyle);
                }
            });
        }

        /// <summary>
        /// Raises the <see cref="PreviewKeyDown" /> event
        /// and <see cref="OnPreviewKeyDown"/> method.
        /// </summary>
        public void RaisePreviewKeyDown(Key key, ModifierKeys modifiers, ref bool isInputKey)
        {
            if(PreviewKeyDown is not null)
            {
                PreviewKeyDownEventArgs e = new(this, key, modifiers);
                PreviewKeyDown(this, e);
                isInputKey = e.IsInputKey;
            }

            OnPreviewKeyDown(key, modifiers, ref isInputKey);

            var b = isInputKey;

            RaiseNotifications((n) =>
            {
                n.AfterPreviewKeyDown(this, key, modifiers, ref b);
            });

            isInputKey = b;
        }

        /// <summary>
        /// Raises the <see cref="CellChanged" /> event and <see cref="OnCellChanged"/> method.
        /// </summary>
        public void RaiseCellChanged()
        {
            CellChanged?.Invoke(this, EventArgs.Empty);
            OnCellChanged(EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterCellChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterCellChanged(this);
            }
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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterIdle(this);
            }

            foreach (var n in nn2)
            {
                n.AfterIdle(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseMove" /> event and <see cref="OnMouseMove"/> method.
        /// </summary>
        public void RaiseMouseMove(MouseEventArgs e)
        {
            HoveredControl = this;
            MouseMove?.Invoke(this, e);
            OnMouseMove(e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseMove(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseMove(this, e);
            }

            if (dragEventArgs is not null)
            {
                var mousePos = Mouse.GetPosition(this);
                var args = new DragStartEventArgs(lastMouseDownPos, mousePos, dragEventArgs, e);
                RaiseDragStart(args);
                if (args.DragStarted || args.Cancel)
                    dragEventArgs = null;
            }
        }

        /// <summary>
        /// Raises the <see cref="HelpRequested" /> event and <see cref="OnHelpRequested"/> method.
        /// </summary>
        public void RaiseHelpRequested(HelpEventArgs e)
        {
            OnHelpRequested(e);
            HelpRequested?.Invoke(this, e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterHelpRequested(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterHelpRequested(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/>event and <see cref="OnParentChanged"/> .
        /// </summary>
        public void RaiseParentChanged()
        {
            ResetScaleFactor();
            Designer?.RaiseParentChanged(this);
            ParentChanged?.Invoke(this, EventArgs.Empty);
            OnParentChanged(EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterParentChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterParentChanged(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseUp" /> event and <see cref="OnMouseUp"/> method.
        /// </summary>
        public void RaiseMouseUp(MouseEventArgs e)
        {
            HoveredControl = this;
            PlessMouse.CancelLongTapTimer();

            MouseUp?.Invoke(this, e);
            dragEventArgs = null;

            OnMouseUp(e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseUp(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseUp(this, e);
            }

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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseLeftButtonUp(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseLeftButtonUp(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseRightButtonUp" /> event
        /// and <see cref="OnMouseRightButtonUp"/> method.
        /// </summary>
        public void RaiseMouseRightButtonUp(MouseEventArgs e)
        {
            MouseRightButtonUp?.Invoke(this, e);
            OnMouseRightButtonUp(e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseRightButtonUp(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseRightButtonUp(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDown" /> event and <see cref="OnMouseDown"/> method.
        /// </summary>
        public void RaiseMouseDown(MouseEventArgs e)
        {
            HoveredControl = this;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                PlessMouse.StartLongTapTimer(this);

                dragEventArgs = e;
                lastMouseDownPos = Mouse.GetPosition(this);
            }

            MouseDown?.Invoke(this, e);

            OnMouseDown(e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseDown(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseDown(this, e);
            }

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
                CaretInfo.ContainerFocused = true;
                InvalidateCaret();
            }

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterGotFocus(this);
            }

            foreach (var n in nn2)
            {
                n.AfterGotFocus(this);
            }

            RunKnownAction(RunAfterGotFocus);
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
                CaretInfo.ContainerFocused = false;
                Invalidate();
            }

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterLostFocus(this);
            }

            foreach (var n in nn2)
            {
                n.AfterLostFocus(this);
            }

            RunKnownAction(RunAfterLostFocus);
        }

        /// <summary>
        /// Raises the <see cref="Activated" /> event and <see cref="OnActivated"/> method.
        /// </summary>
        public void RaiseActivated()
        {
            OnActivated(EventArgs.Empty);
            Activated?.Invoke(this, EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterActivated(this);
            }

            foreach (var n in nn2)
            {
                n.AfterActivated(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="OnHandlerLocationChanged" /> and <see cref="ReportBoundsChanged"/>
        /// methods.
        /// </summary>
        public void RaiseContainerLocationChanged()
        {
            OnHandlerLocationChanged(EventArgs.Empty);
            ReportBoundsChanged();

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterHandlerLocationChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterHandlerLocationChanged(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Deactivated" /> event and <see cref="OnDeactivated"/> method.
        /// </summary>
        public void RaiseDeactivated()
        {
            OnDeactivated(EventArgs.Empty);
            Deactivated?.Invoke(this, EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterDeactivated(this);
            }

            foreach (var n in nn2)
            {
                n.AfterDeactivated(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="SystemColorsChanged" /> event
        /// and <see cref="OnSystemColorsChanged"/> method.
        /// </summary>
        public void RaiseSystemColorsChanged()
        {
            SystemColorsChanged?.Invoke(this, EventArgs.Empty);
            OnSystemColorsChanged(EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterSystemColorsChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterSystemColorsChanged(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="QueryContinueDrag" /> event
        /// and <see cref="OnQueryContinueDrag"/> method.
        /// </summary>
        public void RaiseQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            QueryContinueDrag?.Invoke(this, e);
            OnQueryContinueDrag(e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterQueryContinueDrag(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterQueryContinueDrag(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="HandleCreated" /> event and <see cref="OnHandleCreated"/> method.
        /// </summary>
        public virtual void RaiseHandleCreated()
        {
            OnHandleCreated(EventArgs.Empty);
            HandleCreated?.Invoke(this, EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterHandleCreated(this);
            }

            foreach (var n in nn2)
            {
                n.AfterHandleCreated(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="HandleDestroyed" /> event and <see cref="OnHandleDestroyed"/> method.
        /// </summary>
        public void RaiseHandleDestroyed()
        {
            ResetScaleFactor();
            OnHandleDestroyed(EventArgs.Empty);
            HandleDestroyed?.Invoke(this, EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterHandleDestroyed(this);
            }

            foreach (var n in nn2)
            {
                n.AfterHandleDestroyed(this);
            }
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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseCaptureLost(this);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseCaptureLost(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="TextChanged" /> event.</summary>
        public void RaiseTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
            OnTextChanged(EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterTextChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterTextChanged(this);
            }
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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterSizeChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterSizeChanged(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseEnter" /> event and <see cref="OnMouseEnter"/> method.
        /// </summary>
        public void RaiseMouseEnter()
        {
            PlessMouse.CancelLongTapTimer();
            IsMouseOver = true;
            HoveredControl = this;
            RaiseIsMouseOverChanged();
            OnMouseEnter(EventArgs.Empty);
            MouseEnter?.Invoke(this, EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseEnter(this);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseEnter(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="VisualStateChanged" /> event
        /// and <see cref="OnVisualStateChanged"/> method.
        /// </summary>
        public void RaiseVisualStateChanged()
        {
            OnVisualStateChanged(EventArgs.Empty);
            VisualStateChanged?.Invoke(this, EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterVisualStateChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterVisualStateChanged(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IsMouseOverChanged" /> event
        /// and <see cref="OnIsMouseOverChanged"/> method.
        /// </summary>
        public void RaiseIsMouseOverChanged()
        {
            OnIsMouseOverChanged(EventArgs.Empty);
            IsMouseOverChanged?.Invoke(this, EventArgs.Empty);
            RaiseVisualStateChanged();

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterIsMouseOverChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterIsMouseOverChanged(this);
            }
        }

        /// <summary>
        /// Called after background color changed.
        /// </summary>
        public virtual void RaiseBackgroundColorChanged()
        {
            Refresh();

            foreach (var child in Children)
            {
                if (child.ParentBackColor)
                    child.BackgroundColor = BackgroundColor;
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseLeave" /> event
        /// and <see cref="OnMouseLeave"/> method.
        /// </summary>
        public void RaiseMouseLeave()
        {
            PlessMouse.CancelLongTapTimer();
            IsMouseOver = false;
            if (HoveredControl == this)
                HoveredControl = null;
            RaiseIsMouseOverChanged();
            IsMouseLeftButtonDown = false;
            OnMouseLeave(EventArgs.Empty);
            MouseLeave?.Invoke(this, EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseLeave(this);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseLeave(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ChildInserted" /> event
        /// and <see cref="OnChildInserted"/> method.
        /// </summary>
        public virtual void RaiseChildInserted(int index, AbstractControl childControl)
        {
            OnChildInserted(index, childControl);
            ChildInserted?.Invoke(this, new BaseEventArgs<AbstractControl>(childControl));

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterChildInserted(this, index, childControl);
            }

            foreach (var n in nn2)
            {
                n.AfterChildInserted(this, index, childControl);
            }
        }

        /// <summary>
        /// Raises the <see cref="ChildRemoved" /> event
        /// and <see cref="OnChildRemoved"/> method.
        /// </summary>
        public virtual void RaiseChildRemoved(AbstractControl childControl)
        {
            OnChildRemoved(childControl);
            ChildRemoved?.Invoke(this, new BaseEventArgs<AbstractControl>(childControl));

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterChildRemoved(this, childControl);
            }

            foreach (var n in nn2)
            {
                n.AfterChildRemoved(this, childControl);
            }
        }

        /// <summary>
        /// Raises the <see cref="Paint" /> event
        /// and <see cref="OnPaint"/> method.
        /// </summary>
        public void RaisePaint(PaintEventArgs e)
        {
            OnPaint(e);
            Paint?.Invoke(this, e);
            PaintCaret(e);
            PlessMouse.DrawTestMouseRect(this, e.Graphics);

            RaiseNotifications((n) => n.AfterPaint(this, e));
        }

        /// <summary>
        /// Raises the <see cref="LocationChanged"/> event.
        /// </summary>
        public void RaiseLocationChanged()
        {
            OnLocationChanged(EventArgs.Empty);
            LocationChanged?.Invoke(this, EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterLocationChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterLocationChanged(this);
            }
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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterDragStart(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterDragStart(this, e);
            }
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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterDragDrop(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterDragDrop(this, e);
            }
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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterDragOver(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterDragOver(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DpiChanged"/> event and <see cref="OnDpiChanged"/> method.
        /// </summary>
        /// <param name="e">The <see cref="DpiChangedEventArgs"/> that contains the
        /// event data.</param>
        public void RaiseDpiChanged(DpiChangedEventArgs e)
        {
            ResetScaleFactor();
            OnDpiChanged(e);
            DpiChanged?.Invoke(this, e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterDpiChanged(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterDpiChanged(this, e);
            }
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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterDragEnter(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterDragEnter(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DragLeave"/> event.
        /// </summary>
        public void RaiseDragLeave()
        {
            DragLeave?.Invoke(this, EventArgs.Empty);
            OnDragLeave(EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterDragLeave(this);
            }

            foreach (var n in nn2)
            {
                n.AfterDragLeave(this);
            }
        }

        /// <summary>
        /// Raises bounds changed events
        /// and <see cref="OnHandlerSizeChanged"/> method.
        /// </summary>
        public void RaiseHandlerSizeChanged()
        {
            OnHandlerSizeChanged(EventArgs.Empty);
            ReportBoundsChanged();

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterHandlerSizeChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterHandlerSizeChanged(this);
            }
        }

        /// <summary>
        /// Raises <see cref="VisibleChanged"/> event and <see cref="OnVisibleChanged"/>
        /// method.
        /// </summary>
        public virtual void RaiseVisibleChanged()
        {
            OnVisibleChanged(EventArgs.Empty);
            VisibleChanged?.Invoke(this, EventArgs.Empty);
            Parent?.ChildVisibleChanged?.Invoke(Parent, new BaseEventArgs<AbstractControl>(this));
            Parent?.PerformLayout();
            if (visible)
                AfterShow?.Invoke(this, EventArgs.Empty);
            else
                AfterHide?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="MouseLeftButtonDown" /> event
        /// and <see cref="OnMouseLeftButtonDown"/> method.
        /// </summary>
        public void RaiseMouseLeftButtonDown(MouseEventArgs e)
        {
            IsMouseLeftButtonDown = true;
            RaiseVisualStateChanged();
            Designer?.RaiseMouseLeftButtonDown(this, e);
            MouseLeftButtonDown?.Invoke(this, e);
            OnMouseLeftButtonDown(e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseLeftButtonDown(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseLeftButtonDown(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseRightButtonDown" /> event
        /// and <see cref="OnMouseRightButtonDown"/> method.
        /// </summary>
        public void RaiseMouseRightButtonDown(MouseEventArgs e)
        {
            MouseRightButtonDown?.Invoke(this, e);
            ShowPopupMenu(ContextMenuStrip);
            OnMouseRightButtonDown(e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseRightButtonDown(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseRightButtonDown(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyUp" /> event
        /// and <see cref="OnKeyUp"/> method.
        /// </summary>
        public void RaiseKeyUp(KeyEventArgs e)
        {
            PlessKeyboard.UpdateKeyStateInMemory(e, isDown: false);

            KeyUp?.Invoke(this, e);
            OnKeyUp(e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterKeyUp(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterKeyUp(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyPress" /> event
        /// and <see cref="OnKeyPress"/> method.
        /// </summary>
        public void RaiseKeyPress(KeyPressEventArgs e)
        {
            var isValidChar = IsValidInputChar(e.KeyChar);

            if (!isValidChar)
            {
                e.Handled = true;
                return;
            }

            KeyPress?.Invoke(this, e);
            OnKeyPress(e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterKeyPress(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterKeyPress(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseWheel" /> event
        /// and <see cref="OnMouseWheel"/> method.
        /// </summary>
        public void RaiseMouseWheel(MouseEventArgs e)
        {
            HoveredControl = this;
            OnMouseWheel(e);
            MouseWheel?.Invoke(this, e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseWheel(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseWheel(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDoubleClick" /> event
        /// and <see cref="OnMouseDoubleClick"/> method.
        /// </summary>
        public void RaiseMouseDoubleClick(MouseEventArgs e)
        {
            LastDoubleClickTimestamp = e.Timestamp;
            OnMouseDoubleClick(e);
            MouseDoubleClick?.Invoke(this, e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterMouseDoubleClick(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterMouseDoubleClick(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyDown" /> event
        /// and <see cref="OnKeyDown"/> method.
        /// </summary>
        public void RaiseKeyDown(KeyEventArgs e)
        {
            PlessKeyboard.UpdateKeyStateInMemory(e, isDown: true);

            bool isInputKey = false;
            RaisePreviewKeyDown(e.Key, e.ModifierKeys, ref isInputKey);

            KeyDown?.Invoke(this, e);
            OnKeyDown(e);
#if DEBUG
            if (!e.Handled)
                KeyInfo.Run(KnownShortcuts.ShowDeveloperTools, e, DialogFactory.ShowDeveloperTools);
#endif

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterKeyDown(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterKeyDown(this, e);
            }

            if (isInputKey)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="Click"/> event and calls
        /// <see cref="OnClick(EventArgs)"/>.
        /// See <see cref="Click"/> event description for more details.
        /// </summary>
        public void RaiseClick()
        {
            LastClickedTimestamp = DateTime.Now.Ticks;

            OnClick(EventArgs.Empty);
            Click?.Invoke(this, EventArgs.Empty);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterClick(this);
            }

            foreach (var n in nn2)
            {
                n.AfterClick(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Touch"/> event and calls
        /// <see cref="OnTouch"/> method.
        /// </summary>
        /// <param name="e">An <see cref="TouchEventArgs"/> that contains the event
        /// data.</param>
        public void RaiseTouch(TouchEventArgs e)
        {
            OnTouch(e);
            Touch?.Invoke(this, e);
            if(!e.Handled && TouchEventsAsMouse)
                TouchToMouseEvents(e);

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterTouch(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterTouch(this, e);
            }
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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                n.AfterTitleChanged(this);
            }

            foreach (var n in nn2)
            {
                n.AfterTitleChanged(this);
            }
        }

        /// <summary>
        /// Calls <see cref="RaiseMouseEnter"/> for the control under the mouse pointer.
        /// </summary>
        public void RaiseMouseEnterOnTarget()
        {
            var currentTarget = UI.AbstractControl.GetMouseTargetControl(this);
            currentTarget?.RaiseMouseEnter();
        }

        /// <summary>
        /// Calls <see cref="RaiseMouseLeave"/> for the control under the mouse pointer.
        /// </summary>
        public void RaiseMouseLeaveOnTarget()
        {
            var currentTarget = UI.AbstractControl.GetMouseTargetControl(this);
            currentTarget?.RaiseMouseLeave();
        }

        /// <summary>
        /// Raises the <see cref="LongTap"/> event and calls
        /// <see cref="OnLongTap"/> method.
        /// </summary>
        public void RaiseLongTap(LongTapEventArgs e)
        {
            OnLongTap(e);
            LongTap?.Invoke(this, e);
            RaiseNotifications((n) => n.AfterLongTap(this, e));
        }

        /// <summary>
        /// Calls the specified action for all the registered notifications.
        /// </summary>
        /// <param name="action">Action to call.</param>
        public virtual void RaiseNotifications(Action<IControlNotification> action)
        {
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            foreach (var n in nn)
            {
                action(n);
            }

            foreach (var n in nn2)
            {
                action(n);
            }
        }

        /// <summary>
        /// Raises the <see cref="EnabledChanged"/> event and calls
        /// <see cref="OnEnabledChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void RaiseEnabledChanged(EventArgs e)
        {
            RaiseVisualStateChanged();
            OnEnabledChanged(e);
            EnabledChanged?.Invoke(this, e);
            Parent?.OnChildPropertyChanged(this, nameof(Enabled));
        }
    }
}
