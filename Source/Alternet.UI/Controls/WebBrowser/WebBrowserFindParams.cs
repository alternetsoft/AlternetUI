using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    //-------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /*
wxWebViewFindFlags
enum wxWebViewFindFlags
Find flags used when searching for text on page.

Enumerator
wxWEBVIEW_FIND_WRAP 	
Causes the search to restart when end or beginning reached.

wxWEBVIEW_FIND_ENTIRE_WORD 	
Matches an entire word when searching.

wxWEBVIEW_FIND_MATCH_CASE 	
Match case, i.e.

case sensitive searching

wxWEBVIEW_FIND_HIGHLIGHT_RESULT 	
Highlights the search results.

wxWEBVIEW_FIND_BACKWARDS 	
Searches for phrase in backward direction.

wxWEBVIEW_FIND_DEFAULT 	
The default flag, which is simple searching.
     */
    public class WebBrowserFindParams
    {
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        public bool Wrap = false;
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        public bool EntireWord = false;
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        public bool MatchCase = false;
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        public bool HighlightResult=true;
        /// <summary>
        /// 
        /// </summary>
        /*
         
        */
        public bool Backwards=false;
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /*

         */
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
        //-------------------------------------------------
    }
    //-------------------------------------------------
}
