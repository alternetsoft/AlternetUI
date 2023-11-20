using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class with properties and methods common to all Alternet.UI objects.
    /// </summary>
    public class BaseObject : IBaseObject
    {
        /// <inheritdoc/>
        public virtual void Required()
        {
        }
    }
}
