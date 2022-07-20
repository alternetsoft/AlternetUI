using Alternet.UI.Integration.VisualStudio.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Serilog;
using System;

namespace Alternet.UI.Integration.VisualStudio.Models
{
    public class PreviewData
    {
        public PreviewData(IntPtr windowHandle)
        {
            WindowHandle = windowHandle;
        }

        public IntPtr WindowHandle { get; }
    }
}
