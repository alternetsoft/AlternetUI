using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Contains methods and properties which allow to control page setup dialog.
    /// </summary>
    public interface IPageSetupDialogHandler : IBasePrintDialogHandler
    {
        /// <inheritdoc cref="PageSetupDialog.MinMargins"/>
        Thickness? MinMargins { get; set; }

        /// <inheritdoc cref="PageSetupDialog.AllowMargins"/>
        bool AllowMargins { get; set; }

        /// <inheritdoc cref="PageSetupDialog.AllowOrientation"/>
        bool AllowOrientation { get; set; }

        /// <inheritdoc cref="PageSetupDialog.AllowPaper"/>
        bool AllowPaper { get; set; }

        /// <inheritdoc cref="PageSetupDialog.AllowPrinter"/>
        bool AllowPrinter { get; set; }
    }
}
