using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI;

public class MauiPlatformWindow : NativeWindow
{
    /// <inheritdoc/>
    public override void Activate(IWindow window)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void Close(IWindow window)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override object CreateWindowHandler()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Window? GetActiveWindow()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override bool GetModal(IWindow window)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ModalResult GetModalResult(IWindow window)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Window[] GetOwnedWindows(IWindow window)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override WindowStartLocation GetStartLocation(IWindow window)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override WindowState GetState(IWindow window)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override bool IsActive(IWindow window)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void SetDefaultBounds(RectD defaultBounds)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void SetIsPopupWindow(IWindow window, bool value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void SetModalResult(IWindow window, ModalResult value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void SetStartLocation(IWindow window, WindowStartLocation value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void SetState(IWindow window, WindowState value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void SetStatusBar(IWindow window, FrameworkElement? oldValue, FrameworkElement? value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ModalResult ShowModal(IWindow window, IWindow? owner)
    {
        throw new NotImplementedException();
    }
}