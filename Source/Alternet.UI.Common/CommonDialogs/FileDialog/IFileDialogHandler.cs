using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IFileDialogHandler : IDialogHandler
    {
        bool NoShortcutFollow { get; set; }

        bool ChangeDir { get; set; }

        bool PreviewFiles { get; set; }

        bool ShowHiddenFiles { get; set; }

        string? InitialDirectory { get; set; }

        string? Filter { get; set; }

        int SelectedFilterIndex { get; set; }

        string? FileName { get; set; }
    }
}
