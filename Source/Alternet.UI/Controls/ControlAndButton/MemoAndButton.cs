using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="MultilineTextBox"/> with side buttons.
    /// </summary>
    public partial class MemoAndButton : TextBoxAndButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoAndButton"/> class.
        /// </summary>
        public MemoAndButton()
            : base(typeOfControl: typeof(MultilineTextBox))
        {
        }
    }
}
