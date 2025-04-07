using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a base class for content views in the library.
    /// </summary>
    public partial class BaseContentView : ContentView, UI.IRaiseSystemColorsChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseContentView"/> class.
        /// </summary>
        public BaseContentView()
        {
        }

        /// <summary>
        /// Gets the parent page of the current view.
        /// </summary>
        public virtual Page? ParentPage
        {
            get
            {
                return Alternet.UI.MauiUtils.GetPage(this);
            }
        }

        /// <inheritdoc/>
        public virtual void RaiseSystemColorsChanged()
        {
        }

        /// <summary>
        /// Marks object as required.
        /// </summary>
        public void Required()
        {
        }
    }
}
