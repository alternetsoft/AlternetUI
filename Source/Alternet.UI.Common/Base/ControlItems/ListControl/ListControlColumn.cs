using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a column within a list control, defining its layout properties.
    /// </summary>
    /// <remarks>
    /// This class provides configuration settings such as suggested width for column rendering.
    /// It can be extended to accommodate additional visual or functional enhancements.
    /// </remarks>
    public class ListControlColumn : BaseControlItem
    {
        private static float? defaultColumnSeparatorWidth;

        private Coord suggestedWidth;
        private string? title;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlColumn"/> class.
        /// </summary>
        public ListControlColumn()
        {
        }

        /// <summary>
        /// Specifies the default width, in columns, used to separate columns in a grid or table layout.
        /// </summary>
        public static Coord DefaultColumnSeparatorWidth
        {
            get => defaultColumnSeparatorWidth ?? Splitter.DefaultWidth;
        
            set => defaultColumnSeparatorWidth = value;
        }

        /// <summary>
        /// Initializes a new instance of the ListControlColumn class with the specified column title.
        /// </summary>
        /// <param name="title">The display title for the column. Cannot be null.</param>
        public ListControlColumn(string title)
            : this()
        {
            Title = title;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the column in the header control.
        /// </summary>
        public virtual ObjectUniqueId? ColumnKey { get; set; }

        /// <summary>
        /// Gets or sets the title of the column.
        /// </summary>
        public virtual string? Title
        {
            get => title;

            set
            {
                SetProperty(ref title, value, nameof(Title), () =>
                {
                });
            }
        }

        /// <summary>
        /// Gets or sets the suggested width for the list control column.
        /// </summary>
        /// <remarks>
        /// The width recommendation is based on layout constraints and user preferences.
        /// </remarks>
        public virtual Coord SuggestedWidth
        {
            get => suggestedWidth;

            set
            {
                SetProperty(ref suggestedWidth, value, nameof(SuggestedWidth), () =>
                {
                });
            }
        }
    }
}
