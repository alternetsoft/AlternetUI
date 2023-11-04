using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ComboBoxAndLabel : ControlAndLabel
    {
        /// <inheritdoc/>
        protected override Control CreateControl() => new ComboBox();
    }
}
