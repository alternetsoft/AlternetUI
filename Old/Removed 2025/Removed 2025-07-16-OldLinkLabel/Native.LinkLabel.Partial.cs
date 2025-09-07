﻿using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class LinkLabel
    {
        public void OnPlatformEventHyperlinkClick(CancelEventArgs cea)
        {
            (UIControl as UI.LinkLabel)?.RaiseLinkClicked(cea);
        }
    }
}