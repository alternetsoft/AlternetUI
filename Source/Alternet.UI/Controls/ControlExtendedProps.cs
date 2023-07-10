using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ControlExtendedProps
    {
        public int GridColumn { get; set; } = 0;

        public int GridRow { get; set; } = 0;

        public int GridColumnSpan { get; set; } = 1;

        public int GridRowSpan { get; set; } = 1;

        //public bool GridIsSharedSizeScopes { get; set; }
    }
}