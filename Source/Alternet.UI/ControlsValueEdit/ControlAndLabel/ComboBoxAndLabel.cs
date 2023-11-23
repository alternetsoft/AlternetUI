using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="UI.ComboBox"/> with attached <see cref="Label"/>.
    /// </summary>
    public class ComboBoxAndLabel : ControlAndLabel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxAndLabel"/> class.
        /// </summary>
        /// <param name="label">Label text.</param>
        public ComboBoxAndLabel(string label)
        {
            Label.Text = label;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxAndLabel"/> class.
        /// </summary>
        public ComboBoxAndLabel()
        {
        }

        /// <summary>
        /// Gets main child control.
        /// </summary>
        public new ComboBox MainControl => (ComboBox)base.MainControl;

        /// <summary>
        /// Gets main child control, same as <see cref="MainControl"/>.
        /// </summary>
        public ComboBox ComboBox => (ComboBox)base.MainControl;

        /// <inheritdoc/>
        protected override Control CreateControl() => new ComboBox();
    }
}
