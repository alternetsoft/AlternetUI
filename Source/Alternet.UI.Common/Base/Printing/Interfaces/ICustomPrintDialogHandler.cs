using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    public interface ICustomPrintDialogHandler : IDisposable
    {
        bool ShowHelp { get; set; }

        string? Title { get; set; }

        ModalResult ShowModal(Window? owner);

        void SetDocument(IPrintDocumentHandler? value);
    }
}
