using System.Collections.Generic;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a status bar control.
    /// </summary>
    public class StatusBar : Control
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
        public Collection<StatusBarPanel> Panels { get; } = new ();

        /// <summary>
        /// Gets a <see cref="StatusBarHandler"/> associated with this class.
        /// </summary>
        public new StatusBarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (StatusBarHandler)base.Handler;
            }
        }

        internal override bool IsDummy => true;

        /// <summary>
        /// Gets or sets a value indicating whether a sizing grip is displayed in the lower-right corner of the control.
        /// </summary>
        public bool SizingGripVisible { get => Handler.SizingGripVisible; set => Handler.SizingGripVisible = value; }

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements => Panels;

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection => Panels;

        private void Panels_ItemInserted(object? sender, CollectionChangeEventArgs<StatusBarPanel> e)
        {
            // This is required for data binding inheritance.
            // Commented out for same reason as in Toolbar
            // Children.Add(e.Item);
        }

        private void Panels_ItemRemoved(
            object? sender,
            CollectionChangeEventArgs<StatusBarPanel> e)
        {
            // Commented out as Children.Add(e.Item) was commented
            // Children.Remove(e.Item);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateStatusBarHandler(this);
        }
    }
}