using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS

using Microsoft.UI.Input;

namespace Alternet.UI
{
    /// <summary>
    /// Adds additional functionality
    /// to the <see cref="SkiaSharp.Views.Windows.SKXamlCanvas"/> control.
    /// </summary>
    public partial class PlatformView : SkiaSharp.Views.Windows.SKXamlCanvas
    {
        /// <summary>
        /// Gets or sets the cursor that displays when the pointer is over this element.
        /// Defaults to null, indicating no change to the cursor.
        /// </summary>
        public virtual InputCursor? InputCursor
        {
            get => ProtectedCursor;
            set => ProtectedCursor = value;
        }
    }
}

#endif