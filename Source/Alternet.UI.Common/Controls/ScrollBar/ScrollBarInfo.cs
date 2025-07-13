using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains information about the scrollbar position and range.
    /// </summary>
    public struct ScrollBarInfo : IEquatable<ScrollBarInfo>
    {
        /// <summary>
        /// Gets default value for the <see cref="ScrollBarInfo"/> structure.
        /// </summary>
        public static readonly ScrollBarInfo Default = new(immutable: true);

        private bool immutable;
        private HiddenOrVisible visibility;
        private int position;
        private int range;
        private int pageSize = -1;

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
        /// Initializes a new instance of the <see cref="ScrollBarInfo"/> struct
        /// with the specified visibility.
        /// </summary>
        public ScrollBarInfo(HiddenOrVisible visibility)
        {
            this.visibility = visibility;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBarInfo"/> struct
        /// with the specified position, range and page size properties.
        /// </summary>
        /// <param name="position">The position of the scrollbar in scroll units.</param>
        /// <param name="range">The maximum position of the scrollbar.</param>
        /// <param name="pageSize">The size of the page size in scroll units. This is the
        /// number of units the scrollbar will scroll when it is paged up or down.
        /// Often it is the same as the thumb size.</param>
        public ScrollBarInfo(
            int position,
            int range,
            int pageSize)
        {
            this.position = position;
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
                if (Visibility == HiddenOrVisible.Hidden)
                    return false;

                if (Visibility == HiddenOrVisible.Auto)
                {
                    if (Range <= 0 || PageSize < 0 || PageSize >= Range)
                        return false;
                }

                return true;
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
                value = Math.Max(value, 0);
                position = value;
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
                value = Math.Max(value, 0);
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
                value = Math.Max(value, -1);
                pageSize = value;
            }
        }

        /// <summary>
        /// Compares two <see cref='ScrollBarInfo'/> objects.
        /// The result specifies whether
        /// the values of the properties of the two
        /// <see cref='ScrollBarInfo'/> objects are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ScrollBarInfo left, ScrollBarInfo right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref='ScrollBarInfo'/> objects. The result specifies whether
        /// the values of the properties of the two
        /// <see cref='ScrollBarInfo'/> objects are unequal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ScrollBarInfo left, ScrollBarInfo right) => !(left == right);

        /// <summary>
        /// Returns a copy of this object with the new page size and range values.
        /// </summary>
        /// <param name="pageSize">New page size value.</param>
        /// <param name="range">New range value.</param>
        /// <returns></returns>
        public readonly ScrollBarInfo WithPageSizeAndRange(int pageSize, int range)
        {
            var result = Clone();
            result.PageSize = pageSize;
            result.Range = range;
            return result;
        }

        /// <summary>
        /// Returns a copy of this object with the new position value.
        /// </summary>
        /// <param name="position">New position value.</param>
        /// <returns></returns>
        public readonly ScrollBarInfo WithPosition(int position)
        {
            var result = Clone();
            result.Position = position;
            return result;
        }

        /// <summary>
        /// Returns a copy of this object with the new visibility value.
        /// </summary>
        /// <param name="visibility">New visibility value.</param>
        /// <returns></returns>
        public readonly ScrollBarInfo WithVisibility(HiddenOrVisible visibility)
        {
            var result = Clone();
            result.Visibility = visibility;
            return result;
        }

        /// <summary>
        /// Assigns all properties from the specified object to this object.
        /// </summary>
        /// <param name="assignFrom"></param>
        public void Assign(ScrollBarInfo assignFrom)
        {
            if (immutable)
                return;

            visibility = assignFrom.visibility;
            position = assignFrom.position;
            range = assignFrom.range;
            pageSize = assignFrom.pageSize;
        }

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        public readonly ScrollBarInfo Clone()
        {
            ScrollBarInfo result = new();
            result.Assign(this);
            return result;
        }

        /// <inheritdoc/>
        public readonly bool Equals(ScrollBarInfo other)
        {
            var result =
                visibility == other.visibility &&
                position == other.position &&
                range == other.range &&
                pageSize == other.pageSize;
            return result;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the
        /// current object; otherwise, <c>false</c>.</returns>
        public readonly override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ScrollBarInfo other && Equals(other);

        /// <inheritdoc cref="ImmutableObject.SetImmutable()"/>
        public void SetImmutable()
        {
            immutable = true;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public readonly override int GetHashCode()
        {
            return (visibility, position, range, pageSize).GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public readonly override string ToString()
        {
            string[] names =
            {
                nameof(Position),
                nameof(Range),
                nameof(PageSize),
                nameof(Visibility),
            };

            object[] values = { Position, Range, PageSize, Visibility };

            return StringUtils.ToString(names, values);
        }

        internal static ScrollBarInfo CreateImmutable(HiddenOrVisible value)
        {
            ScrollBarInfo result = new();
            result.Visibility = value;
            result.SetImmutable();
            return result;
        }
    }
}
