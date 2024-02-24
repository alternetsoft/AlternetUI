using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a single tab page in a <see cref="TabControl"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="TabPage"/> controls represent the tabbed pages in a
    /// <see cref="TabControl"/> control.
    /// The tabs in a <see cref="TabControl"/> are part of the <see cref="TabControl"/> but
    /// not parts of the individual <see cref="TabPage"/> controls.
    /// </remarks>
    [ControlCategory("Hidden")]
    public partial class TabPage : Control
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TabPage"/> class.
        /// </summary>
        public TabPage()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TabPage"/> class with the specified title.
        /// </summary>
        public TabPage(string? title)
        {
            Title = title ?? string.Empty;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.TabPage;

        /// <summary>
        /// Gets the zero-based index of the page within the <see cref="TabControl"/> control,
        /// or <see langword="null"/> if the item is not associated with
        /// a <see cref="TabControl"/> control.
        /// </summary>
        public int? Index { get; internal set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Title))
                return base.ToString() ?? nameof(TabPage);
            else
                return Title;
        }
    }
}