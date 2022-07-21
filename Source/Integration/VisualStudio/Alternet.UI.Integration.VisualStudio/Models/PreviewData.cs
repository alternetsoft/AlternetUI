using Alternet.UI.Integration.VisualStudio.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Serilog;
using System;
using System.Drawing;

namespace Alternet.UI.Integration.VisualStudio.Models
{
    public class PreviewData
    {
        public PreviewData(IntPtr windowHandle, Size desiredSize)
        {
            WindowHandle = windowHandle;
            DesiredSize = desiredSize;
        }

        public IntPtr WindowHandle { get; }

        public System.Drawing.Size DesiredSize { get; }
    }
}
