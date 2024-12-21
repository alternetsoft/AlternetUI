using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to get and set indexed values as fast as possible.
    /// </summary>
    public class IndexedValues<TIndex, TValue> : BaseObject
        where TIndex : notnull
    {
        private readonly AdvDictionary<TIndex, ILockedItem> values = new();

        /// <summary>
        /// Contains properties, methods and events which allow to work with indexed value.
        /// </summary>
        public interface ILockedItem
        {
            /// <summary>
            /// Occurs when value is changed.
            /// </summary>
            event EventHandler Changed;

            /// <summary>
            /// Gets whether <see cref="Value"/> was set at least one time.
            /// </summary>
            bool Assigned { get; }

            /// <summary>
            /// Gets or sets value.
            /// </summary>
            /// <returns></returns>
            TValue? Value { get; set; }

            /// <summary>
            /// Gets index.
            /// </summary>
            TIndex Index { get; }
        }

        /// <summary>
        /// Gets or sets value for the specified index.
        /// </summary>
        /// <param name="index">Index of value.</param>
        /// <returns></returns>
        public TValue? this[TIndex index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return GetValue(index);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                SetValue(index, value);
            }
        }

        /// <summary>
        /// Sets value for the specified index.
        /// </summary>
        /// <param name="index">Index of value.</param>
        /// <param name="value">New value to set for the specified index.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(TIndex index, TValue? value)
        {
            GetLockedItem(index).Value = value;
        }

        /// <summary>
        /// Gets value for the specified index.
        /// </summary>
        /// <param name="index">Index of value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue? GetValue(TIndex index)
        {
            return GetLockedItem(index).Value;
        }

        /// <summary>
        /// Gets locked item which allows to access index and value as fast as possible.
        /// </summary>
        /// <param name="index">Index of value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILockedItem GetLockedItem(TIndex index)
        {
            var result = values.GetOrCreate(index, () => new LockedItem(index));
            return result;
        }

        /// <summary>
        /// Gets value for the specified index. If value was not assigned,
        /// <paramref name="getDefault"/> function is used to initialize index/value pair.
        /// </summary>
        /// <param name="index">Index of value.</param>
        /// <returns></returns>
        /// <param name="getDefault">Function which returns default value for the specified index.
        /// It is called when item with the specified index is locked for the first time.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue? GetValue(TIndex index, Func<TValue?> getDefault)
        {
            return GetLockedItem(index, getDefault).Value;
        }

        /// <summary>
        /// Gets locked item initialized with the default value.
        /// </summary>
        /// <param name="index">Index of value.</param>
        /// <returns></returns>
        /// <param name="getDefault">Function which returns default value for the specified index.
        /// It is called when item with the specified index is locked for the first time.</param>
        /// <returns></returns>
        public ILockedItem GetLockedItem(TIndex index, Func<TValue?> getDefault)
        {
            var lockedItem = GetLockedItem(index);

            if (lockedItem.Assigned)
            {
                return lockedItem;
            }
            else
            {
                lockedItem.Value = getDefault();
                return lockedItem;
            }
        }

        private class LockedItem : ILockedItem
        {
            private readonly TIndex index;
            private bool assigned;

            private TValue? data;

            public LockedItem(TIndex index)
            {
                this.index = index;
            }

            public event EventHandler? Changed;

            public TIndex Index => index;

            public bool Assigned => assigned;

            public TValue? Value
            {
                get
                {
                    return data;
                }

                set
                {
                    // This call must be here.
                    assigned = true;

                    if (data is null)
                    {
                        if (value is null)
                            return;
                    }
                    else
                    {
                        if (data.Equals(value))
                            return;
                    }

                    data = value;

                    Changed?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
