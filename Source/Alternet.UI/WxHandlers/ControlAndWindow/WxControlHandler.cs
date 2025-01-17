using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing
    /// a specific <see cref="AbstractControl"/> behavior and appearance.
    /// </summary>
    internal class WxControlHandler : BaseControlHandler, IControlHandler
    {
        const int wxHORIZONTAL = 0x0004;
        const int wxVERTICAL = 0x0008;

        private Native.Control? nativeControl;
        private bool needDispose;
        public WxControlHandler()
        {
        }

        public RectI BoundsI
        {
            get => NativeControl.BoundsI;
            set => NativeControl.BoundsI = value;
        }

        public SizeI EventOldDpi
        {
            get
            {
                return NativeControl.EventOldDpi;
            }
        }

        public SizeI EventNewDpi
        {
            get
            {
                return NativeControl.EventNewDpi;
            }
        }

        public RectD EventBounds => NativeControl.EventBounds;

        /// <summary>
        /// Gets a value indicating whether the control has a native control associated with it.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if a native control has been assigned to the
        /// control; otherwise, <see langword="false" />.</returns>
        public bool IsNativeControlCreated
        {
            get => nativeControl is not null;
        }

        public LangDirection LangDirection
        {
            get => (LangDirection)NativeControl.LayoutDirection;
            set => NativeControl.LayoutDirection = (int)value;
        }

        public ControlBorderStyle BorderStyle
        {
            get => (ControlBorderStyle)NativeControl.BorderStyle;
            set => NativeControl.BorderStyle = (int)value;
        }

        public bool IsFocused => NativeControl.IsFocused;

        public bool IsScrollable
        {
            get => NativeControl.IsScrollable;
            set => NativeControl.IsScrollable = value;
        }

        public string Text
        {
            get => NativeControl.Text;
            set => NativeControl.Text = value;
        }

        public RectD Bounds
        {
            get => NativeControl.Bounds;
            set => NativeControl.Bounds = value;
        }

        public Thickness IntrinsicLayoutPadding
        {
            get => NativeControl.IntrinsicLayoutPadding;
        }

        public Thickness IntrinsicPreferredSizePadding
        {
            get => NativeControl.IntrinsicPreferredSizePadding;
        }

        public bool Visible
        {
            get => NativeControl.Visible;
            set => NativeControl.Visible = value;
        }

        public bool UserPaint
        {
            get => NativeControl.UserPaint;

            set
            {
                if (value && (!Control?.CanUserPaint ?? false))
                    return;

                NativeControl.UserPaint = value;
            }
        }

        public SizeD MinimumSize
        {
            get => NativeControl.MinimumSize;
            set => NativeControl.MinimumSize = value;
        }

        public SizeD MaximumSize
        {
            get => NativeControl.MaximumSize;
            set => NativeControl.MaximumSize = value;
        }

        public Color BackgroundColor
        {
            get => NativeControl.BackgroundColor;
            set => NativeControl.BackgroundColor = value;
        }

        public Color ForegroundColor
        {
            get => NativeControl.ForegroundColor;
            set => NativeControl.ForegroundColor = value;
        }

        public Font? Font
        {
            get => Font.FromInternal(NativeControl.Font);
            set => NativeControl.Font = (UI.Native.Font?)value?.Handler;
        }

        public bool IsBold
        {
            get => NativeControl.IsBold;
            set => NativeControl.IsBold = value;
        }

        public bool WantChars
        {
            get => NativeControl.WantChars;

            set
            {
                NativeControl.WantChars = value;
            }
        }

        public bool ShowHorzScrollBar
        {
            get => NativeControl.ShowHorzScrollBar;

            set
            {
                NativeControl.ShowHorzScrollBar = value;
            }
        }

        public bool ShowVertScrollBar
        {
            get => NativeControl.ShowVertScrollBar;

            set
            {
                NativeControl.ShowVertScrollBar = value;
            }
        }

        public bool ScrollBarAlwaysVisible
        {
            get => NativeControl.ScrollBarAlwaysVisible;

            set
            {
                NativeControl.ScrollBarAlwaysVisible = value;
            }
        }

        public bool TabStop
        {
            get => NativeControl.TabStop;
            set => NativeControl.TabStop = value;
        }

        public bool AllowDrop
        {
            get => NativeControl.AllowDrop;
            set => NativeControl.AllowDrop = value;
        }

        public bool CanSelect
        {
            get
            {
                return AcceptsFocus;
            }

            set
            {
                AcceptsFocus = value;
            }
        }

        public bool AcceptsFocus
        {
            get => NativeControl.AcceptsFocus;
            set => NativeControl.AcceptsFocus = value;
        }

        public ControlBackgroundStyle BackgroundStyle
        {
            get
            {
                var result = NativeControl.GetBackgroundStyle();
                return (ControlBackgroundStyle?)result ?? ControlBackgroundStyle.System;
            }

            set => NativeControl.SetBackgroundStyle((int)value);
        }

        public bool AcceptsFocusFromKeyboard
        {
            get => NativeControl.AcceptsFocusFromKeyboard;
            set => NativeControl.AcceptsFocusFromKeyboard = value;
        }

        public bool AcceptsFocusRecursively
        {
            get => NativeControl.AcceptsFocusRecursively;
            set => NativeControl.AcceptsFocusRecursively = value;
        }

        public bool AcceptsFocusAll
        {
            get => NativeControl.AcceptsFocusAll;
            set => NativeControl.AcceptsFocusAll = value;
        }

        public bool ProcessIdle
        {
            get => NativeControl.ProcessIdle;
            set => NativeControl.ProcessIdle = value;
        }

        public bool BindScrollEvents
        {
            get => NativeControl.BindScrollEvents;
            set => NativeControl.BindScrollEvents = value;
        }

        public SizeD ClientSize
        {
            get => NativeControl.ClientSize;
            set => NativeControl.ClientSize = value;
        }

        public bool CanAcceptFocus => NativeControl.CanAcceptFocus;

        public bool IsMouseOver => NativeControl.IsMouseOver;

        public bool ProcessUIUpdates
        {
            get => NativeControl.ProcessUIUpdates;
            set => NativeControl.ProcessUIUpdates = value;
        }

        public bool IsMouseCaptured => NativeControl.IsMouseCaptured;

        public bool IsHandleCreated => NativeControl.IsHandleCreated;

        public bool IsFocusable => NativeControl.IsFocusable;

        internal Native.Control? NativeControlOrNull
        {
            get
            {
                if (DisposingOrDisposed)
                    return null;
                return NativeControl;
            }
        }

        internal Native.Control NativeControl
        {
            get
            {
                if (nativeControl == null)
                {
                    nativeControl = CreateNativeControl();
                    nativeControl.handler = this;
                    OnNativeControlCreated();
                }

                return nativeControl;
            }
        }

        internal bool NativeControlCreated => nativeControl != null;

        public ScrollBarInfo VertScrollBarInfo
        {
            get
            {
                return GetScrollBarInfo(true);
            }

            set
            {
                SetScrollBarInfo(true, value);
            }
        }

        public ScrollBarInfo HorzScrollBarInfo
        {
            get
            {
                return GetScrollBarInfo(false);
            }

            set
            {
                SetScrollBarInfo(false, value);
            }
        }

        public virtual bool VisibleOnScreen
        {
            get
            {
                if (!Control?.Visible ?? true)
                    return false;
                var parent = Control?.Parent;
                if (parent is null)
                    return false;
                var result = parent.VisibleOnScreen;
                return result;
            }
        }

        public object GetNativeControl() => NativeControl;

        public override void OnLayoutChanged()
        {
            base.OnLayoutChanged();
        }

        public void Raise()
        {
            NativeControl.Raise();
        }

        public void SetCursor(Cursor? value)
        {
            NativeControl.SetCursor(WxCursorHandler.CursorToPtr(value));
        }

        public void SetToolTip(string? value)
        {
            NativeControl.ToolTip = value;
        }

        public void Lower()
        {
            NativeControl.Lower();
        }

        public void UnsetToolTip()
        {
            NativeControl.UnsetToolTip();
        }

        public void RefreshRect(RectD rect, bool eraseBackground = true)
        {
            NativeControl.RefreshRect(rect, eraseBackground);
        }

        public void HandleNeeded()
        {
            NativeControl.Required();
        }

        public void CaptureMouse()
        {
            NativeControl.SetMouseCapture(true);
        }

        public void ReleaseMouseCapture()
        {
            NativeControl.SetMouseCapture(false);
        }

        public void DisableRecreate()
        {
            NativeControl.DisableRecreate();
        }

        public void EnableRecreate()
        {
            NativeControl.EnableRecreate();
        }

        public Graphics CreateDrawingContext()
        {
            return new WxGraphics(NativeControl.OpenClientDrawingContext());
        }

        public PointD ScreenToClient(PointD point)
        {
            return NativeControl.ScreenToClient(point);
        }

        public PointD ClientToScreen(PointD point)
        {
            return NativeControl.ClientToScreen(point);
        }
        public void FocusNextControl(bool forward = true, bool nested = true)
        {
            NativeControl.FocusNextControl(forward, nested);
        }

        public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            allowedEffects &= ~DragDropEffects.Scroll;
            return (DragDropEffects)NativeControl.DoDragDrop(
                UnmanagedDataObjectService.GetUnmanagedDataObject(data),
                allowedEffects);
        }

        public void RecreateWindow()
        {
            NativeControl.RecreateWindow();
        }

        public virtual void BeginUpdate()
        {
            NativeControl.BeginUpdate();
        }

        public virtual void EndUpdate()
        {
            NativeControl.EndUpdate();
        }

        public void BeginInit()
        {
            NativeControl.BeginInit();
        }

        public void EndInit()
        {
            NativeControl.EndInit();
        }

        public bool SetFocus()
        {
            return NativeControl.SetFocus();
        }

        public void SaveScreenshot(string fileName)
        {
            NativeControl.SaveScreenshot(fileName);
        }

        public bool IsTransparentBackgroundSupported()
        {
            return NativeControl.IsTransparentBackgroundSupported();
        }

        public Coord? GetPixelScaleFactor()
        {
            if(App.IsWindowsOS && !DisposingOrDisposed)
                return Native.Control.DrawingDPIScaleFactor(NativeControl.WxWidget);
            return 1D;
        }

        public RectI GetUpdateClientRectI()
        {
            return NativeControl.GetUpdateClientRect();
        }

        public int PixelFromDip(Coord value)
        {
            return Native.Control.DrawingFromDip(value, NativeControl.WxWidget);
        }

        public void SetScrollBar(
            bool isVertical,
            bool visible,
            int value,
            int largeChange,
            int maximum)
        {
            ScrollBarOrientation orientation = isVertical
                ? ScrollBarOrientation.Vertical : ScrollBarOrientation.Horizontal;
            NativeControl.SetScrollBar(orientation, visible, value, largeChange, maximum);
        }

        public bool IsScrollBarVisible(bool isVertical)
        {
            ScrollBarOrientation orientation = isVertical
                ? ScrollBarOrientation.Vertical : ScrollBarOrientation.Horizontal;
            return NativeControl.IsScrollBarVisible(orientation);
        }

        public int GetScrollBarValue(bool isVertical)
        {
            ScrollBarOrientation orientation = isVertical
                ? ScrollBarOrientation.Vertical : ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarValue(orientation);
        }

        public int GetScrollBarLargeChange(bool isVertical)
        {
            ScrollBarOrientation orientation = isVertical
                ? ScrollBarOrientation.Vertical : ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarLargeChange(orientation);
        }

        public int GetScrollBarMaximum(bool isVertical)
        {
            ScrollBarOrientation orientation = isVertical
                ? ScrollBarOrientation.Vertical : ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarMaximum(orientation);
        }

        public void ResetBackgroundColor()
        {
            NativeControl.ResetBackgroundColor();
        }

        public void ResetForegroundColor()
        {
            NativeControl.ResetForegroundColor();
        }

        public void SetEnabled(bool value)
        {
            NativeControl.Enabled = value;
        }

        public Color GetDefaultAttributesBgColor()
        {
            return NativeControl.GetDefaultAttributesBgColor();
        }

        public Color GetDefaultAttributesFgColor()
        {
            return NativeControl.GetDefaultAttributesFgColor();
        }

        public Font? GetDefaultAttributesFont()
        {
            return Font.FromInternal(NativeControl.GetDefaultAttributesFont());
        }

        public void Update()
        {
            if (!IsAttached || Control is null)
                return;
            if (Control.HasParent || Control is Window)
            {
                NativeControl.Update();
            }
        }

        public void Invalidate()
        {
            if (!IsAttached || Control is null)
                return;
            if (Control.HasParent || Control is Window)
            {
                NativeControl.Invalidate();
            }
        }

        public nint GetHandle()
        {
            return NativeControl.Handle;
        }

        public SizeD GetPreferredSize(SizeD availableSize)
        {
            return NativeControl.GetPreferredSize(availableSize);
        }

        public Graphics OpenPaintDrawingContext()
        {
            return new WxGraphics(NativeControl.OpenPaintDrawingContext());
        }

        public void OnNativeControlDestroyed()
        {
            if (needDispose)
            {
                SafeDispose(ref nativeControl);
                needDispose = false;
            }
        }

        /// <summary>
        /// Detaches this handler from the <see cref="AbstractControl"/> it is attached to.
        /// </summary>
        public override void Detach()
        {
            base.Detach();

            if (nativeControl != null)
            {
                if (nativeControl.HasWindowCreated)
                {
                    needDispose = true;
                    nativeControl.Destroy();
                }
                else
                {
                    OnNativeControlDestroyed();
                }
            }
        }

        internal static WxControlHandler? NativeControlToHandler(
            Native.Control control)
        {
            return (WxControlHandler?)control.handler;
        }

        internal virtual Native.Control CreateNativeControl()
        {
            var result = new Native.Control.NonAbstractNativeControl();
            return result;
        }

        public void OnChildInserted(AbstractControl childControl)
        {
            if (childControl is GenericControl)
                return;
            var child = (UI.Control.RequireHandler(childControl) as WxControlHandler)?.NativeControl;
            if (child == null)
                return;
            if (child.ParentRefCounted != null)
                return;
            nativeControl?.AddChild(child);
        }

        public void OnChildRemoved(AbstractControl childControl)
        {
            if (childControl is GenericControl)
                return;
            var child = (UI.Control.RequireHandler(childControl) as WxControlHandler)?.nativeControl;
            if (child != null)
                nativeControl?.RemoveChild(child);
        }

        protected virtual void OnNativeControlCreated()
        {
            if (Control is null)
                return;

            var parent = Control.Parent;

            if (parent is not null)
            {
                (UI.Control.RequireHandler(parent) as WxControlHandler)?.OnChildInserted(Control);
                parent.PerformLayout();
            }
        }

        public void SetFocusFlags(bool canSelect, bool tabStop, bool acceptsFocusRecursively)
        {
            NativeControl.SetFocusFlags(canSelect, tabStop, acceptsFocusRecursively);
        }

        public bool CanScroll(bool isVertical)
        {
            return NativeControl.CanScroll(isVertical ? wxVERTICAL : wxHORIZONTAL);
        }

        public bool HasScrollbar(bool isVertical)
        {
            return NativeControl.HasScrollbar(isVertical ? wxVERTICAL : wxHORIZONTAL);
        }

        public ScrollBarInfo GetScrollBarInfo(bool isVertical)
        {
            ScrollBarInfo result = new();

            result.Range = GetScrollBarMaximum(isVertical);
            result.PageSize = GetScrollBarLargeChange(isVertical);
            result.Position = GetScrollBarValue(isVertical);

            if (ScrollBarAlwaysVisible)
                result.Visibility = HiddenOrVisible.Visible;
            else
            {
                result.Visibility = IsScrollBarVisible(isVertical)
                    ? HiddenOrVisible.Auto : HiddenOrVisible.Hidden;
            }

            return result;
        }

        public void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
            SetScrollBar(
                       isVertical,
                       value.IsVisible,
                       value.Position,
                       value.PageSize,
                       value.Range);
        }

        public bool EnableTouchEvents(TouchEventsMask flag)
        {
            return NativeControl.EnableTouchEvents((int)flag);
        }

        public void InvalidateBestSize()
        {
            NativeControl.InvalidateBestSize();
        }

        public void UpdateFocusFlags(bool canSelect, bool tabStop)
        {
            NativeControl.SetFocusFlags(canSelect, tabStop && canSelect, canSelect);
        }

        public virtual void OnHandleCreated()
        {
        }
    }
}