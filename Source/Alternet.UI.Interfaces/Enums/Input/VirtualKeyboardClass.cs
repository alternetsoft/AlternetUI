using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates virtual keyboard classes. Includes default keyboard and specialized
    /// keyboards, such as
    /// those for telephone numbers, emails, URLs and other.
    /// </summary>
    public enum VirtualKeyboardClass
    {
        /// <summary>
        /// Use default keyboard.
        /// </summary>
        Default,

        /// <summary>
        /// Use chat keyboard.
        /// </summary>
        Chat,

        /// <summary>
        /// Use e-mail keyboard.
        /// </summary>
        Email,

        /// <summary>
        /// Use numeric keyboard.
        /// </summary>
        Numeric,

        /// <summary>
        /// Use plain keyboard.
        /// </summary>
        Plain,

        /// <summary>
        /// Use telephone keyboard.
        /// </summary>
        Telephone,

        /// <summary>
        /// Use text keyboard.
        /// </summary>
        Text,

        /// <summary>
        /// Use url keyboard.
        /// </summary>
        Url,
    }
}