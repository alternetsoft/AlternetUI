using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        private VisualControlStates? reportedVisualStates;

        /// <summary>
        /// Raises the <see cref="Invalidated" /> event
        /// and calls <see cref="OnInvalidated"/> method.
        /// </summary>
        public void RaiseInvalidated(InvalidateEventArgs e)
        {
            OnInvalidated(e);
            Invalidated?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="FontChanged" /> event
        /// and calls <see cref="OnFontChanged"/> method.
        /// </summary>
        [Browsable(false)]
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
        /// and calls <see cref="OnPreviewKeyDown"/> method.
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
        [Browsable(false)]
        public void RaiseCellChanged()
        {
            CellChanged?.Invoke(this, EventArgs.Empty);
            OnCellChanged(EventArgs.Empty);

            RaiseNotifications((n) => n.AfterCellChanged(this));
        }

        /// <summary>
        /// Raises the <see cref="Idle"/> event and calls
        /// <see cref="OnIdle(EventArgs)"/>.
        /// See <see cref="Idle"/> event description for more details.
        /// </summary>
        [Browsable(false)]
        public void RaiseIdle()
        {
            OnIdle(EventArgs.Empty);
            Idle?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterIdle(this));
        }

        /// <summary>
        /// Raises the <see cref="MouseMove" /> event and <see cref="OnMouseMove"/> method.
        /// </summary>
        public void RaiseMouseMove(MouseEventArgs e)
        {
            HoveredControl = this;
            MouseMove?.Invoke(this, e);
            OnMouseMove(e);

            Mouse.RaiseMoved(this, e);
            RaiseNotifications((n) => n.AfterMouseMove(this, e));

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

            RaiseNotifications((n) => n.AfterHelpRequested(this, e));
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/>event and <see cref="OnParentChanged"/> .
        /// </summary>
        [Browsable(false)]
        public void RaiseParentChanged()
        {
            ResetScaleFactor();
            Designer?.RaiseParentChanged(this);
            ParentChanged?.Invoke(this, EventArgs.Empty);
            OnParentChanged(EventArgs.Empty);

            RaiseNotifications((n) => n.AfterParentChanged(this));
        }

        /// <summary>
        /// Raises the <see cref="MouseUp" /> event and <see cref="OnMouseUp"/> method.
        /// </summary>
        public void RaiseMouseUp(MouseEventArgs e)
        {
            if (ForEachVisibleChild(e, (control, e) => control.OnBeforeParentMouseUp(this, e)))
                return;

            HoveredControl = this;
            PlessMouse.CancelLongTapTimer();

            MouseUp?.Invoke(this, e);
            dragEventArgs = null;

            OnMouseUp(e);

            RaiseNotifications((n) => n.AfterMouseUp(this, e));

            if (e.ChangedButton == MouseButton.Left)
            {
                RaiseMouseLeftButtonUp(e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                RaiseMouseRightButtonUp(e);
            }

            ForEachVisibleChild(e, (control, e) => control.OnBeforeParentMouseUp(this, e));
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

            RaiseNotifications((n) => n.AfterMouseLeftButtonUp(this, e));
        }

        /// <summary>
        /// Raises the <see cref="MouseRightButtonUp" /> event
        /// and <see cref="OnMouseRightButtonUp"/> method.
        /// </summary>
        public void RaiseMouseRightButtonUp(MouseEventArgs e)
        {
            MouseRightButtonUp?.Invoke(this, e);
            OnMouseRightButtonUp(e);

            RaiseNotifications((n) => n.AfterMouseRightButtonUp(this, e));
        }

        /// <summary>
        /// Raises the <see cref="MouseDown" /> event and <see cref="OnMouseDown"/> method.
        /// </summary>
        public void RaiseMouseDown(MouseEventArgs e)
        {
            if (ForEachVisibleChild(e, (control, e) => control.OnBeforeParentMouseDown(this, e)))
                return;

            HoveredControl = this;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                PlessMouse.StartLongTapTimer(this);

                dragEventArgs = e;
                lastMouseDownPos = Mouse.GetPosition(this);
            }

            MouseDown?.Invoke(this, e);

            OnMouseDown(e);

            RaiseNotifications((n) => n.AfterMouseDown(this, e));

            if (e.ChangedButton == MouseButton.Left)
            {
                RaiseMouseLeftButtonDown(e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                RaiseMouseRightButtonDown(e);
            }

            ForEachVisibleChild(e, (control, e) => control.OnAfterParentMouseDown(this, e));

            RaiseVisualStateChanged();
        }

        /// <summary>
        /// Raises the <see cref="GotFocus" /> event and <see cref="OnGotFocus"/> method.
        /// </summary>
        [Browsable(false)]
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

            RaiseNotifications((n) => n.AfterGotFocus(this));

            RunKnownAction(RunAfterGotFocus);
        }

        /// <summary>
        /// Calls <see cref="OnChildMouseLeave"/> method of the parent control.
        /// </summary>
        [Browsable(false)]
        public void RaiseChildMouseLeave(object? sender)
        {
            if (Parent is null)
                return;
            Parent.OnChildMouseLeave(sender, EventArgs.Empty);
            Parent.RaiseChildMouseLeave(sender);
        }

        /// <summary>
        /// Calls <see cref="OnChildLostFocus"/> method of the parent control.
        /// </summary>
        [Browsable(false)]
        public void RaiseChildLostFocus(object? sender)
        {
            if (Parent is null)
                return;
            Parent.OnChildLostFocus(sender, EventArgs.Empty);
            Parent.RaiseChildLostFocus(sender);
        }

        /// <summary>
        /// Raises the <see cref="LostFocus" /> event and <see cref="OnLostFocus"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseLostFocus()
        {
            if (FocusedControl == this)
                FocusedControl = null;
            OnLostFocus(EventArgs.Empty);
            RaiseChildLostFocus(this);
            LostFocus?.Invoke(this, EventArgs.Empty);
            RaiseVisualStateChanged();

            if (CaretInfo is not null)
            {
                CaretInfo.ContainerFocused = false;
                Invalidate();
            }

            RaiseNotifications((n) => n.AfterLostFocus(this));

            RunKnownAction(RunAfterLostFocus);
        }

        /// <summary>
        /// Raises the <see cref="Activated" /> event and <see cref="OnActivated"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseActivated()
        {
            OnActivated(EventArgs.Empty);
            Activated?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterActivated(this));
        }

        /// <summary>
        /// Raises the <see cref="OnHandlerLocationChanged" /> and <see cref="ReportBoundsChanged"/>
        /// methods.
        /// </summary>
        [Browsable(false)]
        public void RaiseContainerLocationChanged()
        {
            OnHandlerLocationChanged(EventArgs.Empty);
            ReportBoundsChanged();

            RaiseNotifications((n) => n.AfterContainerLocationChanged(this));
        }

        /// <summary>
        /// Raises the <see cref="Deactivated" /> event and <see cref="OnDeactivated"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseDeactivated()
        {
            OnDeactivated(EventArgs.Empty);
            Deactivated?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterDeactivated(this));
        }

        /// <summary>
        /// Raises the <see cref="SystemColorsChanged" /> event
        /// and <see cref="OnSystemColorsChanged"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseSystemColorsChanged()
        {
            SystemColorsChanged?.Invoke(this, EventArgs.Empty);
            OnSystemColorsChanged(EventArgs.Empty);

            RaiseNotifications((n) => n.AfterSystemColorsChanged(this));
        }

        /// <summary>
        /// Raises the <see cref="QueryContinueDrag" /> event
        /// and <see cref="OnQueryContinueDrag"/> method.
        /// </summary>
        public void RaiseQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            QueryContinueDrag?.Invoke(this, e);
            OnQueryContinueDrag(e);

            RaiseNotifications((n) => n.AfterQueryContinueDrag(this, e));
        }

        /// <summary>
        /// Raises the <see cref="HandleCreated" /> event and <see cref="OnHandleCreated"/> method.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseHandleCreated()
        {
            OnHandleCreated(EventArgs.Empty);
            HandleCreated?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterHandleCreated(this));
        }

        /// <summary>
        /// Raises the <see cref="HandleDestroyed" /> event and <see cref="OnHandleDestroyed"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseHandleDestroyed()
        {
            ResetScaleFactor();
            OnHandleDestroyed(EventArgs.Empty);
            HandleDestroyed?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterHandleDestroyed(this));
        }

        /// <summary>
        /// Raises the <see cref="MouseCaptureLost" /> event and <see cref="OnMouseCaptureLost"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseMouseCaptureLost()
        {
            if (HoveredControl == this)
                HoveredControl = null;
            IsMouseLeftButtonDown = false;
            OnMouseCaptureLost(EventArgs.Empty);
            OnMouseCaptureChanged(EventArgs.Empty);
            MouseCaptureLost?.Invoke(this, EventArgs.Empty);
            MouseCaptureChanged?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterMouseCaptureLost(this));
        }

        /// <summary>
        /// Raises the <see cref="TextChanged" /> event.</summary>
        [Browsable(false)]
        public virtual void RaiseTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
            OnTextChanged(EventArgs.Empty);

            RaiseNotifications((n) => n.AfterTextChanged(this));
        }

        /// <summary>
        /// Raises the <see cref="SizeChanged"/>, <see cref="Resize"/> events and
        /// <see cref="OnSizeChanged"/>, <see cref="OnResize"/> methods.
        /// </summary>
        [Browsable(false)]
        public void RaiseSizeChanged()
        {
            OnSizeChanged(EventArgs.Empty);
            SizeChanged?.Invoke(this, EventArgs.Empty);
            Resize?.Invoke(this, EventArgs.Empty);
            OnResize(EventArgs.Empty);

            RaiseNotifications((n) => n.AfterSizeChanged(this));
        }

        /// <summary>
        /// Raises the <see cref="MouseEnter" /> event and <see cref="OnMouseEnter"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseMouseEnter()
        {
            PlessMouse.CancelLongTapTimer();
            /*IsMouseOver = true;*/
            HoveredControl = this;
            RaiseIsMouseOverChanged();
            OnMouseEnter(EventArgs.Empty);
            MouseEnter?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterMouseEnter(this));
        }

        /// <summary>
        /// Raises the <see cref="VisualStateChanged" /> event
        /// and <see cref="OnVisualStateChanged"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseVisualStateChanged()
        {
            if (reportedVisualStates == VisualStates)
                return;
            reportedVisualStates = VisualStates;

            OnVisualStateChanged(EventArgs.Empty);
            VisualStateChanged?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterVisualStateChanged(this));
        }

        /// <summary>
        /// Raises the <see cref="IsMouseOverChanged" /> event
        /// and <see cref="OnIsMouseOverChanged"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseIsMouseOverChanged()
        {
            var reportedHovered = reportedVisualStates?.HasFlag(VisualControlStates.Hovered);
            if (reportedHovered == IsMouseOver)
                return;

            OnIsMouseOverChanged(EventArgs.Empty);
            IsMouseOverChanged?.Invoke(this, EventArgs.Empty);
            RaiseVisualStateChanged();

            RaiseNotifications((n) => n.AfterIsMouseOverChanged(this));
        }

        /// <summary>
        /// Called after background color changed.
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
        public void RaiseMouseLeave()
        {
            PlessMouse.CancelLongTapTimer();
            /*IsMouseOver = false;*/
            if (HoveredControl == this)
                HoveredControl = null;
            RaiseIsMouseOverChanged();
            IsMouseLeftButtonDown = false;
            OnMouseLeave(EventArgs.Empty);
            RaiseChildMouseLeave(this);
            MouseLeave?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterMouseLeave(this));
        }

        /// <summary>
        /// Raises the <see cref="ChildInserted" /> event
        /// and <see cref="OnChildInserted"/> method.
        /// </summary>
        public virtual void RaiseChildInserted(int index, AbstractControl childControl)
        {
            OnChildInserted(index, childControl);
            ChildInserted?.Invoke(this, new BaseEventArgs<AbstractControl>(childControl));

            RaiseNotifications((n) => n.AfterChildInserted(this, index, childControl));
        }

        /// <summary>
        /// Raises the <see cref="ChildRemoved" /> event
        /// and <see cref="OnChildRemoved"/> method.
        /// </summary>
        public virtual void RaiseChildRemoved(AbstractControl childControl)
        {
            OnChildRemoved(childControl);
            ChildRemoved?.Invoke(this, new BaseEventArgs<AbstractControl>(childControl));

            RaiseNotifications((n) => n.AfterChildRemoved(this, childControl));
        }

        /// <summary>
        /// Raises the <see cref="Paint" /> event
        /// and <see cref="OnPaint"/> method.
        /// </summary>
        public void RaisePaint(PaintEventArgs e)
        {
            if (IsPainting())
                return;

            BeginPaint();

            try
            {
                if (e.ClipRectangle.SizeIsEmpty)
                    return;
                OnPaint(e);
                Paint?.Invoke(this, e);
                PaintCaret(e);
                PlessMouse.DrawTestMouseRect(this, () => e.Graphics);

                RaiseNotifications((n) => n.AfterPaint(this, e));
            }
            finally
            {
                EndPaint();
            }
        }

        /// <summary>
        /// Raises the <see cref="LocationChanged"/> event.
        /// </summary>
        [Browsable(false)]
        public void RaiseLocationChanged()
        {
            OnLocationChanged(EventArgs.Empty);
            LocationChanged?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterLocationChanged(this));
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

            RaiseNotifications((n) => n.AfterDragStart(this, e));
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

            RaiseNotifications((n) => n.AfterDragDrop(this, e));
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

            RaiseNotifications((n) => n.AfterDragOver(this, e));
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

            RaiseNotifications((n) => n.AfterDpiChanged(this, e));
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

            RaiseNotifications((n) => n.AfterDragEnter(this, e));
        }

        /// <summary>
        /// Raises the <see cref="DragLeave"/> event.
        /// </summary>
        [Browsable(false)]
        public void RaiseDragLeave()
        {
            DragLeave?.Invoke(this, EventArgs.Empty);
            OnDragLeave(EventArgs.Empty);

            RaiseNotifications((n) => n.AfterDragLeave(this));
        }

        /// <summary>
        /// Raises bounds changed events
        /// and <see cref="OnHandlerSizeChanged"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseHandlerSizeChanged()
        {
            OnHandlerSizeChanged(EventArgs.Empty);
            ReportBoundsChanged();

            RaiseNotifications((n) => n.AfterHandlerSizeChanged(this));
        }

        /// <summary>
        /// Raises <see cref="VisibleChanged"/> event and <see cref="OnVisibleChanged"/>
        /// method.
        /// </summary>
        [Browsable(false)]
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

            RaiseNotifications((n) => n.AfterMouseLeftButtonDown(this, e));
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

            RaiseNotifications((n) => n.AfterMouseRightButtonDown(this, e));
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

            RaiseNotifications((n) => n.AfterKeyUp(this, e));
        }

        /// <summary>
        /// Raises the <see cref="KeyPress" /> event
        /// and <see cref="OnKeyPress"/> method.
        /// </summary>
        public void RaiseKeyPress(KeyPressEventArgs e)
        {
            if (ForEachVisibleChild(e, (control, e) => control.OnBeforeParentKeyPress(this, e)))
                return;

            var isValidChar = IsValidInputChar(e.KeyChar);

            if (!isValidChar)
            {
                e.Handled = true;
                return;
            }

            KeyPress?.Invoke(this, e);
            OnKeyPress(e);

            RaiseNotifications((n) => n.AfterKeyPress(this, e));

            ForEachVisibleChild(e, (control, e) => control.OnAfterParentKeyPress(this, e));
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

            RaiseNotifications((n) => n.AfterMouseWheel(this, e));
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
            DoubleClick?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterMouseDoubleClick(this, e));
        }

        /// <summary>
        /// Raises the <see cref="KeyDown" /> event
        /// and <see cref="OnKeyDown"/> method.
        /// </summary>
        public void RaiseKeyDown(KeyEventArgs e)
        {
            PlessKeyboard.UpdateKeyStateInMemory(e, isDown: true);

            if(ForEachParent(e, (control, e) => control.OnBeforeChildKeyDown(this, e)))
                return;
            if (ForEachVisibleChild(e, (control, e) => control.OnBeforeParentKeyDown(this, e)))
                return;

            bool isInputKey = false;
            RaisePreviewKeyDown(e.Key, e.ModifierKeys, ref isInputKey);

            KeyDown?.Invoke(this, e);
            OnKeyDown(e);

            RaiseNotifications((n) => n.AfterKeyDown(this, e));

            DebugUtils.DebugCall(() =>
            {
                if (!e.Handled)
                    KeyInfo.Run(KnownShortcuts.ShowDeveloperTools, e, DialogFactory.ShowDeveloperTools);
            });

            Parent?.OnAfterChildKeyDown(this, e);
            ForEachVisibleChild(e, (control, e) => control.OnAfterParentKeyDown(this, e));

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
        [Browsable(false)]
        public void RaiseClick()
        {
            LastClickedTimestamp = DateTime.Now.Ticks;

            OnClick(EventArgs.Empty);
            Click?.Invoke(this, EventArgs.Empty);

            RaiseNotifications((n) => n.AfterClick(this));
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

            RaiseNotifications((n) => n.AfterTouch(this, e));
        }

        /// <summary>
        /// Raises the <see cref="TitleChanged"/> event and calls
        /// <see cref="OnTitleChanged(EventArgs)"/>.
        /// </summary>
        [Browsable(false)]
        public void RaiseTitleChanged()
        {
            OnTitleChanged(EventArgs.Empty);
            TitleChanged?.Invoke(this, EventArgs.Empty);
            Parent?.OnChildPropertyChanged(this, nameof(Title));

            RaiseNotifications((n) => n.AfterTitleChanged(this));
        }

        /// <summary>
        /// Calls <see cref="RaiseMouseEnter"/> for the control under the mouse pointer.
        /// </summary>
        [Browsable(false)]
        public void RaiseMouseEnterOnTarget()
        {
            var currentTarget = UI.AbstractControl.GetMouseTargetControl(this);
            currentTarget?.RaiseMouseEnter();
        }

        /// <summary>
        /// Calls <see cref="RaiseMouseLeave"/> for the control under the mouse pointer.
        /// </summary>
        [Browsable(false)]
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
