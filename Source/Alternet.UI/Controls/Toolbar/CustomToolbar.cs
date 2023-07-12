using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class CustomToolbar : Toolbar
    {
        /// <inheritdoc />
        protected override ControlHandler CreateHandler()
        {
            return new NativeToolbarHandler(false);
        }
    }
}
