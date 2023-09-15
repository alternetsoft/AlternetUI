using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a status bar control.
    /// </summary>
    public class StatusBar : NonVisualControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class.
        /// </summary>
        public StatusBar()
        {
            Panels.ItemInserted += Panels_ItemInserted;
            Panels.ItemRemoved += Panels_ItemRemoved;
        }

        /// <summary>
        /// Gets a collection of <see cref="StatusBarPanel"/> objects associated with the status bar.
        /// </summary>
        [Content]
        public Collection<StatusBarPanel> Panels { get; } = new();

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.StatusBar;

        /// <summary>
        /// Gets a <see cref="StatusBarHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public new StatusBarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (StatusBarHandler)base.Handler;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a sizing grip is displayed in the
        /// lower-right corner of the control.
        /// </summary>
        public bool SizingGripVisible
        {
            get => Handler.SizingGripVisible;
            set => Handler.SizingGripVisible = value;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements => Panels;

        internal override bool IsDummy => true;

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection => Panels;

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateStatusBarHandler(this);
        }

        private void Panels_ItemInserted(object? sender, int index, StatusBarPanel item)
        {
            // This is required for data binding inheritance.
            // Commented out for same reason as in Toolbar
            // Children.Add(e.Item);
        }

        private void Panels_ItemRemoved(object? sender, int index, StatusBarPanel item)
        {
            // Commented out as Children.Add(e.Item) was commented
            // Children.Remove(e.Item);
        }
    }
}