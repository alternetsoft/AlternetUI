using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods and properties of the <see cref="BaseObject"/>.
    /// </summary>
    public interface IBaseObject
    {
        /// <summary>
        /// Marks object as required.
        /// </summary>
        void Required();
    }
}
