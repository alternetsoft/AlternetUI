using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates virtual keyboard option flags that controls capitalization, spellcheck,
    /// suggestion and other behavior.
    /// This enumeration supports a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum VirtualKeyboardFlags
    {
        /// <summary>
        /// Indicates that no automatical capitalization or spellcheck is done.
        /// </summary>
        None = 0,

        /// <summary>
        /// The first letters of the first words of each sentence
        /// are automatically capitalized.
        /// </summary>
        CapitalizeSentence = 1,

        /// <summary>
        /// Performs spellcheck on text that the user enters.
        /// </summary>
        Spellcheck = 2,

        /// <summary>
        /// Offers suggested word completions on text that the user enters.
        /// </summary>
        Suggestions = 4,

        /// <summary>
        /// The first letter of each word is automatically capitalized.
        /// </summary>
        CapitalizeWord = 8,

        /// <summary>
        /// Every character is automatically capitalized.
        /// </summary>
        CapitalizeCharacter = 0x10,

        /// <summary>
        /// No automatical capitalization is done.
        /// </summary>
        CapitalizeNone = 0x20,

        /// <summary>
        /// Capitalizes the first letter of the first words of sentences, performs spellcheck,
        /// and offers suggested word completions on text that the user enters.
        /// </summary>
        All = -1,
    }
}