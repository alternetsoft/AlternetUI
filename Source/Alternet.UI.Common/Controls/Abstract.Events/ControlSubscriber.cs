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
        public event EventHandler<MouseEventArgs>? BeforeControlMouseDown;

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
        public event EventHandler<MouseEventArgs>? BeforeControlMouseMove;

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
        public event EventHandler<MouseEventArgs>? BeforeControlMouseUp;

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
        public event EventHandler? AfterControlIsMouseOverChanged;

        /// <summary>
        /// Occurs when the corresponding control's event is raised.
        /// </summary>
        public event EventHandler? AfterControlBoundsChanged;

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
        public void AfterActivated(AbstractControl sender, EventArgs e)
        {
            AfterControlActivated?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterCellChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlCellChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterClick(AbstractControl sender, EventArgs e)
        {
            AfterControlClick?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterContainerLocationChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlContainerLocationChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterCreate(AbstractControl sender, EventArgs e)
        {
            AfterControlCreate?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterDeactivated(AbstractControl sender, EventArgs e)
        {
            AfterControlDeactivated?.Invoke(sender, e);
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
        public void AfterDragLeave(AbstractControl sender, EventArgs e)
        {
            AfterControlDragLeave?.Invoke(sender, e);
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
        public void AfterEnabledChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlEnabledChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterFontChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlFontChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterGotFocus(AbstractControl sender, GotFocusEventArgs e)
        {
            AfterControlGotFocus?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterHandleCreated(AbstractControl sender, EventArgs e)
        {
            AfterControlHandleCreated?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterHandleDestroyed(AbstractControl sender, EventArgs e)
        {
            AfterControlHandleDestroyed?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterHandlerAttached(AbstractControl sender, EventArgs e)
        {
            AfterControlHandlerAttached?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterHandlerDetaching(AbstractControl sender, EventArgs e)
        {
            AfterControlHandlerDetaching?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterHandlerSizeChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlHandlerSizeChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterHelpRequested(AbstractControl sender, HelpEventArgs e)
        {
            AfterControlHelpRequested?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterIsMouseOverChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlIsMouseOverChanged?.Invoke(sender, e);
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
        public void AfterLocationChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlLocationChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterLongTap(AbstractControl sender, LongTapEventArgs e)
        {
            AfterControlLongTap?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterLostFocus(AbstractControl sender, LostFocusEventArgs e)
        {
            AfterControlLostFocus?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMarginChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlMarginChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseCaptureLost(AbstractControl sender, EventArgs e)
        {
            AfterControlMouseCaptureLost?.Invoke(sender, e);
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
        public void BeforeMouseDown(AbstractControl sender, MouseEventArgs e)
        {
            BeforeControlMouseDown?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseEnter(AbstractControl sender, EventArgs e)
        {
            AfterControlMouseEnter?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseLeave(AbstractControl sender, EventArgs e)
        {
            AfterControlMouseLeave?.Invoke(sender, e);
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
        public void BeforeMouseMove(AbstractControl sender, MouseEventArgs e)
        {
            BeforeControlMouseMove?.Invoke(sender, e);
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
        public void BeforeMouseUp(AbstractControl sender, MouseEventArgs e)
        {
            BeforeControlMouseUp?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterMouseWheel(AbstractControl sender, MouseEventArgs e)
        {
            AfterControlMouseWheel?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterPaddingChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlPaddingChanged?.Invoke(sender, e);
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
        public void AfterParentChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlParentChanged?.Invoke(sender, e);
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
        public void AfterResize(AbstractControl sender, EventArgs e)
        {
            AfterControlResize?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterScroll(AbstractControl sender, ScrollEventArgs e)
        {
            AfterControlScroll?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterSizeChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlSizeChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterSystemColorsChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlSystemColorsChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterTextChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlTextChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterTitleChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlTitleChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterToolTipChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlToolTipChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterTouch(AbstractControl sender, TouchEventArgs e)
        {
            AfterControlTouch?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterVisibleChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlVisibleChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public void AfterVisualStateChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlVisualStateChanged?.Invoke(sender, e);
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

        /// <inheritdoc/>
        public void AfterBoundsChanged(AbstractControl sender, EventArgs e)
        {
            AfterControlBoundsChanged?.Invoke(sender, e);
        }
    }
}