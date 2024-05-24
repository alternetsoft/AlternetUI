using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IDialogHandler : IDisposable
    {
        /// <summary>
        /// Gets or sets the dialog window title.
        /// </summary>
        string? Title { get; set; }

        bool ShowHelp { get; set; }

        ModalResult ShowModal(Window? owner);
    }
}
