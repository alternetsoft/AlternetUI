using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class ComboBoxColorPainter : IComboBoxItemPainter
    {
        public double GetHeight(ComboBox sender, int index, double defaultHeight)
        {
            return -1;
        }

        public double GetWidth(ComboBox sender, int index, double defaultWidth)
        {
            return -1;
        }

        public void Paint(ComboBox sender, ComboBoxItemPaintEventArgs e)
        {
            sender.DefaultItemPaint(e);
        }
    }
}
