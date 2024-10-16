using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which allow to implement custom painted items for the
    /// <see cref="VirtualListBox"/> control.
    /// </summary>
    public interface IListBoxItemPainter
    {
        /// <summary>
        /// Called by owner drawn <see cref="VirtualListBox"/> control for the item
        /// painting.
        /// </summary>
        /// <param name="sender"><see cref="VirtualListBox"/> control.</param>
        /// <param name="e">Paint arguments</param>
        void Paint(object sender, ListBoxItemPaintEventArgs e);

        /// <summary>
        /// Called by owner drawn <see cref="VirtualListBox"/> control for the background
        /// painting.
        /// </summary>
        /// <remarks>
        /// This method must return <c>true</c> if default background
        /// painting is not needed. The simplest implemetation is to return <c>false</c>.
        /// In this case default background will be painted.
        /// </remarks>
        /// <param name="sender"><see cref="VirtualListBox"/> control.</param>
        /// <param name="e">Paint arguments</param>
        bool PaintBackground(object sender, ListBoxItemPaintEventArgs e);

        /// <summary>
        /// Called by owner drawn <see cref="VirtualListBox"/> control in order
        /// to get size of the item. Return (-1, -1) to use default behavior.
        /// </summary>
        /// <param name="sender"><see cref="VirtualListBox"/> control.</param>
        /// <param name="index">Index of the item.</param>
        /// <returns></returns>
        SizeD GetSize(object sender, int index);
    }
}
