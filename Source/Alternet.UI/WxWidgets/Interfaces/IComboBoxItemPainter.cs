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
        /// <summary>
        /// Called by owner drawn <see cref="ComboBox"/> control for the item
        /// painting.
        /// </summary>
        /// <param name="sender"><see cref="ComboBox"/> control.</param>
        /// <param name="e">Paint arguments</param>
        void Paint(ComboBox sender, ComboBoxItemPaintEventArgs e);

        /// <summary>
        /// Called by owner drawn <see cref="ComboBox"/> control in order
        /// to get height of the item. Return -1 to use default behavior.
        /// </summary>
        /// <param name="sender"><see cref="ComboBox"/> control.</param>
        /// <param name="index">Index of the item.</param>
        /// <param name="defaultHeight">Default height of the item.</param>
        /// <returns></returns>
        Coord GetHeight(ComboBox sender, int index, Coord defaultHeight);

        /// <summary>
        /// Called by owner drawn <see cref="ComboBox"/> control in order
        /// to get width of the item. Return -1 to use default behavior.
        /// </summary>
        /// <param name="sender"><see cref="ComboBox"/> control.</param>
        /// <param name="index">Index of the item.</param>
        /// <param name="defaultWidth">Default width of the item.</param>
        /// <returns></returns>
        Coord GetWidth(ComboBox sender, int index, Coord defaultWidth);
    }
}
