using CompletionMetadata = Alternet.UI.Integration.IntelliSense.Metadata;

namespace Alternet.UI.Integration.VisualStudio.Models
{
    internal class XamlBufferMetadata
    {
        public CompletionMetadata CompletionMetadata { get; set; }

        public bool NeedInvalidation { get; set; } = true;
    }
}
