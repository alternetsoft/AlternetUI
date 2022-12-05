using Alternet.Base.Collections;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a status bar control.
    /// </summary>
    public class StatusBar : Control
    {
        /// <inheritdoc/>
        public new StatusBarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (StatusBarHandler)base.Handler;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class.
        /// </summary>
        public StatusBar()
        {
            Panels.ItemInserted += Panels_ItemInserted;
            Panels.ItemRemoved += Panels_ItemRemoved;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a sizing grip is displayed in the lower-right corner of the control.
        /// </summary>
        public bool SizingGripVisible { get => Handler.SizingGripVisible; set => Handler.SizingGripVisible = value; }

        private void Panels_ItemInserted(object? sender, CollectionChangeEventArgs<StatusBarPanel> e)
        {
            // This is required for data binding inheritance.
            Children.Add(e.Item);
        }

        private void Panels_ItemRemoved(object? sender, CollectionChangeEventArgs<StatusBarPanel> e)
        {
            Children.Remove(e.Item);
        }

        /// <summary>
        /// Gets a collection of <see cref="StatusBarPanel"/> objects associated with the status bar.
        /// </summary>
        [Content]
        public Collection<StatusBarPanel> Panels { get; } = new Collection<StatusBarPanel>();

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements => Panels;

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection => Panels;
    }
}