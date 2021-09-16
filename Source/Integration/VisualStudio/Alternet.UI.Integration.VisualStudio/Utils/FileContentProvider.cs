using Microsoft.VisualStudio.Shell;
using System.IO;

namespace Alternet.UI.Integration.VisualStudio.Utils
{
    internal static class FileContentProvider
    {
        public static string GetFileText(DocumentOperations documentOperations, string fileName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var document = documentOperations.TryFindOpenDocument(fileName);
            if (document == null)
                return File.ReadAllText(fileName);

            return documentOperations.GetDocumentText(documentOperations.GetTextDocumentFromDocument(document));
        }
    }
}