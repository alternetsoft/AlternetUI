using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains properties and methods which allow to work with
    /// custom preview control.
    /// </summary>
    public interface IFilePreview
    {
        /// <summary>
        /// Gets or sets path to the previewed file.
        /// </summary>
        string? FileName { get; set; }

        /// <summary>
        /// Gets preview control.
        /// </summary>
        Control Control { get; }
    }
}
