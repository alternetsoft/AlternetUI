using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements multiline text editor.
    /// </summary>
    public class MultilineTextBox : TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultilineTextBox"/> class.
        /// </summary>
        public MultilineTextBox()
        {
            ProcessTab = true;
            base.Multiline = true;
        }

        /// <summary>
        /// Always returns <c>false</c>.
        /// </summary>
        public override bool CanUserPaint => false;

        /// <summary>
        /// Always returns <c>true</c>.
        /// </summary>
        public override bool Multiline
        {
            get => true;

            set
            {
            }
        }
    }
}
