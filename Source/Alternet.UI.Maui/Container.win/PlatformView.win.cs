using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS

namespace Alternet.UI
{
    /// <summary>
    /// Adds additional functionality to the <see cref="SkiaSharp.Views.Windows.SKXamlCanvas"/> control.
    /// </summary>
    public class PlatformView : SkiaSharp.Views.Windows.SKXamlCanvas
    {
    }
}

#endif