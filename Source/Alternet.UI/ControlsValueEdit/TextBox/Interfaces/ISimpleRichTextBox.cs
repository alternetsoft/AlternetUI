using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the <see cref="RichTextBox"/> or <see cref="TextBox"/> methods
    /// and properties.
    /// </summary>
    internal interface ISimpleRichTextBox
    {
        /// <inheritdoc cref="RichTextBox.CurrentPositionChanged"/>
        event EventHandler? CurrentPositionChanged;

        /// <summary>
        /// Gets or sets name of the control.
        /// </summary>
        string? Name { get; set; }

        /// <inheritdoc cref="RichTextBox.CurrentPosition"/>
        Int32Point? CurrentPosition { get; set; }

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
