using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with group box control.
    /// </summary>
    public interface IGroupBoxHandler : IControlHandler
    {
        /// <summary>
        /// Gets offset for the top border.
        /// </summary>
        /// <returns></returns>
        int GetTopBorderForSizer();

        /// <summary>
        /// Gets offset for all borders except top border.
        /// </summary>
        /// <returns></returns>
        int GetOtherBorderForSizer();
    }
}
