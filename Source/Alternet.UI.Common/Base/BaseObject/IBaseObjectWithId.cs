using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="IBaseObject"/> with <see cref="UniqueId"/>
    /// and id related features.
    /// </summary>
    public interface IBaseObjectWithId : IBaseObject
    {
        /// <summary>
        /// Gets unique id of this object.
        /// </summary>
        ObjectUniqueId UniqueId { get; }
    }
}
