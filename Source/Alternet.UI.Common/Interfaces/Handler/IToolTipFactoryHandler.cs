using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to create and manage tooltips.
    /// </summary>
    public interface IToolTipFactoryHandler : IDisposable
    {
        /// <inheritdoc cref="ToolTipFactory.SetReshow"/>
        bool SetReshow(long msecs);

        /// <inheritdoc cref="ToolTipFactory.SetEnabled"/>
        bool SetEnabled(bool flag);

        /// <inheritdoc cref="ToolTipFactory.SetAutoPop"/>
        bool SetAutoPop(long msecs);

        /// <inheritdoc cref="ToolTipFactory.SetDelay"/>
        bool SetDelay(long msecs);

        /// <inheritdoc cref="ToolTipFactory.SetMaxWidth"/>
        bool SetMaxWidth(int width);
    }
}
