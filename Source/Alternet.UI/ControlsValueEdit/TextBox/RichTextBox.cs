using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements rich text editor functionality.
    /// </summary>
    public class RichTextBox : TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBox"/> class.
        /// </summary>
        public RichTextBox()
        {
            base.Multiline = true;
            base.IsRichEdit = true;
        }

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

        /// <summary>
        /// Always returns <c>true</c>.
        /// </summary>
        public override bool IsRichEdit
        {
            get => true;

            set
            {
            }
        }
    }
}
