using Microsoft.VisualStudio.Text;
using UITextChange = Alternet.UI.Integration.IntelliSense.ITextChange;

namespace Alternet.UI.Integration.VisualStudio.IntelliSense
{
    public class TextChangeAdapter : UITextChange
    {
        private readonly ITextChange _textChange;

        public TextChangeAdapter(ITextChange textChange)
        {
            _textChange = textChange;
        }

        /// <inheritdoc/>
        public int NewPosition => _textChange.NewPosition;

        /// <inheritdoc/>
        public string NewText => _textChange.NewText;

        /// <inheritdoc/>
        public int OldPosition => _textChange.OldPosition;

        /// <inheritdoc/>
        public string OldText => _textChange.OldText;
    }
}
