using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    ///     Find flags used when searching for text on web page.
    /// </summary>
    public class WebBrowserFindParams
    {
        /// <summary>
        ///     Causes the search to restart when end or beginning reached.
        /// </summary>
        public bool Wrap { get; set; } = true;

        /// <summary>
        ///     Matches an entire word when searching.
        /// </summary>
        public bool EntireWord { get; set; } = false;

        /// <summary>
        ///     Match case, i.e. case sensitive searching.
        /// </summary>
        public bool MatchCase { get; set; } = false;

        /// <summary>
        ///     Highlights the search results.
        /// </summary>
        public bool HighlightResult { get; set; } = true;

        /// <summary>
        ///     Searches for phrase in backward direction.
        /// </summary>
        public bool Backwards { get; set; } = false;

        internal int ToWebViewParams()
        {
            const int FindWrap = 0x0001;
            const int FindEntireWord = 0x0002;
            const int FindMatchCase = 0x0004;
            const int FindHighlightResult = 0x0008;
            const int FindBackwards = 0x0010;
            const int FindDefault = 0;

            int result = FindDefault;

            if (Wrap) result |= FindWrap;
            if (EntireWord) result |= FindEntireWord;
            if (MatchCase) result |= FindMatchCase;
            if (HighlightResult) result |= FindHighlightResult;
            if (Backwards) result |= FindBackwards;
            return result;
        }
    }
}
