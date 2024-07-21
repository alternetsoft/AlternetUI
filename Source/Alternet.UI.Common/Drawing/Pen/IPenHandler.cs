using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to manage pen.
    /// </summary>
    public interface IPenHandler : IDisposable
    {
        /// <summary>
        /// Updates native pen properties from the managed pen properties.
        /// </summary>
        /// <param name="pen"></param>
        void Update(Pen pen);
    }
}
