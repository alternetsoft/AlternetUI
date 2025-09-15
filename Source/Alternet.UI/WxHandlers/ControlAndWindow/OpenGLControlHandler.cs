using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class OpenGLControlHandler : WxControlHandler
    {
        public override bool Visible
        {
            get => base.Visible;
            set
            {
                base.Visible = value;
            }
        }

        public override void OnHandleCreated()
        {
            base.OnHandleCreated();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.GLControl();
        }
    }
}
