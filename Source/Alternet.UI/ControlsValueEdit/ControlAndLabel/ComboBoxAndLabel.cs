using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="ComboBox"/> with attached <see cref="Label"/>.
    /// </summary>
    public class ComboBoxAndLabel : ControlAndLabel
    {
        /// <summary>
        /// Gets main child control.
        /// </summary>
        public new ComboBox MainControl => (ComboBox)base.MainControl;

        /// <summary>
        /// Gets main child control, same as <see cref="MainControl"/>.
        /// </summary>
        public ComboBox TextBox => (ComboBox)base.MainControl;

        /// <inheritdoc/>
        protected override Control CreateControl() => new ComboBox();
    }
}
