using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class CustomTextBox
    {
        /// <summary>
        /// Returns the contents of a given line in the text control, not
        /// including any trailing newline character(s).
        /// </summary>
        /// <param name="lineNo">Line number (starting from zero).</param>
        /// <returns>The contents of the line.</returns>
        public abstract string GetLineText(long lineNo);

        /// <summary>
        /// Returns the number of lines in the text control buffer.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The returned number is the number of logical lines, i.e. just the count of
        /// the number of newline characters in the control + 1, for GTK
        /// and OSX/Cocoa ports while it is the number of physical lines,
        /// i.e. the count of
        /// lines actually shown in the control, in MSW and OSX/Carbon. Because of
        /// this discrepancy, it is not recommended to use this function.
        /// </remarks>
        /// <remarks>
        /// Note that even empty text controls have one line (where the
        /// insertion point is), so this function never returns 0.
        /// </remarks>
        public abstract int GetNumberOfLines();
    }
}
