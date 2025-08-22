using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a separator item within a toolbar, used to visually separate groups of toolbar items.
    /// </summary>
    /// <remarks>The <see cref="ToolBarSeparatorItem"/> can be oriented either
    /// vertically or horizontally,
    /// depending on the value of the <see cref="IsVertical"/> property.
    /// The orientation affects the appearance and layout of the separator.</remarks>
    public class ToolBarSeparatorItem : Border
    {
        private bool isVertical;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBarSeparatorItem"/> class.
        /// </summary>
        public ToolBarSeparatorItem()
        {
            VerticalChanged();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the orientation is vertical.
        /// </summary>
        /// <remarks>Changing this property triggers the <c>VerticalChanged</c> method.</remarks>
        public virtual bool IsVertical
        {
            get
            {
                return isVertical;
            }

            set
            {
                if (isVertical == value)
                    return;
                isVertical = value;
                VerticalChanged();
            }
        }

        /// <summary>
        /// Updates the layout and appearance of the separator based on its orientation.
        /// </summary>
        /// <remarks>This method adjusts the border width, suggested size, and margin
        /// of the separator  depending on whether it is oriented vertically or horizontally.
        /// Subclasses can override this method to provide custom behavior when
        /// the orientation changes.</remarks>
        protected virtual void VerticalChanged()
        {
            if (isVertical)
            {
                BorderWidth = (0, ToolBar.DefaultSeparatorWidth, 0, 0);
                SuggestedSize = (Coord.NaN, ToolBar.DefaultSeparatorWidth);
                Margin = ToolBar.DefaultSeparatorMargin;
            }
            else
            {
                BorderWidth = (ToolBar.DefaultSeparatorWidth, 0, 0, 0);
                SuggestedSize = (ToolBar.DefaultSeparatorWidth, Coord.NaN);
                Margin = ToolBar.DefaultSeparatorMargin;
            }
        }

        /// <inheritdoc/>
        protected override void OnDataContextChanged(object? oldValue, object? newValue)
        {
            base.OnDataContextChanged(oldValue, newValue);
        }
    }
}
