using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// A miniframe is a <see cref="Window"/> descendant with a small title bar.
    /// </summary>
    /// <remarks>
    /// It is suitable for floating toolbars that must not take up too much screen area.
    /// </remarks>
    public class MiniFrameWindow : Window
    {
        internal override WindowKind GetWindowKind() => WindowKind.MiniFrame;
    }
}
