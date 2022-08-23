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
        public PreviewData(string imageFileName, Size desiredSize)
        {
            ImageFileName = imageFileName;
            DesiredSize = desiredSize;
        }

        public string ImageFileName { get; }

        public System.Drawing.Size DesiredSize { get; }
    }
}
