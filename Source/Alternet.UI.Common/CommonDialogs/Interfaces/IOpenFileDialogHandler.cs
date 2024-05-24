using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IOpenFileDialogHandler : IFileDialogHandler
    {
        bool FileMustExist { get; set; }

        bool AllowMultipleSelection { get; set; }

        string[] FileNames { get; }
    }
}
