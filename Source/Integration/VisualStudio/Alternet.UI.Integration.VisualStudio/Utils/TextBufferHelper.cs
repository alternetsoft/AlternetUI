using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;

namespace Alternet.UI.Integration.VisualStudio.Utils
{
    internal static class TextBufferHelper
    {
        public static string GetTextBufferFilePath(ITextBuffer textBuffer)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            textBuffer.Properties.TryGetProperty(typeof(IVsTextBuffer), out IVsTextBuffer bufferAdapter);
            var persistFileFormat = bufferAdapter as Microsoft.VisualStudio.Shell.Interop.IPersistFileFormat;

            if (persistFileFormat == null)
                throw new InvalidOperationException();

            if (persistFileFormat.GetCurFile(out string filePath, out _) != VSConstants.S_OK)
                throw new InvalidOperationException();

            return filePath;
        }
    }
}
