using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods and properties of the <see cref="BaseControlItem"/>.
    /// </summary>
    public interface IBaseControlItem : IBaseObject
    {
        /// <summary>
        /// Gets unique id of this object.
        /// </summary>
        ObjectUniqueId UniqueId { get; }
    }
}
