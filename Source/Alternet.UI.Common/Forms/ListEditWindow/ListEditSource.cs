using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Base abstract class which implements <see cref="IListEditSource"/>.
    /// </summary>
    public abstract class ListEditSource : IListEditSource
    {
        private WeakReferenceValue<object> instance;
        private PropertyInfo? propInfo;

        /// <inheritdoc/>
        public virtual bool AllowSubItems => false;

        /// <inheritdoc/>
        public object? Instance
        {
            get => instance.Value;

            set
            {
                if (instance.Value == value)
                    return;
                instance.Value = value;
            }
        }

        /// <inheritdoc/>
        public PropertyInfo? PropInfo
        {
            get => propInfo;
            set
            {
                if (propInfo == value)
                    return;
                propInfo = value;
            }
        }

        /// <inheritdoc/>
        public virtual IEnumerable? RootItems
        {
            get
            {
                if (Instance is null)
                    return Array.Empty<object>();

                var value = PropInfo?.GetValue(Instance);
                var result = value as IEnumerable;
                return result;
            }
        }

        /// <inheritdoc/>
        public virtual ImageList? ImageList => null;

        /// <inheritdoc/>
        public bool AllowApplyData => true;

        /// <inheritdoc/>
        public virtual bool AllowAdd => true;

        /// <inheritdoc/>
        public virtual bool AllowDelete => true;

        /// <summary>
        /// Creates <see cref="IListEditSource"/> provider using collection editors
        /// registered with <see cref="PropertyGrid.RegisterCollectionEditor"/>.
        /// </summary>
        /// <param name="instance">Object which contains the collection.</param>
        /// <param name="propInfo">Property information.</param>
        /// <returns><see cref="IListEditSource"/> provider which is capable of editing
        /// specified collecion property; <c>null</c> if no suitable providers were found.</returns>
        public static IListEditSource? CreateEditSource(object? instance, PropertyInfo? propInfo)
        {
            if (propInfo == null)
                return null;
            var editType = PropertyGrid.GetListEditSourceType(instance?.GetType(), propInfo);
            if (editType == null)
                return null;

            var value = propInfo?.GetValue(instance);
            if (value is not ICollection)
                return null;

            var result = (IListEditSource?)Activator.CreateInstance(editType);
            if (result == null)
                return null;
            result.Instance = instance;
            result.PropInfo = propInfo;
            return result;
        }

        /// <inheritdoc/>
        public virtual string? GetItemTitle(object item)
        {
            return item?.ToString();
        }

        /// <inheritdoc/>
        public abstract void ApplyData(IEnumerableTree tree);

        /// <inheritdoc/>
        public abstract object CloneItem(object item);

        /// <inheritdoc/>
        public virtual IEnumerable? GetChildren(object item)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual object? GetProperties(object item)
        {
            return item;
        }

        /// <inheritdoc/>
        public virtual int? GetItemImageIndex(object item)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual object? CreateNewItem()
        {
            return null;
        }

        /// <summary>
        /// Applies <see cref="IEnumerableTree"/> data to the property specified
        /// in <see cref="Instance"/> and <see cref="PropInfo"/>.
        /// </summary>
        /// <typeparam name="T">Type of the item in the result collection.</typeparam>
        /// <param name="tree">Data to apply.</param>
        protected void ApplyDataAsArray<T>(IEnumerableTree tree)
        {
            if (Instance is null)
                return;

            List<T> value = new();
            value.AddRange(EnumerableUtils.GetItems<T>(tree));
            T[] asArray = value.ToArray();

            PropInfo?.SetValue(Instance, asArray);
        }
    }
}
