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
}