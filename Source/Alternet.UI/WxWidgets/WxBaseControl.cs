using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI;

/// <summary>
/// Base class for WxWidgets controls.
/// </summary>
public class WxBaseControl : Control
{
    /// <summary>
    /// Gets <see cref="NativeControl"/> attached to this control.
    /// </summary>
    internal Native.Control NativeControl => ((WxControlHandler)Handler).NativeControl;

    internal new WxControlHandler Handler => (WxControlHandler)base.Handler;

    internal IControlHandlerFactory? ControlHandlerFactory { get; set; }

    /// <summary>
    /// Pops up the given menu at the specified coordinates, relative to this window,
    /// and returns control when the user has dismissed the menu.
    /// </summary>
    /// <remarks>
    /// If a menu item is selected, the corresponding menu event is generated and will
    /// be processed as usual. If coordinates are not specified (-1,-1), the current
    /// mouse cursor position is used.
    /// </remarks>
    /// <remarks>
    /// It is recommended to not explicitly specify coordinates when calling PopupMenu
    /// in response to mouse click, because some of the ports(namely, on Linux)
    /// can do a better job of positioning the menu in that case.
    /// </remarks>
    /// <param name="menu">The menu to pop up.</param>
    /// <param name="x">The X position in dips where the menu will appear.</param>
    /// <param name="y">The Y position in dips where the menu will appear.</param>
    /// <remarks>Position is specified in device independent units (1/96 inch).</remarks>
    public virtual void ShowPopupMenu(ContextMenu? menu, double x = -1, double y = -1)
    {
        ControlUtils.ShowPopupMenu(this, menu, x, y);
    }

    /// <summary>
    /// Gets an <see cref="IControlHandlerFactory"/> to use when creating
    /// new control handlers for this control.
    /// </summary>
    internal IControlHandlerFactory GetEffectiveControlHandlerHactory() =>
        ControlHandlerFactory ??
            Application.Current.VisualTheme.ControlHandlerFactory;

    /// <inheritdoc/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
#if DEBUG
        KeyInfo.Run(KnownKeys.ShowDeveloperTools, e, DebugUtils.ShowDeveloperTools);
#endif
    }

    /// <summary>
    ///     Virtual method reporting the right mouse button was pressed
    /// </summary>
    protected override void OnMouseRightButtonDown(MouseEventArgs e)
    {
        base.OnMouseRightButtonDown(e);
        ShowPopupMenu(ContextMenuStrip);
    }
}