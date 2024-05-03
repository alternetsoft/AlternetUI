using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class WxPlatform
    {
        public SizeD ControlGetClientSize(Control control)
        {
            return control.NativeControl.ClientSize;
        }

        public void ControlSetClientSize(Control control, SizeD value)
        {
            control.NativeControl.ClientSize = value;
        }

        public void ControlSetCursor(Control control, Cursor? value)
        {
            if (value is null)
                control.NativeControl.SetCursor(default);
            else
                control.NativeControl.SetCursor((IntPtr)value.NativeObject);
        }

        public bool ControlGetIsScrollable(Control control)
        {
            return control.NativeControl.IsScrollable;
        }

        public void ControlSetIsScrollable(Control control, bool value)
        {
            control.NativeControl.IsScrollable = value;
        }
    }
}
