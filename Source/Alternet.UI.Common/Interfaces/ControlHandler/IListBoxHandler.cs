using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allows to work with list box control.
    /// </summary>
    public interface IListBoxHandler
    {
        /// <inheritdoc cref="CustomListBox{T}.HasBorder"/>
        bool HasBorder { get; set; }

        /// <inheritdoc cref="CustomListBox{T}.EnsureVisible"/>
        void EnsureVisible(int itemIndex);

        /// <inheritdoc cref="CustomListBox{T}.HitTest"/>
        int? HitTest(PointD position);
    }
}
