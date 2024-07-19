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
        /// <summary>
        /// Creates <see cref="IRichToolTipHandler"/> interface provider.
        /// </summary>
        /// <param name="title">Tooltip title.</param>
        /// <param name="message">Tooltip message.</param>
        /// <param name="useGeneric">Whether or not to use generic tooltip
        /// which is platform independent.</param>
        /// <returns></returns>
        IRichToolTipHandler CreateRichToolTipHandler(
            string title,
            string message,
            bool useGeneric);

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
