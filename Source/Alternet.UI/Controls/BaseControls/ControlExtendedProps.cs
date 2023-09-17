using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ControlExtendedProps
    {
        public int GridColumn { get; set; } = 0;

        public int GridRow { get; set; } = 0;

        public int GridColumnSpan { get; set; } = 1;

        public int GridRowSpan { get; set; } = 1;

        internal double DistanceRight { get; set; }

        internal double DistanceBottom { get; set; }

        internal AutoSizeMode AutoSizeMode { get; set; }

        internal Size MinimumSize { get; set; }

        internal AnchorStyles Anchor { get; set; }

        internal DockStyle Dock { get; set; }

        internal bool AutoSize { get; set; }
    }
}