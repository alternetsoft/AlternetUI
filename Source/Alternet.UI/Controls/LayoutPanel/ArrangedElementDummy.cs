using Alternet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ArrangedElementDummy : IArrangedElementLite
    {
        public bool Visible { get; set; }
        public Rect Bounds { get; set; }

        public Size ClientSize { get; set; }

        public Thickness Padding { get; set; }

        public Thickness Margin { get; set; }

        public Size GetPreferredSize(Size proposedSize)
        {
            return proposedSize;
        }
    }
}
