using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a scrollable area that can contain other visible elements.
    /// </summary>
    public class ScrollViewer : Control
    {
        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.ScrollViewer;

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateScrollViewerHandler(this);
        }
    }
}