using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Descendant of the <see cref="ListControl{T}"/> with object items.
    /// </summary>
    public abstract class ListControl : ListControl<object>
    {
        /// <summary>
        /// Adds enum values to the items collection.
        /// </summary>
        /// <typeparam name="T">Type of the enum which values are added.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void AddEnumValues<T>()
            where T : Enum
        {
            AddEnumValues(typeof(T));
        }

        /// <summary>
        /// Adds enum values to items collection.
        /// </summary>
        /// <typeparam name="T">Type of the enum which values are added.</typeparam>
        /// <param name="selectValue">New selected item value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void AddEnumValues<T>(T selectValue)
            where T : Enum
        {
            AddEnumValues(typeof(T), selectValue);
        }

        /// <summary>
        /// Adds enum values to the items collection.
        /// </summary>
        /// <param name="type">Type of the enum which values are added.</param>
        /// <param name="selectValue">New selected item value.</param>
        public virtual void AddEnumValues(Type type, object? selectValue = default)
        {
            foreach (var item in Enum.GetValues(type))
                Items.Add(item);
            if (selectValue is not null)
                SelectedItem = selectValue;
        }

        /// <summary>
        /// Adds <paramref name="text"/> with <paramref name="data"/> to the end of
        /// the items collection.
        /// </summary>
        /// <param name="text">Item text (title).</param>
        /// <param name="data">Item data.</param>
        /// <remarks>
        /// This method creates <see cref="ListControlItem"/>, assigns its properties with
        /// <paramref name="text"/> and <paramref name="data"/>. Created object is added to
        /// the items collection.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int Add(string text, object? data)
        {
            return Add(new ListControlItem(text, data));
        }

        /// <summary>
        /// Adds <paramref name="text"/> with <paramref name="action"/> to the end of
        /// the items collection.
        /// </summary>
        /// <param name="text">Item text (title).</param>
        /// <param name="action">Action associated with the item.</param>
        /// <remarks>
        /// This method creates <see cref="ListControlItem"/>, assigns its properties with
        /// <paramref name="text"/> and <paramref name="action"/>. Created object is added to
        /// the collection.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int Add(string text, Action action)
        {
            return Add(new ListControlItem(text, action));
        }
    }
}
