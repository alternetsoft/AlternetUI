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
    public interface IListBoxHandler : IControlHandler
    {
        /// <inheritdoc cref="ListBox.HasBorder"/>
        bool HasBorder { get; set; }

        /// <inheritdoc cref="ListBox.EnsureVisible"/>
        void EnsureVisible(int itemIndex);

        /// <inheritdoc cref="ListBox.HitTest"/>
        int? HitTest(PointD position);
    }
}
