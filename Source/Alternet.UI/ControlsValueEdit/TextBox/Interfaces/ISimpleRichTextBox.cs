using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the <see cref="RichTextBox"/> or <see cref="TextBox"/> methods
    /// and properties.
    /// </summary>
    internal interface ISimpleRichTextBox
    {
        /// <inheritdoc cref="RichTextBox.LastLineNumber"/>
        long LastLineNumber { get; }

        /// <inheritdoc cref="RichTextBox.InsertionPointLineNumber"/>
        long InsertionPointLineNumber { get; }

        /// <inheritdoc cref="RichTextBox.XYToPosition"/>
        long XYToPosition(long x, long y);

        /// <inheritdoc cref="RichTextBox.SetInsertionPoint"/>
        void SetInsertionPoint(long pos);

        /// <inheritdoc cref="RichTextBox.ShowPosition"/>
        void ShowPosition(long pos);
    }
}
