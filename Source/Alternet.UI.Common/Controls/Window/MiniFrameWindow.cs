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
    public partial class MiniFrameWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MiniFrameWindow"/> class.
        /// </summary>
        public MiniFrameWindow()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniFrameWindow"/> class.
        /// </summary>
        /// <param name="windowKind">Window kind to use instead of default value.</param>
        /// <remarks>
        /// Fo example, this constructor allows to use window as control
        /// (specify <see cref="WindowKind.Control"/>) as a parameter.
        /// </remarks>
        public MiniFrameWindow(WindowKind windowKind)
            : base(windowKind)
        {
        }

        /// <inheritdoc />
        public override WindowKind GetWindowKind() => GetWindowKindOverride() ?? WindowKind.MiniFrame;
    }
}
