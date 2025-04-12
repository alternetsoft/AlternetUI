using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// Gets a value indicating whether the current theme is dark.
        /// </summary>
        public static bool IsDark
        {
            get
            {
                return Alternet.UI.SystemSettings.AppearanceIsDark;
            }
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

        /// <summary>
        /// Handles the event when the size of the parent element changes.
        /// </summary>
        /// <param name="sender">The source of the event, typically the parent element.</param>
        /// <param name="e">The event data containing information about the size change.</param>
        protected virtual void OnParentSizeChanged(object? sender, EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnParentChanging(ParentChangingEventArgs args)
        {
            base.OnParentChanging(args);

            var oldParent = args.OldParent as VisualElement;
            var newParent = args.NewParent as VisualElement;

            if (oldParent is not null)
            {
                oldParent.SizeChanged -= OnParentSizeChanged;
            }

            if (newParent is not null)
            {
                newParent.SizeChanged += OnParentSizeChanged;
            }
        }
    }
}
