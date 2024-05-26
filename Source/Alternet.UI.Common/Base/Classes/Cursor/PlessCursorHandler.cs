using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class PlessCursorHandler : DisposableObject, ICursorHandler
    {
        public virtual bool IsOk
        {
            get => true;
        }

        public virtual PointI GetHotSpot()
        {
            return PointI.Empty;
        }
    }
}
