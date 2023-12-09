using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="Window"/> with <see cref="LogListBox"/> control inside.
    /// </summary>
    /// <remarks>
    /// <see cref="Application.Log"/> method is automatically forwarded to the attached
    /// <see cref="ListBox"/>.
    /// </remarks>
    public class WindowLogListBox : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowLogListBox"/> class.
        /// </summary>
        public WindowLogListBox()
        {
            Size = (800, 600);
            ListBox = new()
            {
                Parent = this,
            };
            ListBox.BindApplicationLog();
            ListBox.ContextMenu.Required();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowLogListBox"/> class.
        /// </summary>
        public WindowLogListBox(Action runAction)
            : this()
        {
            runAction();
        }

        /// <summary>
        /// Gets attached <see cref="LogListBox"/> control.
        /// </summary>
        public LogListBox ListBox { get; }
    }
}
