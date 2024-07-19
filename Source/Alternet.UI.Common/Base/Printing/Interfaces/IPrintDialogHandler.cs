using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Contains methods and properties which allow to work with print dialog.
    /// </summary>
    public interface IPrintDialogHandler : IBasePrintDialogHandler
    {
        /// <inheritdoc cref="PrintDialog.AllowSomePages"/>
        bool AllowSomePages { get; set; }

        /// <inheritdoc cref="PrintDialog.AllowSelection"/>
        bool AllowSelection { get; set; }

        /// <inheritdoc cref="PrintDialog.AllowPrintToFile"/>
        bool AllowPrintToFile { get; set; }
    }
}
