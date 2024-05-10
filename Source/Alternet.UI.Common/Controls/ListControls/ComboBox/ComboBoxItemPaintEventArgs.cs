using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="IComboBoxItemPainter.Paint"/> event.
    /// </summary>
    public class ComboBoxItemPaintEventArgs
        : PaintEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxItemPaintEventArgs"/> class.
        /// </summary>
        /// <param name="control">Control which owns the item.</param>
        /// <param name="graphics"><see cref="Graphics"/> where drawing is performed.</param>
        /// <param name="bounds">Bounds of the item.</param>
        public ComboBoxItemPaintEventArgs(ComboBox control, Graphics graphics, RectD bounds)
            : base(graphics, bounds)
        {
            ComboBox = control;
        }

        /// <summary>
        /// Gets whether item is selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets whether painting is done inside <see cref="ComboBox"/> (<c>true</c>)
        /// or in the popup control (<c>false</c>).
        /// </summary>
        public bool IsPaintingControl { get; set; }

        /// <summary>
        /// Get whether item index is not found.
        /// </summary>
        public bool IsIndexNotFound { get; set; }

        /// <summary>
        /// Gets index of the item.
        /// </summary>
        public int ItemIndex { get; set; }

        /// <summary>
        /// Gets whether background painting need to be performed.
        /// </summary>
        public bool IsPaintingBackground { get; set; }

        internal ComboBox ComboBox { get; set; }

        /// <summary>
        /// Default drawing method.
        /// </summary>
        public void DefaultPaint()
        {
            if (IsPaintingBackground)
                ComboBox.Handler.DefaultOnDrawBackground();
            else
                ComboBox.Handler.DefaultOnDrawItem();
        }
    }
}
