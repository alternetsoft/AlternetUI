using Alternet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ArrangedElementControl : IArrangedElementLite
    {
        Control control;

        public ArrangedElementControl(Control control)
        {
            this.control = control;
        }

        public bool Visible
        {
            get => control.Visible;
            set => control.Visible = value;
        }

        public Rect Bounds
        {
            get => control.Bounds;
            set => control.Bounds = value;
        }

        public Size ClientSize => control.ClientSize;

        public Thickness Padding
        {
            get => control.Padding;
            set => control.Padding = value;
        }

        public Thickness Margin
        {
            get => control.Margin;
            set => control.Margin = value;
        }

        public Size GetPreferredSize(Size proposedSize)
        {
            return control.GetPreferredSize(proposedSize);
        }
    }
}
