using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Descendant of the <see cref="UserControl"/> which doesn't need to have focus.
    /// An example of <see cref="GraphicControl"/> is <see cref="GenericLabel"/>.
    /// </summary>
    public class GraphicControl : UserControl
    {
        /// <inheritdoc/>
        public override bool IsGraphicControl => true;
    }
}
