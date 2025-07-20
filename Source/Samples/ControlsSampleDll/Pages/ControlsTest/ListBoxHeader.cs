using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class ListBoxHeader : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxHeader"/> class.
        /// </summary>
        public ListBoxHeader()
        {
            Layout = LayoutStyle.Dock;
            LayoutFlags = LayoutFlags.IterateBackward;
        }
    }
}
