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
}