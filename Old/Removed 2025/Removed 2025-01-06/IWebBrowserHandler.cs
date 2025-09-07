using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="IWebBrowserLite"/>
    /// with additional properties and methods specific to web browser control.
    /// </summary>
    public interface IWebBrowserHandler : IWebBrowserLite
    {
        /// <summary>
        /// Gets or sets whether or not control has border.
        /// </summary>
        bool HasBorder { get; set; }
    }
}
