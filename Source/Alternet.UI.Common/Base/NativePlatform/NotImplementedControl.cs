using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI;

internal class NotImplementedControl : NativeControl
{
    public override bool CanAcceptFocus(IControl control)
    {
        throw new NotImplementedException();
    }

    public override Color GetBackgroundColor(IControl control)
    {
        throw new NotImplementedException();
    }

    public override RectD GetBounds(IControl control)
    {
        throw new NotImplementedException();
    }

    public override SizeD GetClientSize(IControl control)
    {
        throw new NotImplementedException();
    }

    public override Font? GetFont(IControl control)
    {
        throw new NotImplementedException();
    }

    public override Color GetForegroundColor(IControl control)
    {
        throw new NotImplementedException();
    }

    public override Thickness GetIntrinsicLayoutPadding(IControl control)
    {
        throw new NotImplementedException();
    }

    public override Thickness GetIntrinsicPreferredSizePadding(IControl control)
    {
        throw new NotImplementedException();
    }

    public override bool GetIsScrollable(IControl control)
    {
        throw new NotImplementedException();
    }

    public override SizeD GetMaximumSize(IControl control)
    {
        throw new NotImplementedException();
    }

    public override SizeD GetMinimumSize(IControl control)
    {
        throw new NotImplementedException();
    }

    public override bool GetUserPaint(IControl control)
    {
        throw new NotImplementedException();
    }

    public override bool GetVisible(IControl control)
    {
        throw new NotImplementedException();
    }

    public override bool IsFocusable(IControl control)
    {
        throw new NotImplementedException();
    }

    public override bool IsHandleCreated(IControl control)
    {
        throw new NotImplementedException();
    }

    public override bool IsMouseCaptured(IControl control)
    {
        throw new NotImplementedException();
    }

    public override bool IsMouseOver(IControl control)
    {
        throw new NotImplementedException();
    }

    public override void SetBackgroundColor(IControl control, Color value)
    {
        throw new NotImplementedException();
    }

    public override void SetBounds(IControl control, RectD value)
    {
        throw new NotImplementedException();
    }

    public override void SetClientSize(IControl control, SizeD value)
    {
        throw new NotImplementedException();
    }

    public override void SetCursor(IControl control, Cursor? value)
    {
        throw new NotImplementedException();
    }

    public override void SetFont(IControl control, Font? value)
    {
        throw new NotImplementedException();
    }

    public override void SetForegroundColor(IControl control, Color value)
    {
        throw new NotImplementedException();
    }

    public override void SetIsScrollable(IControl control, bool value)
    {
        throw new NotImplementedException();
    }

    public override void SetMaximumSize(IControl control, SizeD value)
    {
        throw new NotImplementedException();
    }

    public override void SetMinimumSize(IControl control, SizeD value)
    {
        throw new NotImplementedException();
    }

    public override void SetToolTip(IControl control, string? value)
    {
        throw new NotImplementedException();
    }

    public override void SetUserPaint(IControl control, bool value)
    {
        throw new NotImplementedException();
    }

    public override void SetVisible(IControl control, bool value)
    {
        throw new NotImplementedException();
    }
}