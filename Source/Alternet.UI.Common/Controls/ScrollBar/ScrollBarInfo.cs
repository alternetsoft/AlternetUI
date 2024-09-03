using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains information about the scrollbar position and range.
    /// </summary>
    public struct ScrollBarInfo
    {
        /// <summary>
        /// Gets default value for the <see cref="ScrollBarInfo"/> structure.
        /// </summary>
        public static readonly ScrollBarInfo Default = new(immutable: true);

        private bool immutable;
        private HiddenOrVisible visibility;
        private int position;
        private int thumbSize;
        private int range;
        private int pageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBarInfo"/> struct
        /// with the specified value of the immutable state.
        /// </summary>
        public ScrollBarInfo(bool immutable)
        {
            this.immutable = immutable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBarInfo"/> struct.
        /// </summary>
        public ScrollBarInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBarInfo"/> struct.
        /// </summary>
        /// <param name="position">The position of the scrollbar in scroll units.</param>
        /// <param name="thumbSize">The size of the thumb, or visible portion of the
        /// scrollbar, in scroll units.</param>
        /// <param name="range">The maximum position of the scrollbar.</param>
        /// <param name="pageSize">The size of the page size in scroll units. This is the
        /// number of units the scrollbar will scroll when it is paged up or down.
        /// Often it is the same as the thumb size.</param>
        public ScrollBarInfo(
            int position,
            int thumbSize,
            int range,
            int pageSize)
        {
            this.position = position;
            this.thumbSize = thumbSize;
            this.range = range;
            this.pageSize = pageSize;
        }

        /// <summary>
        /// Gets whether scrollbar is visible.
        /// </summary>
        public readonly bool IsVisible
        {
            get
            {
                return Visibility != HiddenOrVisible.Hidden;
            }
        }

        /// <summary>
        /// Gets or sets scrollbar visibility.
        /// </summary>
        public HiddenOrVisible Visibility
        {
            readonly get
            {
                return visibility;
            }

            set
            {
                if (immutable)
                    return;
                visibility = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the scrollbar in scroll units.
        /// </summary>
        public int Position
        {
            readonly get
            {
                return position;
            }

            set
            {
                if (immutable)
                    return;
                position = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the thumb, or visible portion of the
        /// scrollbar, in scroll units.
        /// </summary>
        public int ThumbSize
        {
            readonly get
            {
                return thumbSize;
            }

            set
            {
                if (immutable)
                    return;
                thumbSize = value;
            }
        }

        /// <summary>
        /// Returns <see cref="ThumbSize"/> if it's positive; otherwise returns <see cref="PageSize"/>.
        /// Returned value is less than <see cref="Range"/>.
        /// </summary>
        public int SafeThumbSize
        {
            get
            {
                int result = 0;

                if (ThumbSize > 0)
                    result = ThumbSize;
                else
                if (PageSize > 0)
                    result = PageSize;
                result = Math.Min(result, Range);
                return result;
            }
        }

        /// <summary>
        /// Gets or sets the maximum position of the scrollbar.
        /// </summary>
        public int Range
        {
            readonly get
            {
                return range;
            }

            set
            {
                if (immutable)
                    return;
                range = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the page size in scroll units. This is the
        /// number of units the scrollbar will scroll when it is paged up or down.
        /// Often it is the same as the thumb size.
        /// </summary>
        public int PageSize
        {
            readonly get
            {
                return pageSize;
            }

            set
            {
                if (immutable)
                    return;
                pageSize = value;
            }
        }

        private static ScrollBarInfo WithAutoFlag { get; } = CreateImmutable(HiddenOrVisible.Auto);

        private static ScrollBarInfo WithHiddenFlag { get; } = CreateImmutable(HiddenOrVisible.Hidden);

        private static ScrollBarInfo WithVisibleFlag { get; } = CreateImmutable(HiddenOrVisible.Visible);

        /// <summary>
        /// Implicit conversion operator from <see cref="HiddenOrVisible"/>
        /// to <see cref="ScrollBarInfo"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public static implicit operator ScrollBarInfo(HiddenOrVisible value)
        {
            switch (value)
            {
                default:
                case HiddenOrVisible.Auto:
                    return WithAutoFlag;
                case HiddenOrVisible.Hidden:
                    return WithHiddenFlag;
                case HiddenOrVisible.Visible:
                    return WithVisibleFlag;
            }
        }

        /// <inheritdoc cref="ImmutableObject.SetImmutable"/>
        public void SetImmutable()
        {
            immutable = true;
        }

        private static ScrollBarInfo CreateImmutable(HiddenOrVisible value)
        {
            ScrollBarInfo result = new();
            result.Visibility = value;
            result.SetImmutable();
            return result;
        }
    }
}
