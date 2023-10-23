﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class Calendar : Control
    {
        [Browsable(false)]
        internal new NativeCalendarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeCalendarHandler)base.Handler;
            }
        }

        internal Native.Calendar NativeControl => Handler.NativeControl;

        protected override ControlHandler CreateHandler()
        {
            return new NativeCalendarHandler();
        }
    }
}
