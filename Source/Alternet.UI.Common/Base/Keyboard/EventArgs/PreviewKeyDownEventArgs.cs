using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the preview key down events.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="PreviewKeyDownEventArgs" /> that contains the event data.</param>
    public delegate void PreviewKeyDownEventHandler(object? sender, PreviewKeyDownEventArgs e);

    /// <summary>
    /// Provides data for the key preview events.
    /// </summary>
    public class PreviewKeyDownEventArgs : CustomKeyEventArgs
    {
        private bool isInputKey;

        /// <summary>
        /// Constructs an instance of the <see cref="PreviewKeyDownEventArgs"/> class.
        /// </summary>
        public PreviewKeyDownEventArgs()
        {
        }

        /// <summary>
        /// Constructs an instance of the <see cref="PreviewKeyDownEventArgs"/> class
        /// using properties of <see cref="KeyEventArgs"/>.
        /// </summary>
        public PreviewKeyDownEventArgs(KeyEventArgs e)
            : this(e.CurrentTarget, e.Key, e.ModifierKeys)
        {
        }

        /// <summary>
        /// Constructs an instance of the <see cref="PreviewKeyDownEventArgs"/> class.
        /// </summary>
        /// <param name="key">
        /// The key referenced by the event.
        /// </param>
        /// <param name="modifiers">The set of modifier keys pressed
        /// at the time when event was received.</param>
        /// <param name="originalTarget">Original target object which received the event.</param>
        public PreviewKeyDownEventArgs(
            object originalTarget,
            Key key,
            ModifierKeys modifiers)
            : base(originalTarget, key, modifiers)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether a key is a regular input key.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the key is a regular input key;
        /// otherwise, <see langword="false" />.
        /// </returns>
        public bool IsInputKey
        {
            get
            {
                return isInputKey;
            }

            set
            {
                isInputKey = value;
            }
        }
    }
}
