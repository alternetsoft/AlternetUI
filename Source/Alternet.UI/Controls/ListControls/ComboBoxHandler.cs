using System;
using System.Collections.Generic;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific
    /// <see cref="ComboBox"/> behavior and appearance.
    /// </summary>
    internal abstract class ComboBoxHandler : ControlHandler
    {
        /// <summary>
        /// Returns <see cref="ComboBox"/> instance with which this handler
        /// is associated.
        /// </summary>
        public new ComboBox Control => (ComboBox)base.Control;

        /// <summary>
        /// Gets the starting index of text selected in the combo box.
        /// </summary>
        /// <value>The zero-based index of the first character in the string
        /// of the current text selection.</value>
        public abstract int TextSelectionStart { get; }

        /// <summary>
        /// Gets the number of characters selected in the editable
        /// portion of the combo box.
        /// </summary>
        /// <value>The number of characters selected in the combo box.</value>
        public abstract int TextSelectionLength { get; }

        internal abstract bool HasBorder { get; set; }

        /// <summary>
        /// Selects a range of text in the editable portion of the ComboBox.
        /// </summary>
        public abstract void SelectTextRange(int start, int length);

        /// <summary>
        /// Selects all the text in the editable portion of the ComboBox.
        /// </summary>
        public abstract void SelectAllText();
    }
}