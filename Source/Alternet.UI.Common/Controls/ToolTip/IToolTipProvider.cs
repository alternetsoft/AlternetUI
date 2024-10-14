using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with tooltips.
    /// </summary>
    public interface IToolTipProvider
    {
        IRichToolTip? Get(object? sender);
    }
}