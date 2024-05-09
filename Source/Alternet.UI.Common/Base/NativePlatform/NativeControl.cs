using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI;

public abstract class NativeControl
{
    public static NativeControl Default = new NotImplementedControl();

    public abstract void ShowContextMenu(ContextMenu menu, IControl control, PointD? position = null);

    public abstract CustomControlPainter GetPainter();

    public abstract bool GetWantChars(IControl control);

    public abstract void SetWantChars(IControl control, bool value);

    public abstract BaseControlHandler CreateMenuItemHandler(IControl control);

    public abstract BaseControlHandler CreateContextMenuHandler(IControl control);

    public abstract BaseControlHandler CreateMainMenuHandler(IControl control);

    public abstract BaseControlHandler CreateControlHandler(IControl control);

    public abstract void SetShowHorzScrollBar(IControl control, bool value);

    public abstract void SetShowVertScrollBar(IControl control, bool value);

    public abstract void SetScrollBarAlwaysVisible(IControl control, bool value);

    public abstract void NotifyCaptureLost();

    public abstract bool CanAcceptFocus(IControl control);

    public abstract SizeD GetClientSize(IControl control);

    public abstract void SetClientSize(IControl control, SizeD value);

    public abstract void SetCursor(IControl control, Cursor? value);

    public abstract bool GetIsScrollable(IControl control);

    public abstract void SetIsScrollable(IControl control, bool value);

    public abstract bool IsMouseOver(IControl control);

    public abstract bool IsMouseCaptured(IControl control);

    public abstract bool IsFocusable(IControl control);

    public abstract void SetToolTip(IControl control, string? value);

    public abstract RectD GetBounds(IControl control);

    public abstract void SetBounds(IControl control, RectD value);

    public abstract bool GetVisible(IControl control);

    public abstract void SetVisible(IControl control, bool value);

    public abstract bool IsHandleCreated(IControl control);

    public abstract bool GetUserPaint(IControl control);

    public abstract void SetUserPaint(IControl control, bool value);

    public abstract SizeD GetMinimumSize(IControl control);

    public abstract void SetMinimumSize(IControl control, SizeD value);

    public abstract SizeD GetMaximumSize(IControl control);

    public abstract void SetMaximumSize(IControl control, SizeD value);

    public abstract Color GetBackgroundColor(IControl control);

    public abstract void SetBackgroundColor(IControl control, Color value);

    public abstract Color GetForegroundColor(IControl control);

    public abstract void SetForegroundColor(IControl control, Color value);

    public abstract Thickness GetIntrinsicLayoutPadding(IControl control);

    public abstract Thickness GetIntrinsicPreferredSizePadding(IControl control);

    public abstract Font? GetFont(IControl control);

    public abstract void SetFont(IControl control, Font? value);

    public abstract bool GetIsBold(IControl control);

    public abstract void SetIsBold(IControl control, bool value);

    public abstract bool GetTabStop(IControl control);

    public abstract void SetTabStop(IControl control, bool value);

    public abstract bool GetAllowDrop(IControl control);

    public abstract void SetAllowDrop(IControl control, bool value);

    public abstract ControlBackgroundStyle GetBackgroundStyle(IControl control);

    public abstract void SetBackgroundStyle(IControl control, ControlBackgroundStyle value);

    public abstract bool IsFocused(IControl control);

    public abstract bool GetAcceptsFocus(IControl control);

    public abstract void SetAcceptsFocus(IControl control, bool value);

    public abstract bool GetAcceptsFocusFromKeyboard(IControl control);

    public abstract void SetAcceptsFocusFromKeyboard(IControl control, bool value);

    public abstract bool GetAcceptsFocusRecursively(IControl control);

    public abstract void SetAcceptsFocusRecursively(IControl control, bool value);

    public abstract bool GetAcceptsFocusAll(IControl control);

    public abstract void SetAcceptsFocusAll(IControl control, bool value);

    public abstract bool GetProcessIdle(IControl control);

    public abstract void SetProcessIdle(IControl control, bool value);

    public abstract bool GetBindScrollEvents(IControl control);

    public abstract void SetBindScrollEvents(IControl control, bool value);

    public abstract void Raise(IControl control);

    public abstract void CenterOnParent(IControl control, GenericOrientation direction);

    public abstract void Lower(IControl control);

    public abstract void SendSizeEvent(IControl control);

    public abstract void UnsetToolTip(IControl control);

    public abstract void RefreshRect(IControl control, RectD rect, bool eraseBackground = true);

    public abstract void HandleNeeded(IControl control);

    public abstract void CaptureMouse(IControl control);

    public abstract void ReleaseMouseCapture(IControl control);

    public abstract void DisableRecreate(IControl control);

    public abstract void EnableRecreate(IControl control);

    public abstract Graphics CreateDrawingContext(IControl control);

    public abstract PointD ScreenToClient(IControl control, PointD point);

    public abstract PointD ClientToScreen(IControl control, PointD point);

    public abstract PointI ScreenToDevice(IControl control, PointD point);

    public abstract PointD DeviceToScreen(IControl control, PointI point);

    public abstract void FocusNextControl(IControl control, bool forward = true, bool nested = true);

    public abstract DragDropEffects DoDragDrop(
        IControl control,
        object data,
        DragDropEffects allowedEffects);

    public abstract void RecreateWindow(IControl control);

    public abstract void BeginUpdate(IControl control);

    public abstract void EndUpdate(IControl control);

    public abstract void SetBounds(IControl control, RectD rect, SetBoundsFlags flags);

    public abstract void BeginInit(IControl control);

    public abstract void EndInit(IControl control);

    public abstract bool SetFocus(IControl control);

    public abstract void SaveScreenshot(IControl control, string fileName);

    public abstract SizeD GetDPI(IControl control);

    public abstract bool IsTransparentBackgroundSupported(IControl control);

    public abstract void EndIgnoreRecreate(IControl control);

    public abstract void BeginIgnoreRecreate(IControl control);

    public abstract double GetPixelScaleFactor(IControl control);

    public abstract RectI GetUpdateClientRectI(IControl control);

    public abstract double PixelToDip(IControl control, int value);

    public abstract int PixelFromDip(IControl control, double value);

    public abstract double PixelFromDipF(IControl control, double value);

    public abstract void SetScrollBar(
        IControl control,
        bool isVertical,
        bool visible,
        int value,
        int largeChange,
        int maximum);

    public abstract bool IsScrollBarVisible(IControl control, bool isVertical);

    public abstract int GetScrollBarValue(IControl control, bool isVertical);

    public abstract int GetScrollBarLargeChange(IControl control, bool isVertical);

    public abstract int GetScrollBarMaximum(IControl control, bool isVertical);

    public abstract void ResetBackgroundColor(IControl control);

    public abstract void ResetForegroundColor(IControl control);

    public abstract void SetEnabled(IControl control, bool value);

    public abstract bool GetProcessUIUpdates(IControl control);

    public abstract void SetProcessUIUpdates(IControl control, bool value);

    public abstract Color GetDefaultAttributesBgColor(IControl control);

    public abstract Color GetDefaultAttributesFgColor(IControl control);

    public abstract Font? GetDefaultAttributesFont(IControl control);

    public abstract void SendMouseDownEvent(IControl control, int x, int y);

    public abstract void SendMouseUpEvent(IControl control, int x, int y);

    public abstract bool BeginRepositioningChildren(IControl control);

    public abstract void EndRepositioningChildren(IControl control);

    public abstract void AlwaysShowScrollbars(IControl control, bool hflag = true, bool vflag = true);

    public abstract void Update(IControl control);

    public abstract void Invalidate(IControl control);

    public abstract IntPtr GetHandle(IControl control);

    public abstract SizeD GetPreferredSize(IControl control, SizeD availableSize);

    public abstract int GetScrollBarEvtPosition(IControl control);

    public abstract ScrollEventType GetScrollBarEvtKind(IControl control);

    public abstract Graphics OpenPaintDrawingContext(IControl control);

    public abstract ControlBorderStyle GetBorderStyle(IControl control);

    public abstract void SetBorderStyle(IControl control, ControlBorderStyle value);

    public abstract LangDirection GetLangDirection(IControl control);

    public abstract void SetLangDirection(IControl control, LangDirection value);

    public abstract IControl? GetFocusedControl();

    public abstract Color GetClassDefaultAttributesBgColor(
        ControlTypeId controlType,
        ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

    public abstract Color GetClassDefaultAttributesFgColor(
        ControlTypeId controlType,
        ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

    public abstract Font? GetClassDefaultAttributesFont(
        ControlTypeId controlType,
        ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);
}