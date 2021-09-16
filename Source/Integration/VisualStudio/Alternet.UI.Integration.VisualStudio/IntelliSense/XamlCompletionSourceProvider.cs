using System;
using System.ComponentModel.Composition;
using Alternet.UI.Integration.VisualStudio.Models;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Alternet.UI.Integration.VisualStudio.IntelliSense
{
    [Export(typeof(ICompletionSourceProvider))]
    [ContentType("xml")]
    [Name("Alternet UIXML Completion")]
    internal class XamlCompletionSourceProvider : ICompletionSourceProvider
    {
        private readonly IVsImageService2 _imageService;
        private readonly IServiceProvider serviceProvider;

        [ImportingConstructor]
        public XamlCompletionSourceProvider(
            [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            _imageService = serviceProvider.GetService<IVsImageService2, SVsImageService>();
        }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            if (textBuffer.Properties.ContainsProperty(typeof(XamlBufferMetadata)))
            {
                return new XamlCompletionSource(serviceProvider, textBuffer, _imageService);
            }

            return null;
        }
    }
}
