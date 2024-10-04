using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to work with dialog which allow to select color.
    /// </summary>
    public interface IColorDialogHandler : IDialogHandler
    {
        /// <inheritdoc cref="ColorDialog.Color"/>
        Color Color { get; set; }
    }
}
