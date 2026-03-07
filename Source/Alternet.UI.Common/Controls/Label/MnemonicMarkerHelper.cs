using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides functionality for managing and processing mnemonic markers in text, including customization of marker
    /// behavior and retrieval of mnemonic character indices.
    /// </summary>
    /// <remarks>MnemonicMarkerHelper enables control over how mnemonic markers are identified and processed
    /// within text, such as specifying the marker character, enabling or disabling marker processing, and overriding
    /// the default underlined character index. This struct is typically used in scenarios where text-based controls,
    /// like labels, require support for keyboard mnemonics. By configuring its properties, developers can tailor
    /// mnemonic handling to match application requirements or platform conventions.</remarks>
    public partial struct MnemonicMarkerHelper
    {
        /// <summary>
        /// Gets or sets underlined character index. This property is used when the text contains
        /// mnemonic markers and specifies the index of the character to be underlined as a mnemonic.
        /// </summary>
        /// <remarks>
        /// By default equals -1 and is determined by the position of the mnemonic marker in the text.
        /// Setting this property overrides the default behavior.
        /// </remarks>
        public int? MnemonicCharIndex = null;

        /// <summary>
        /// Gets or sets whether to process mnemonic markers in the text.
        /// If this property is not specified (default),
        /// <see cref="Label.DefaultMnemonicMarkerEnabled"/> is used
        /// to determine whether mnemonic markers are processed.
        /// </summary>
        public bool? MnemonicMarkerEnabled;

        /// <summary>
        /// Gets or sets the character used as the mnemonic marker.
        /// If not specified, <see cref="Label.DefaultMnemonicMarker"/> is used as the default marker.
        /// </summary>
        public char? MnemonicMarker;

        /// <summary>
        /// Initializes a new instance of the MnemonicMarkerHelper class.
        /// </summary>
        public MnemonicMarkerHelper()
        {
        }

        /// <summary>
        /// Removes mnemonic markers from a string and returns the index of the
        /// mnemonic character, if present.
        /// </summary>
        /// <param name="s">The input string containing mnemonic markers.</param>
        /// <param name="mnemonicCharIndex">
        /// The output index of the mnemonic character in the returned string.
        /// Returns -1 if no mnemonic marker is found.
        /// </param>
        /// <returns>
        /// The input string with mnemonic markers removed.
        /// Double markers are converted to a single literal character.
        /// </returns>
        /// <remarks>
        /// If <see cref="MnemonicCharIndex"/> is not null, it is assigned to
        /// <paramref name="mnemonicCharIndex"/>.
        /// In this case text is not processed and is used as is.
        /// </remarks>
        public readonly string GetWithoutMnemonicMarkers(string s, out int mnemonicCharIndex)
        {
            if (MnemonicCharIndex.HasValue)
            {
                mnemonicCharIndex = MnemonicCharIndex.Value;
                return s;
            }

            var checkMnemonic = MnemonicMarkerEnabled ?? Label.DefaultMnemonicMarkerEnabled;
            if (checkMnemonic)
            {
                var result = StringUtils.GetWithoutMnemonicMarkers(
                            s,
                            out mnemonicCharIndex,
                            MnemonicMarker ?? Label.DefaultMnemonicMarker);
                return result;
            }
            else
            {
                mnemonicCharIndex = -1;
                return s;
            }
        }
    }
}
