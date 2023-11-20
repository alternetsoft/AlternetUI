using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class with properties and methods common to all control items like
    /// <see cref="TreeViewItem"/>, <see cref="ListViewItem"/> and <see cref="ListControlItem"/>.
    /// </summary>
    public class BaseControlItem : BaseObject, IBaseControlItem
    {
        private ObjectUniqueId? uniqueId;

        /// <inheritdoc/>
        public ObjectUniqueId UniqueId
        {
            get
            {
                return uniqueId ??= new();
            }
        }
    }
}
