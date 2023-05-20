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
    public class WebBrowserFindParams
    {
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool Wrap = false;
        /// <summary>
        /// 
        /// </summary>
        public bool EntireWord = false;
        /// <summary>
        /// 
        /// </summary>
        public bool MatchCase = false;
        /// <summary>
        /// 
        /// </summary>
        public bool HighlightResult=true;
        /// <summary>
        /// 
        /// </summary>
        public bool Backwards=false;
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public int ToWebViewParams()
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
