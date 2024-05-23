using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface ISelectDirectoryDialogHandler : IDialogHandler
    {
        string? InitialDirectory { get; set; }

        string? DirectoryName { get; set; }
    }
}
