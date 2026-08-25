using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// This is the <see cref="GenericBorder"/> descendant which is initialized with the hidden border.
    /// </summary>
    public partial class HiddenGenericBorder : GenericBorder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HiddenBorder"/> class.
        /// </summary>
        public HiddenGenericBorder()
        {
        }

        /// <inheritdoc/>
        protected override bool GetDefaultHasBorder() => false;
    }
}
