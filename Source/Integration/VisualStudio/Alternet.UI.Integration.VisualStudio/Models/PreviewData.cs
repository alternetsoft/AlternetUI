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
        public PreviewData(string imageFileName)
        {
            ImageFileName = imageFileName;
        }

        public string ImageFileName { get; }
    }
}
