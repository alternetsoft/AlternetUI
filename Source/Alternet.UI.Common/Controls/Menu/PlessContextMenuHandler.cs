using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class PlessContextMenuHandler : PlessControlHandler, IContextMenuHandler
    {
        public virtual void Show(IControl control, PointD? position = null)
        {
        }
    }
}
