using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    public interface IPageSetupDialogHandler : ICustomPrintDialogHandler
    {
        Thickness? MinMargins { get; set; }

        bool AllowMargins { get; set; }

        bool AllowOrientation { get; set; }

        bool AllowPaper { get; set; }

        bool AllowPrinter { get; set; }
    }
}
