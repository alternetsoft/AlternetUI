using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// This is the <see cref="Border"/> descendant which is initialized with the hidden border.
    /// </summary>
    public class HiddenBorder : Border
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HiddenBorder"/> class.
        /// </summary>
        public HiddenBorder()
        {
        }

        /// <inheritdoc/>
        protected override bool GetDefaultHasBorder() => false;
    }
}
