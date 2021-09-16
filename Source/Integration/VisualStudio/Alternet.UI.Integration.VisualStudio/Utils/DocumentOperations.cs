using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.IO;
using System.Linq;

namespace Alternet.UI.Integration.VisualStudio.Utils
{
    internal class DocumentOperations
    {
        private EnvDTE80.DTE2 dte2;
        private EnvDTE.DTE dte;

        public DocumentOperations(IServiceProvider serviceProvider)
        {
            dte = serviceProvider.GetService<EnvDTE.DTE>();
            dte2 = dte as EnvDTE80.DTE2;
        }

        public EnvDTE.Document TryFindOpenDocument(string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            var fullFilePath = Path.GetFullPath(filePath);

            ThreadHelper.ThrowIfNotOnUIThread();

            foreach (var document in dte2.Documents.OfType<EnvDTE.Document>())
            {
                if (fullFilePath.Equals(Path.GetFullPath(document.FullName), StringComparison.OrdinalIgnoreCase))
                    return document;
            }

            return null;
        }

        public EnvDTE.TextDocument GetTextDocumentFromDocument(EnvDTE.Document document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return (EnvDTE.TextDocument)document.Object("TextDocument");
        }

        public string GetDocumentText(EnvDTE.TextDocument document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return document.StartPoint.CreateEditPoint().GetText(document.EndPoint);
        }

        public EnvDTE.Document GetActiveDocument()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return dte.ActiveDocument;
        }
        
        public void OpenFile(string fileName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            dte.ItemOperations.OpenFile(fileName);
        }
    }
}
