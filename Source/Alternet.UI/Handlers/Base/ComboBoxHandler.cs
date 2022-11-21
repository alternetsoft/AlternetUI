using Alternet.Drawing;
using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="ComboBox"/> behavior and appearance.
    /// </summary>
    public abstract class ComboBoxHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new ComboBox Control => (ComboBox)base.Control;

        /// <summary>
        /// Gets the starting index of text selected in the combo box.
        /// </summary>
        /// <value>The zero-based index of the first character in the string of the current text selection.</value>
        public abstract int TextSelectionStart { get; }

        /// <summary>
        /// Gets the number of characters selected in the editable portion of the combo box.
        /// </summary>
        /// <value>The number of characters selected in the combo box.</value>
        public abstract int TextSelectionLength { get; }

        /// <summary>
        /// Selects a range of text in the editable portion of the ComboBox.
        /// </summary>
        public abstract void SelectTextRange(int start, int length);

        /// <summary>
        /// Selects all the text in the editable portion of the ComboBox.
        /// </summary>
        public abstract void SelectAllText();

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}