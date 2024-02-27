using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which allow to implement custom painted items for the
    /// <see cref="ComboBox"/> control.
    /// </summary>
    public interface IComboBoxItemPainter
    {
        void Paint(ComboBox sender, ComboBoxItemPaintEventArgs prm);

        double GetHeight(ComboBox sender, int index, double defaultHeight);

        double GetWidth(ComboBox sender, int index, double defaultWidth);
    }
}
