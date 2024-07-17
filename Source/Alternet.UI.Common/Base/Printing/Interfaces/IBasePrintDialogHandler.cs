using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Extends <see cref="IDialogHandler"/> with additional methods and properties
    /// which allow to perform operations with print dialogs.
    /// </summary>
    public interface IBasePrintDialogHandler : IDialogHandler
    {
        /// <summary>
        /// Sets <see cref="IPrintDocumentHandler"/> object to the print dialog.
        /// </summary>
        /// <param name="value">Print document.</param>
        void SetDocument(IPrintDocumentHandler? value);
    }
}
