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
        /// <summary>
        /// Gets or sets whether to call activated/deactivated events for all child
        /// controls recursively.
        /// Default is True. If this property is False, only top-most forms are notified.
        /// </summary>
        public static bool RaiseActivatedForChildren = true;

        private static
            (VisualControlStates ControlState, ObjectUniqueId ControlId)? reportedVisualStates;

        private DelayedEvent<EventArgs> delayedTextChanged = new();

        /// <summary>
        /// Occurs when the layout of the various visual elements changes.
        /// </summary>
        public event EventHandler? LayoutUpdated;

        /// <summary>
        /// Gets or sets the last reported visual states of the control.
        /// </summary>
        /// <remarks>
        /// This property returns the last reported visual states of the control,
        /// which may include states such as Hovered, Focused, or Disabled.
        /// </remarks>
        public static (VisualControlStates ControlState, ObjectUniqueId ControlId)? ReportedVisualStates
        {
            get
            {
                return reportedVisualStates;
            }

            set
            {
                reportedVisualStates = value;
            }
        }

        /// <summary>
        /// Raises <see cref="LayoutUpdated"/> event.
        /// </summary>
        [Browsable(false)]
        public void RaiseLayoutUpdated(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            LayoutUpdated?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Invalidated" /> event
        /// and calls <see cref="OnInvalidated"/> method.
        /// </summary>
        public void RaiseInvalidated(InvalidateEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnInvalidated(e);
            Invalidated?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="FontChanged" /> event
        /// and calls <see cref="OnFontChanged"/> method.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseFontChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            PerformLayoutAndInvalidate(() =>
            {
                OnFontChanged(e);
                FontChanged?.Invoke(this, e);

                if (HasChildren)
                {
                    foreach (var child in Children)
                    {
                        if (child.ParentFont)
                            child.Font = RealFont;
                    }
                }
            });
        }

        /// <summary>
        /// Raises the <see cref="PreviewKeyDown" /> event
        /// and calls <see cref="OnPreviewKeyDown"/> method.
        /// </summary>
        public void RaisePreviewKeyDown(Key key, ModifierKeys modifiers, ref bool isInputKey)
        {
            if (DisposingOrDisposed)
                return;
            if (PreviewKeyDown is not null)
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
        public void RaiseCellChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            CellChanged?.Invoke(this, e);
            OnCellChanged(e);

            RaiseNotifications((n) => n.AfterCellChanged(this, e));
        }

        /// <summary>
        /// Raises the <see cref="MouseMove" /> event and <see cref="OnMouseMove"/> method.
        /// </summary>
        public void RaiseMouseMove(MouseEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
            OnHelpRequested(e);
            HelpRequested?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterHelpRequested(this, e));
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/>event and <see cref="OnParentChanged"/> .
        /// </summary>
        [Browsable(false)]
        public void RaiseParentChanged(EventArgs e)
        {
            stateFlags |= ControlFlags.ParentAssigned;
            if (DisposingOrDisposed)
                return;
            ResetScaleFactor();
            Designer?.RaiseParentChanged(this, e);
            ParentChanged?.Invoke(this, e);
            OnParentChanged(e);

            RaiseNotifications((n) => n.AfterParentChanged(this, e));
        }

        /// <summary>
        /// Raises the <see cref="MouseUp" /> event and <see cref="OnMouseUp"/> method.
        /// </summary>
        public void RaiseMouseUp(MouseEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
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
        /// Raises the <see cref="MouseLeftButtonUp" /> event
        /// and <see cref="OnMouseLeftButtonUp"/> method.
        /// </summary>
        public void RaiseMouseLeftButtonUp(MouseEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            IsMouseLeftButtonDown = false;
            RaiseVisualStateChanged(e);
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
            if (DisposingOrDisposed)
                return;
            MouseRightButtonUp?.Invoke(this, e);

            if (HasContextMenu)
            {
                App.AddIdleTask(() =>
                {
                    if (DisposingOrDisposed)
                        return;

                    ShowPopupMenu(ContextMenuStrip);
                });
            }

            OnMouseRightButtonUp(e);

            RaiseNotifications((n) => n.AfterMouseRightButtonUp(this, e));
        }

        /// <summary>
        /// Raises the <see cref="MouseHover"/> event and calls <see cref="OnMouseHover"/> method.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /*
        The system starts tracking when the mouse enters a control.

        If the pointer remains stationary within a small rectangle (default: 4×4 pixels)
        for a set time (default: 400 ms), the MouseHover event fires.

        The event is raised only once per hover session. If the mouse leaves
        and re-enters the control, tracking restarts.        
        */
        public void RaiseMouseHover(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnMouseHover(e);
            MouseHover?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MouseDown" /> event and <see cref="OnMouseDown"/> method.
        /// </summary>
        public void RaiseMouseDown(MouseEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
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

            RaiseVisualStateChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="GotFocus" /> event and <see cref="OnGotFocus"/> method.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseGotFocus(GotFocusEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            FocusedControl = this;

            OnGotFocus(e);
            GotFocus?.Invoke(this, e);
            Designer?.RaiseGotFocus(this, e);
            RaiseVisualStateChanged(e);

            if (CaretInfo is not null)
            {
                CaretInfo.ContainerFocused = true;
                InvalidateCaret();
            }

            RaiseNotifications((n) => n.AfterGotFocus(this, e));

            RunKnownAction(RunAfterGotFocus);
        }

        /// <summary>
        /// Calls <see cref="OnChildLostFocus"/> method of the parent control.
        /// </summary>
        [Browsable(false)]
        public void RaiseChildLostFocus(object? sender, LostFocusEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (Parent is null)
                return;
            Parent.OnChildLostFocus(sender, e);
            Parent.RaiseChildLostFocus(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="LostFocus" /> event and <see cref="OnLostFocus"/> method.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseLostFocus(LostFocusEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (FocusedControl == this)
                FocusedControl = null;

            OnLostFocus(e);
            RaiseChildLostFocus(this, e);
            LostFocus?.Invoke(this, e);
            RaiseVisualStateChanged(e);

            if (CaretInfo is not null)
            {
                CaretInfo.ContainerFocused = false;
                Invalidate();
            }

            RaiseNotifications((n) => n.AfterLostFocus(this, e));

            RunKnownAction(RunAfterLostFocus);
        }

        /// <summary>
        /// Raises the <see cref="Deactivated" /> event and <see cref="OnDeactivated"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseDeactivated(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnDeactivated(e);
            Deactivated?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterDeactivated(this, e));

            if (RaiseActivatedForChildren)
            {
                ForEachChild((c) => c.RaiseDeactivated(e), false);
            }
        }

        /// <summary>
        /// Raises the <see cref="Activated" /> event and <see cref="OnActivated"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseActivated(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (this is Window window)
            {
                window.LastActivateTime = DateTime.Now;
            }

            OnActivated(e);
            Activated?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterActivated(this, e));

            if (RaiseActivatedForChildren)
            {
                ForEachChild((c) => c.RaiseActivated(e), false);
            }
        }

        /// <summary>
        /// Raises the <see cref="OnHandlerLocationChanged" /> and <see cref="ReportBoundsChanged"/>
        /// methods.
        /// </summary>
        [Browsable(false)]
        public void RaiseContainerLocationChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnHandlerLocationChanged(e);

            var layoutOnLocation = Parent is not null || this is not Window;

            ReportBoundsChanged(layoutOnLocation);

            RaiseNotifications((n) => n.AfterContainerLocationChanged(this, e));
        }

        /// <summary>
        /// Raises the <see cref="SystemColorsChanged" /> event
        /// and <see cref="OnSystemColorsChanged"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseSystemColorsChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            SystemColorsChanged?.Invoke(this, e);
            OnSystemColorsChanged(e);

            RaiseNotifications((n) => n.AfterSystemColorsChanged(this, e));
        }

        /// <summary>
        /// Raises the <see cref="QueryContinueDrag" /> event
        /// and <see cref="OnQueryContinueDrag"/> method.
        /// </summary>
        public void RaiseQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            QueryContinueDrag?.Invoke(this, e);
            OnQueryContinueDrag(e);

            RaiseNotifications((n) => n.AfterQueryContinueDrag(this, e));
        }

        /// <summary>
        /// Raises the <see cref="HandleCreated" /> event
        /// and <see cref="OnHandleCreated"/> method.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseHandleCreated(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnHandleCreated(e);
            HandleCreated?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterHandleCreated(this, e));
        }

        /// <summary>
        /// Raises the <see cref="HandleDestroyed" /> event
        /// and <see cref="OnHandleDestroyed"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseHandleDestroyed(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            ResetScaleFactor();
            OnHandleDestroyed(e);
            HandleDestroyed?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterHandleDestroyed(this, e));
        }

        /// <summary>
        /// Raises the <see cref="MouseCaptureLost" /> event and
        /// <see cref="OnMouseCaptureLost"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseMouseCaptureLost(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            IsMouseLeftButtonDown = false;
            OnMouseCaptureLost(e);
            OnMouseCaptureChanged(e);
            MouseCaptureLost?.Invoke(this, e);
            MouseCaptureChanged?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterMouseCaptureLost(this, e));
        }

        /// <summary>
        /// Raises the <see cref="TextChanged" /> event.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseTextChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            Invoke(Internal);

            void Internal()
            {
                TextChanged?.Invoke(this, e);
                OnTextChanged(e);

                RaiseNotifications((n) => n.AfterTextChanged(this, e));

                delayedTextChanged.Raise(this, e, () => DisposingOrDisposed);

                StateFlags &= ~ControlFlags.ForceTextChange;
            }
        }

        /// <summary>
        /// Raises the <see cref="SizeChanged"/>, <see cref="Resize"/> events and
        /// <see cref="OnSizeChanged"/>, <see cref="OnResize"/> methods.
        /// </summary>
        [Browsable(false)]
        public void RaiseSizeChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnSizeChanged(e);
            SizeChanged?.Invoke(this, e);
            Resize?.Invoke(this, e);
            OnResize(e);

            RaiseNotifications((n) => n.AfterSizeChanged(this, e));

            ForEachVisibleChild(
                HandledEventArgs.NotHandled,
                (control, e) => control.OnAfterParentSizeChanged(this, e));
        }

        /// <summary>
        /// Calls <see cref="OnChildMouseLeave"/> method of the parent control.
        /// </summary>
        [Browsable(false)]
        public void RaiseChildMouseLeave(object? sender, EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (Parent is null)
                return;
            PlessMouse.LastMousePosition = (null, null);
            Parent.OnChildMouseLeave(sender, e);
            Parent.RaiseChildMouseLeave(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="MouseLeave" /> event
        /// and <see cref="OnMouseLeave"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseMouseLeave(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            PlessMouse.CancelLongTapTimer();
            if (HoveredControl == this)
                HoveredControl = null;
            RaiseIsMouseOverChanged(e);
            IsMouseLeftButtonDown = false;
            OnMouseLeave(e);
            RaiseChildMouseLeave(this, e);
            MouseLeave?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterMouseLeave(this, e));

            if (PlessMouse.ShowTestMouseInControl)
            {
                Refresh();
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseEnter" /> event and <see cref="OnMouseEnter"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseMouseEnter(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            PlessMouse.CancelLongTapTimer();
            HoveredControl = this;
            PlessMouse.LastMousePosition = (null, this);
            RaiseIsMouseOverChanged(e);
            OnMouseEnter(e);
            MouseEnter?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterMouseEnter(this, e));
        }

        /// <summary>
        /// Raises the <see cref="VisualStateChanged" /> event
        /// and <see cref="OnVisualStateChanged"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseVisualStateChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            var newVisualStates = (VisualStates, this.UniqueId);

            if (reportedVisualStates == newVisualStates)
                return;
            reportedVisualStates = newVisualStates;

            OnVisualStateChanged(e);
            VisualStateChanged?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterVisualStateChanged(this, e));
        }

        /// <summary>
        /// Raises the <see cref="IsMouseOverChanged" /> event
        /// and <see cref="OnIsMouseOverChanged"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseIsMouseOverChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnIsMouseOverChanged(e);
            IsMouseOverChanged?.Invoke(this, e);
            RaiseVisualStateChanged(e);

            RaiseNotifications((n) => n.AfterIsMouseOverChanged(this, e));
        }

        /// <summary>
        /// Called after background color changed.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseBackgroundColorChanged()
        {
            if (DisposingOrDisposed)
                return;
            Refresh();

            if (HasChildren)
            {
                foreach (var child in Children)
                {
                    if (child.ParentBackColor)
                        child.BackgroundColor = BackColor;
                }
            }
        }

        /// <summary>
        /// Called after foreground color changed.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseForegroundColorChanged()
        {
            if (DisposingOrDisposed)
                return;
            Refresh();

            if (HasChildren)
            {
                foreach (var child in Children)
                {
                    if (child.ParentForeColor)
                        child.ForegroundColor = ForeColor;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="ChildInserted" /> event
        /// and <see cref="OnChildInserted"/> method.
        /// </summary>
        public virtual void RaiseChildInserted(int index, AbstractControl childControl)
        {
            childControl.SetParentInternal(this);
            childControl.RaiseParentChanged(EventArgs.Empty);

            if (DisposingOrDisposed)
                return;

            childControl.RaiseHandleCreated(EventArgs.Empty);

            if (childControl.Visible && !childControl.IgnoreLayout)
            {
                DoInsideLayout(UpdateFontAndColor);
            }
            else
            {
                UpdateFontAndColor();
            }

            void UpdateFontAndColor()
            {
                if (childControl.ParentFont)
                {
                    childControl.Font = RealFont;
                }

                if (childControl.ParentBackColor)
                {
                    childControl.BackColor = RealBackgroundColor;
                }

                if (childControl.ParentForeColor)
                {
                    childControl.ForeColor = RealForegroundColor;
                }
            }

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
            childControl.SetParentInternal(null);
            childControl.RaiseParentChanged(EventArgs.Empty);

            if (DisposingOrDisposed)
                return;
            OnChildRemoved(childControl);
            ChildRemoved?.Invoke(this, new BaseEventArgs<AbstractControl>(childControl));

            RaiseNotifications((n) => n.AfterChildRemoved(this, childControl));
            if (childControl.Visible && !childControl.IgnoreLayout)
                PerformLayout();
        }

        /// <summary>
        /// Raises the <see cref="Paint" /> event
        /// and <see cref="OnPaint"/> method.
        /// </summary>
        public void RaisePaint(PaintEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (IsPainting())
                return;

            ResetScaleFactor();

            BeginPaint();

            try
            {
                if (e.ClipRectangle.SizeIsEmpty)
                    return;
                OnPaint(e);
                Paint?.Invoke(this, e);

                if(!App.IsMaui && HasVisibleChildren && this is Control)
                {
                    TemplateUtils.RaisePaintForGenericChildren(this, () => e.Graphics);
                }

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
        public void RaiseLocationChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnLocationChanged(e);
            LocationChanged?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterLocationChanged(this, e));
        }

        /// <summary>
        /// Raises the <see cref="DragStart"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragStartEventArgs"/> that contains the
        /// event data.</param>
        public void RaiseDragStart(DragStartEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
            OnDragOver(e);
            DragOver?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterDragOver(this, e));
        }

        /// <summary>
        /// Notifies control and optionally it's child controls about dpi changes.
        /// </summary>
        /// <param name="recursive">Whether to notify child controls.</param>
        public void RaiseDpiChanged(bool recursive)
        {
            if (DisposingOrDisposed)
                return;
            RaiseDpiChanged();
            if (recursive && HasChildren)
            {
                foreach(var child in Children)
                {
                    child.RaiseDpiChanged(true);
                }
            }
        }

        /// <summary>
        /// Calls <see cref="RaiseDpiChanged(DpiChangedEventArgs)"/>.
        /// </summary>
        /// <param name="dpiNew">New dpi value</param>
        /// <param name="dpiOld">Old dpi value</param>
        public void RaiseDpiChanged(SizeI? dpiOld = null, SizeI? dpiNew = null)
        {
            if (DisposingOrDisposed)
                return;
            var oldValue = dpiOld ?? GetDPI().ToSize();

            if(dpiNew is null)
            {
                ResetScaleFactor();
                dpiNew = GetDPI().ToSize();
            }

            DpiChangedEventArgs e = new(oldValue, dpiNew.Value);
            RaiseDpiChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="DpiChanged"/> event and <see cref="OnDpiChanged"/> method.
        /// </summary>
        /// <param name="e">The <see cref="DpiChangedEventArgs"/> that contains the
        /// event data.</param>
        public void RaiseDpiChanged(DpiChangedEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            Display.Reset();
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
            if (DisposingOrDisposed)
                return;
            OnDragEnter(e);
            DragEnter?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterDragEnter(this, e));
        }

        /// <summary>
        /// Raises the <see cref="DragLeave"/> event.
        /// </summary>
        [Browsable(false)]
        public void RaiseDragLeave(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            DragLeave?.Invoke(this, e);
            OnDragLeave(e);

            RaiseNotifications((n) => n.AfterDragLeave(this, e));
        }

        /// <summary>
        /// Raises bounds changed events
        /// and <see cref="OnHandlerSizeChanged"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseHandlerSizeChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnHandlerSizeChanged(e);
            ReportBoundsChanged();

            RaiseNotifications((n) => n.AfterHandlerSizeChanged(this, e));
        }

        /// <summary>
        /// Raises <see cref="VisibleChanged"/> event and <see cref="OnVisibleChanged"/>
        /// method.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseVisibleChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (Visible)
            {
                ResetScaleFactor();
            }

            OnVisibleChanged(e);
            VisibleChanged?.Invoke(this, e);
            Parent?.ChildVisibleChanged?.Invoke(Parent, new BaseEventArgs<AbstractControl>(this));
            Parent?.PerformLayout();
            if (visible)
                AfterShow?.Invoke(this, e);
            else
                AfterHide?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MouseLeftButtonDown" /> event
        /// and <see cref="OnMouseLeftButtonDown"/> method.
        /// </summary>
        public void RaiseMouseLeftButtonDown(MouseEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            IsMouseLeftButtonDown = true;
            RaiseVisualStateChanged(e);
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
            if (DisposingOrDisposed)
                return;
            MouseRightButtonDown?.Invoke(this, e);
            OnMouseRightButtonDown(e);

            RaiseNotifications((n) => n.AfterMouseRightButtonDown(this, e));
        }

        /// <summary>
        /// Raises the <see cref="KeyDown" /> event
        /// and <see cref="OnKeyDown"/> method.
        /// </summary>
        public void RaiseKeyDown(KeyEventArgs e, Action<KeyEventArgs>? after = null)
        {
            if (DisposingOrDisposed)
                return;
            PlessKeyboard.UpdateKeyStateInMemory(e, isDown: true);

            if (ForEachParent(e, (control, e) => control.OnBeforeChildKeyDown(this, e)))
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
                {
                    KeyInfo.Run(
                        KnownShortcuts.ShowDeveloperTools,
                        e,
                        DialogFactory.ShowDeveloperTools);
                }
            });

            if (ForEachVisibleChild(e, (control, e) => control.OnAfterParentKeyDown(this, e)))
                return;
            if (ForEachParent(e, (control, e) => control.OnAfterChildKeyDown(this, e)))
                return;

            after?.Invoke(e);

            if (isInputKey)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyUp" /> event
        /// and <see cref="OnKeyUp"/> method.
        /// </summary>
        public void RaiseKeyUp(KeyEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
            HoveredControl = this;
            OnMouseWheel(e);

            if (ForEachParent(e, (control, e) => control.OnBeforeChildMouseWheel(this, e)))
                return;

            MouseWheel?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterMouseWheel(this, e));
            ForEachVisibleChild(e, (control, e) => control.OnAfterParentMouseWheel(this, e));

            if (ForEachParent(e, (control, e) => control.OnAfterChildMouseWheel(this, e)))
                return;
        }

        /// <summary>
        /// Raises the <see cref="MouseDoubleClick" /> event
        /// and <see cref="OnMouseDoubleClick"/> method.
        /// </summary>
        public void RaiseMouseDoubleClick(MouseEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            LastDoubleClickTimestamp = e.Timestamp;
            OnMouseDoubleClick(e);
            MouseDoubleClick?.Invoke(this, e);
            DoubleClick?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterMouseDoubleClick(this, e));
        }

        /// <summary>
        /// Calls <see cref="RaiseClick(EventArgs)"/> with an empty arguments.
        /// </summary>
        public void RaiseClick() => RaiseClick(EventArgs.Empty);

        /// <summary>
        /// Raises the <see cref="Click"/> event and calls
        /// <see cref="OnClick(EventArgs)"/>.
        /// See <see cref="Click"/> event description for more details.
        /// </summary>
        [Browsable(false)]
        public virtual void RaiseClick(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            LastClickedTimestamp = DateTime.Now.Ticks;

            OnClick(e);
            Click?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterClick(this, e));
        }

        /// <summary>
        /// Raises the <see cref="Touch"/> event and calls
        /// <see cref="OnTouch"/> method.
        /// </summary>
        /// <param name="e">An <see cref="TouchEventArgs"/> that contains the event
        /// data.</param>
        public void RaiseTouch(TouchEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnTouch(e);
            Touch?.Invoke(this, e);
            if (!e.Handled && TouchEventsAsMouse)
                TouchToMouseEvents(e);

            RaiseNotifications((n) => n.AfterTouch(this, e));
        }

        /// <summary>
        /// Raises the <see cref="TitleChanged"/> event and calls
        /// <see cref="OnTitleChanged(EventArgs)"/>.
        /// </summary>
        [Browsable(false)]
        public void RaiseTitleChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnTitleChanged(e);
            TitleChanged?.Invoke(this, e);
            Parent?.OnChildPropertyChanged(this, nameof(Title));

            RaiseNotifications((n) => n.AfterTitleChanged(this, e));
        }

        /// <summary>
        /// Calls <see cref="RaiseMouseEnter"/> for the control under the mouse pointer.
        /// </summary>
        [Browsable(false)]
        public void RaiseMouseEnterOnTarget(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            PointD? position = null;
            var originalTarget = this;

            var currentTarget = UI.AbstractControl.GetMouseTargetControl(
                ref originalTarget,
                ref position);

            currentTarget?.RaiseMouseEnter(e);
        }

        /// <summary>
        /// Calls <see cref="RaiseMouseLeave"/> for the control under the mouse pointer.
        /// </summary>
        [Browsable(false)]
        public void RaiseMouseLeaveOnTarget(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            PointD? position = null;
            var originalTarget = this;

            var currentTarget = UI.AbstractControl.GetMouseTargetControl(
                ref originalTarget,
                ref position);

            currentTarget?.RaiseMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="LongTap"/> event and calls
        /// <see cref="OnLongTap"/> method.
        /// </summary>
        public void RaiseLongTap(LongTapEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
            var nn = Notifications.ToArray();
            var nn2 = GlobalNotifications.ToArray();

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
            if (DisposingOrDisposed)
                return;
            RaiseVisualStateChanged(e);
            OnEnabledChanged(e);
            EnabledChanged?.Invoke(this, e);
            Parent?.OnChildPropertyChanged(this, nameof(Enabled));
        }
    }
}
