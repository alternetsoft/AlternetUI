using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal partial class ObjectInitializers
    {
        public static void InitScrollBar(object control)
        {
            if (control is not ScrollBar scrollBar)
                return;
            scrollBar.SuggestedWidth = 250;
        }
    }
}
