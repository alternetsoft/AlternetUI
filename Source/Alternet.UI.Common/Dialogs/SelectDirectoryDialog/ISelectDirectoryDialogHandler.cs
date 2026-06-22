using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with select directory dialog window.
    /// </summary>
    public interface ISelectDirectoryDialogHandler : IDialogHandler
    {
        string? GetInitialDirectory();

        void SetInitialDirectory(string? value);

        string? GetDirectoryName();

        void SetDirectoryName(string? value);
    }
}
