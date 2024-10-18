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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            CellChanged?.Invoke(this, EventArgs.Empty);
            OnCellChanged(EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnIdle(EventArgs.Empty);
            Idle?.Invoke(this, EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            HoveredControl = this;
            MouseMove?.Invoke(this, e);
            OnMouseMove(e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnHelpRequested(e);
            HelpRequested?.Invoke(this, e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            HoveredControl = this;
            PlessMouse.CancelLongTapTimer();

            MouseUp?.Invoke(this, e);
            dragEventArgs = null;

            OnMouseUp(e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            IsMouseLeftButtonDown = false;
            RaiseVisualStateChanged();
            MouseLeftButtonUp?.Invoke(this, e);
            OnMouseLeftButtonUp(e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            MouseRightButtonUp?.Invoke(this, e);
            OnMouseRightButtonUp(e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            HoveredControl = this;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                PlessMouse.StartLongTapTimer(this);

                dragEventArgs = e;
                lastMouseDownPos = Mouse.GetPosition(this);
            }

            MouseDown?.Invoke(this, e);

            OnMouseDown(e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnActivated(EventArgs.Empty);
            Activated?.Invoke(this, EventArgs.Empty);

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
        public void RaiseHandlerLocationChanged()
        {
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnHandlerLocationChanged(EventArgs.Empty);
            ReportBoundsChanged();

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnDeactivated(EventArgs.Empty);
            Deactivated?.Invoke(this, EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            SystemColorsChanged?.Invoke(this, EventArgs.Empty);
            OnSystemColorsChanged(EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            QueryContinueDrag?.Invoke(this, e);
            OnQueryContinueDrag(e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            ResetScaleFactor();
            OnHandleDestroyed(EventArgs.Empty);
            HandleDestroyed?.Invoke(this, EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            if (HoveredControl == this)
                HoveredControl = null;
            IsMouseLeftButtonDown = false;
            OnMouseCaptureLost(EventArgs.Empty);
            MouseCaptureLost?.Invoke(this, EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            TextChanged?.Invoke(this, EventArgs.Empty);
            OnTextChanged(EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnSizeChanged(EventArgs.Empty);
            SizeChanged?.Invoke(this, EventArgs.Empty);
            Resize?.Invoke(this, EventArgs.Empty);
            OnResize(EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            PlessMouse.CancelLongTapTimer();
            IsMouseOver = true;
            HoveredControl = this;
            RaiseIsMouseOverChanged();
            OnMouseEnter(EventArgs.Empty);
            MouseEnter?.Invoke(this, EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnVisualStateChanged(EventArgs.Empty);
            VisualStateChanged?.Invoke(this, EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnIsMouseOverChanged(EventArgs.Empty);
            IsMouseOverChanged?.Invoke(this, EventArgs.Empty);
            RaiseVisualStateChanged();

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            PlessMouse.CancelLongTapTimer();
            IsMouseOver = false;
            if (HoveredControl == this)
                HoveredControl = null;
            RaiseIsMouseOverChanged();
            IsMouseLeftButtonDown = false;
            OnMouseLeave(EventArgs.Empty);
            MouseLeave?.Invoke(this, EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnLocationChanged(EventArgs.Empty);
            LocationChanged?.Invoke(this, EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnDragStart(e);
            DragStart?.Invoke(this, e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnDragDrop(e);
            DragDrop?.Invoke(this, e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnDragOver(e);
            DragOver?.Invoke(this, e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            ResetScaleFactor();
            OnDpiChanged(e);
            DpiChanged?.Invoke(this, e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnDragEnter(e);
            DragEnter?.Invoke(this, e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            DragLeave?.Invoke(this, EventArgs.Empty);
            OnDragLeave(EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnHandlerSizeChanged(EventArgs.Empty);
            ReportBoundsChanged();

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            IsMouseLeftButtonDown = true;
            RaiseVisualStateChanged();
            Designer?.RaiseMouseLeftButtonDown(this, e);
            MouseLeftButtonDown?.Invoke(this, e);
            OnMouseLeftButtonDown(e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            MouseRightButtonDown?.Invoke(this, e);
            ShowPopupMenu(ContextMenuStrip);
            OnMouseRightButtonDown(e);

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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            KeyUp?.Invoke(this, e);
            OnKeyUp(e);

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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            KeyPress?.Invoke(this, e);
            OnKeyPress(e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            HoveredControl = this;
            OnMouseWheel(e);
            MouseWheel?.Invoke(this, e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            LastDoubleClickTimestamp = e.Timestamp;
            OnMouseDoubleClick(e);
            MouseDoubleClick?.Invoke(this, e);

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

            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnClick(EventArgs.Empty);
            Click?.Invoke(this, EventArgs.Empty);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnTouch(e);
            Touch?.Invoke(this, e);
            if(!e.Handled && TouchEventsAsMouse)
                TouchToMouseEvents(e);

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
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnTitleChanged(EventArgs.Empty);
            TitleChanged?.Invoke(this, EventArgs.Empty);
            Parent?.OnChildPropertyChanged(this, nameof(Title));

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
