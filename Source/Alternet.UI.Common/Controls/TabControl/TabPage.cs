using System;

using Alternet.Base.Collections;

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
    /// <remarks>
    /// You can add any control descendant to the <see cref="TabControl"/>, not only
    /// <see cref="TabPage"/>.
    /// </remarks>
    [ControlCategory("Hidden")]
    public partial class TabPage : ContainerControl
    {
        private int? index;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabPage"/> class.
        /// </summary>
        /// <param name="title">Page title.</param>
        /// <param name="parent">Parent of the control.</param>
        public TabPage(Control parent, string? title = null)
            : this(title)
        {
            if (parent is TabControl tabControl)
                tabControl.Add(this);
            else
                Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TabPage"/> class.
        /// </summary>
        public TabPage()
        {
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TabPage"/> class with the specified title.
        /// </summary>
        /// <param name="title">Page title.</param>
        public TabPage(string? title)
            : this()
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
        public int? Index
        {
            get
            {
                if (index is not null)
                    return index;
                if (Parent?.Parent is not TabControl tabControl)
                    return null;
                var result = tabControl.GetTabIndex(this);
                return result;
            }

            internal set
            {
                index = value;
            }
        }

        /// <summary>
        /// Updates tabpage indexes starting from the specified index.
        /// </summary>
        /// <param name="controls">List of <see cref="TabPage"/> controls.</param>
        /// <param name="startIndex">Starting index.</param>
        public static void UpdatePageIndexes(Collection<TabPage> controls, int startIndex)
        {
            for (int i = startIndex; i < controls.Count; i++)
                controls[i].Index = i;
        }

        /// <summary>
        /// Do not use this method.
        /// </summary>
        public static void SetPageIndexInternal(TabPage tab, int? index)
        {
            tab.Index = index;
        }

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