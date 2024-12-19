using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Customization flags for text search operations in the WebBrowser control.
    /// </summary>
    public class WebBrowserFindParams : BaseObject
    {
        /// <summary>
        /// Causes the search to restart when end or beginning reached.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the search will be restarted when end or
        /// beginning reached; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Wrap { get; set; } = true;

        /// <summary>
        /// Matches an entire word when searching.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if an entire word will be matched when search
        /// is performed; otherwise, <see langword="false"/>.
        /// </returns>
        public bool EntireWord { get; set; } = false;

        /// <summary>
        /// Match case, i.e. case sensitive searching.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if case sensitive searching will be
        /// performed; otherwise, <see langword="false"/>.
        /// </returns>
        public bool MatchCase { get; set; } = false;

        /// <summary>
        /// Highlights the search results.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the search results will be highlighted;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool HighlightResult { get; set; } = true;

        /// <summary>
        /// Searches for phrase in backward direction.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if search is done in backward direction;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Backwards { get; set; } = false;

        /// <summary>
        /// Gets search parameters as int value.
        /// </summary>
        /// <returns></returns>
        public static explicit operator WebBrowserSearchFlags(WebBrowserFindParams value)
        {
            var result = WebBrowserSearchFlags.Default;

            if (value.Wrap) result |= WebBrowserSearchFlags.Wrap;
            if (value.EntireWord) result |= WebBrowserSearchFlags.EntireWord;
            if (value.MatchCase) result |= WebBrowserSearchFlags.MatchCase;
            if (value.HighlightResult) result |= WebBrowserSearchFlags.HighlightResult;
            if (value.Backwards) result |= WebBrowserSearchFlags.Backwards;
            return result;
        }
    }
}