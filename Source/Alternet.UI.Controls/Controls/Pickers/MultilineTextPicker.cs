using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to edit multiline text in a user-friendly manner.
    /// </summary>
    [ControlCategory(KnownControlCategory.Editors)]
    public partial class MultilineTextPicker : TextPicker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultilineTextPicker"/> class.
        /// </summary>
        public MultilineTextPicker()
        {
            MinimumSize = (120, 96);
            SuggestedHeight = 96;
            Multiline = true;
            WantTabInEditor = true;
        }
    }
}
