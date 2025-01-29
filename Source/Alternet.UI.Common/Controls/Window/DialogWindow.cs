using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// A dialog box is a <see cref="Window"/> descendant with a title bar and sometimes
    /// a system menu, which can be moved around the screen. It can contain controls and
    /// other windows and is often used to allow the user to make some choice
    /// or to answer a question.
    /// </summary>
    public partial class DialogWindow : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogWindow"/> class.
        /// </summary>
        public DialogWindow()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogWindow"/> class.
        /// </summary>
        /// <param name="windowKind">Window kind to use instead of default value.</param>
        /// <remarks>
        /// Fo example, this constructor allows to use window as control
        /// (specify <see cref="WindowKind.Control"/>) as a parameter.
        /// </remarks>
        public DialogWindow(WindowKind windowKind)
            : base(windowKind)
        {
        }
    }
}
