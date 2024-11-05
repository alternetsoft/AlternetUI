using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements subscriber to the control events. You can pass it to the
    /// <see cref="AbstractControl.AddNotification(IControlNotification?)"/>
    /// and <see cref="AbstractControl.AddGlobalNotification(IControlNotification)"/>
    /// methods.
    /// </summary>
    public class ControlSubscriber : DisposableObject, IControlNotification
    {
        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event PreviewKeyDownEventHandler? AfterControlPreviewKeyDown;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<(int Index, AbstractControl Child)>? AfterControlChildInserted;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<AbstractControl>? AfterControlChildRemoved;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<ScrollBarInfo>? AfterControlSetVerticalScrollBarInfo;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<ScrollBarInfo>? AfterControlSetHorizontalScrollBarInfo;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<DpiChangedEventArgs>? AfterControlDpiChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<DragEventArgs>? AfterControlDragDrop;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<DragEventArgs>? AfterControlDragEnter;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<DragEventArgs>? AfterControlDragOver;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<DragStartEventArgs>? AfterControlDragStart;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<HelpEventArgs>? AfterControlHelpRequested;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<KeyEventArgs>? AfterControlKeyDown;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<KeyPressEventArgs>? AfterControlKeyPress;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<KeyEventArgs>? AfterControlKeyUp;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<LongTapEventArgs>? AfterControlLongTap;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<MouseEventArgs>? AfterControlMouseDoubleClick;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<MouseEventArgs>? AfterControlMouseDown;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<MouseEventArgs>? AfterControlMouseLeftButtonDown;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<MouseEventArgs>? AfterControlMouseLeftButtonUp;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<MouseEventArgs>? AfterControlMouseMove;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<MouseEventArgs>? AfterControlMouseRightButtonDown;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<MouseEventArgs>? AfterControlMouseRightButtonUp;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<MouseEventArgs>? AfterControlMouseUp;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<MouseEventArgs>? AfterControlMouseWheel;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<PaintEventArgs>? AfterControlPaint;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<PaintEventArgs>? AfterControlPaintBackground;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<ThrowExceptionEventArgs>? AfterControlProcessException;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<QueryContinueDragEventArgs>? AfterControlQueryContinueDrag;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<ScrollEventArgs>? AfterControlScroll;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler<TouchEventArgs>? AfterControlTouch;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlResize;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlSizeChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlSystemColorsChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlTextChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlTitleChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlToolTipChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlVisibleChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlVisualStateChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlPaddingChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlParentChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlActivated;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlCellChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlClick;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlContainerLocationChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlCreate;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlDeactivated;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlDragLeave;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlEnabledChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlFontChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlGotFocus;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlHandleCreated;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlHandleDestroyed;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlHandlerAttached;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlHandlerDetaching;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlHandlerSizeChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlIdle;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlIsMouseOverChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlLocationChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlLostFocus;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlMarginChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlMouseCaptureLost;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlMouseEnter;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlMouseLeave;

        /// <inheritdoc/>
        public void AfterActivated(AbstractControl sender)
        {
            AfterControlActivated?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterCellChanged(AbstractControl sender)
        {
            AfterControlCellChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterClick(AbstractControl sender)
        {
            AfterControlClick?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterContainerLocationChanged(AbstractControl sender)
        {
            AfterControlContainerLocationChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterCreate(AbstractControl sender)
        {
            AfterControlCreate?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterDeactivated(AbstractControl sender)
        {
            AfterControlDeactivated?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterDpiChanged(AbstractControl sender, DpiChangedEventArgs e)
        {
            AfterControlDpiChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterDragDrop(AbstractControl sender, DragEventArgs e)
        {
            AfterControlDragDrop?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterDragEnter(AbstractControl sender, DragEventArgs e)
        {
            AfterControlDragEnter?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterDragLeave(AbstractControl sender)
        {
            AfterControlDragLeave?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterDragOver(AbstractControl sender, DragEventArgs e)
        {
            AfterControlDragOver?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterDragStart(AbstractControl sender, DragStartEventArgs e)
        {
            AfterControlDragStart?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterEnabledChanged(AbstractControl sender)
        {
            AfterControlEnabledChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterFontChanged(AbstractControl sender)
        {
            AfterControlFontChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterGotFocus(AbstractControl sender)
        {
            AfterControlGotFocus?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterHandleCreated(AbstractControl sender)
        {
            AfterControlHandleCreated?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterHandleDestroyed(AbstractControl sender)
        {
            AfterControlHandleDestroyed?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterHandlerAttached(AbstractControl sender)
        {
            AfterControlHandlerAttached?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterHandlerDetaching(AbstractControl sender)
        {
            AfterControlHandlerDetaching?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterHandlerSizeChanged(AbstractControl sender)
        {
            AfterControlHandlerSizeChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterHelpRequested(AbstractControl sender, HelpEventArgs e)
        {
            AfterControlHelpRequested?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterIdle(AbstractControl sender)
        {
            AfterControlIdle?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterIsMouseOverChanged(AbstractControl sender)
        {
            AfterControlIsMouseOverChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterKeyDown(AbstractControl sender, KeyEventArgs e)
        {
            AfterControlKeyDown?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterKeyPress(AbstractControl sender, KeyPressEventArgs e)
        {
            AfterControlKeyPress?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterKeyUp(AbstractControl sender, KeyEventArgs e)
        {
            AfterControlKeyUp?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterLocationChanged(AbstractControl sender)
        {
            AfterControlLocationChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterLongTap(AbstractControl sender, LongTapEventArgs e)
        {
            AfterControlLongTap?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterLostFocus(AbstractControl sender)
        {
            AfterControlLostFocus?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterMarginChanged(AbstractControl sender)
        {
            AfterControlMarginChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterMouseCaptureLost(AbstractControl sender)
        {
            AfterControlMouseCaptureLost?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterMouseDoubleClick(AbstractControl sender, MouseEventArgs e)
        {
            AfterControlMouseDoubleClick?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseDown(AbstractControl sender, MouseEventArgs e)
        {
            AfterControlMouseDown?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseEnter(AbstractControl sender)
        {
            AfterControlMouseEnter?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterMouseLeave(AbstractControl sender)
        {
            AfterControlMouseLeave?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterMouseLeftButtonDown(AbstractControl sender, MouseEventArgs e)
        {
            AfterControlMouseLeftButtonDown?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseLeftButtonUp(AbstractControl sender, MouseEventArgs e)
        {
            AfterControlMouseLeftButtonUp?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseMove(AbstractControl sender, MouseEventArgs e)
        {
            AfterControlMouseMove?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseRightButtonDown(AbstractControl sender, MouseEventArgs e)
        {
            AfterControlMouseRightButtonDown?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseRightButtonUp(AbstractControl sender, MouseEventArgs e)
        {
            AfterControlMouseRightButtonUp?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseUp(AbstractControl sender, MouseEventArgs e)
        {
            AfterControlMouseUp?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseWheel(AbstractControl sender, MouseEventArgs e)
        {
            AfterControlMouseWheel?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterPaddingChanged(AbstractControl sender)
        {
            AfterControlPaddingChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterPaint(AbstractControl sender, PaintEventArgs e)
        {
            AfterControlPaint?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterPaintBackground(AbstractControl sender, PaintEventArgs e)
        {
            AfterControlPaintBackground?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterParentChanged(AbstractControl sender)
        {
            AfterControlParentChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterProcessException(AbstractControl sender, ThrowExceptionEventArgs e)
        {
            AfterControlProcessException?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterQueryContinueDrag(AbstractControl sender, QueryContinueDragEventArgs e)
        {
            AfterControlQueryContinueDrag?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterResize(AbstractControl sender)
        {
            AfterControlResize?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterScroll(AbstractControl sender, ScrollEventArgs e)
        {
            AfterControlScroll?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterSizeChanged(AbstractControl sender)
        {
            AfterControlSizeChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterSystemColorsChanged(AbstractControl sender)
        {
            AfterControlSystemColorsChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterTextChanged(AbstractControl sender)
        {
            AfterControlTextChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterTitleChanged(AbstractControl sender)
        {
            AfterControlTitleChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterToolTipChanged(AbstractControl sender)
        {
            AfterControlToolTipChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterTouch(AbstractControl sender, TouchEventArgs e)
        {
            AfterControlTouch?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterVisibleChanged(AbstractControl sender)
        {
            AfterControlVisibleChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterVisualStateChanged(AbstractControl sender)
        {
            AfterControlVisualStateChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void AfterChildInserted(AbstractControl sender, int index, AbstractControl childControl)
        {
            AfterControlChildInserted?.Invoke(sender, (index, childControl));
        }

        /// <inheritdoc/>
        public void AfterChildRemoved(AbstractControl sender, AbstractControl childControl)
        {
            AfterControlChildRemoved?.Invoke(sender, childControl);
        }

        /// <inheritdoc/>
        public void AfterPreviewKeyDown(
            AbstractControl sender,
            Key key,
            ModifierKeys modifiers,
            ref bool isInputKey)
        {
            if (AfterControlPreviewKeyDown is null)
                return;
            PreviewKeyDownEventArgs e = new(sender, key, modifiers);
            AfterControlPreviewKeyDown.Invoke(sender, e);
            isInputKey = e.IsInputKey;
        }

        /// <inheritdoc/>
        public void AfterSetScrollBarInfo(AbstractControl sender, bool isVertical, ScrollBarInfo value)
        {
            if (isVertical)
            {
                AfterControlSetVerticalScrollBarInfo?.Invoke(sender, value);
            }
            else
            {
                AfterControlSetHorizontalScrollBarInfo?.Invoke(sender, value);
            }
        }
    }
}