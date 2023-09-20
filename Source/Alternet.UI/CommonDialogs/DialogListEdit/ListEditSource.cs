using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Base abstract class which implements <see cref="IListEditSource"/>.
    /// </summary>
    public abstract class ListEditSource : IListEditSource
    {
        private object? instance;
        private PropertyInfo? propInfo;

        /// <inheritdoc/>
        public virtual bool AllowSubItems => false;

        /// <inheritdoc/>
        public object? Instance
        {
            get => instance;
            set
            {
                if (instance == value)
                    return;
                instance = value;
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
        /// Used as event handler.
        /// </summary>
        /// <param name="sender">Must implement <see cref="IPropInfoAndInstance"/>.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// Calls <see cref="DialogFactory.EditPropertyWithListEditor(object,string)"/> for
        /// the <paramref name="sender"/>,
        /// if it implements <see cref="IPropInfoAndInstance"/> interface.
        /// </remarks>
        public static void EditWithListEdit(object? sender, EventArgs e)
        {
            if (sender is not IPropInfoAndInstance prop)
                return;
            var instance = prop.Instance;
            var propInfo = prop.PropInfo;

            DialogFactory.EditPropertyWithListEditor(instance, propInfo);
        }

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
        public virtual string? GetItemTitle(object item) => item?.ToString();

        /// <inheritdoc/>
        public abstract void ApplyData(IEnumerableTree tree);

        /// <inheritdoc/>
        public abstract object CloneItem(object item);

        /// <inheritdoc/>
        public virtual IEnumerable? GetChildren(object item) => null;

        /// <inheritdoc/>
        public virtual object? GetProperties(object item) => item;

        /// <inheritdoc/>
        public virtual int? GetItemImageIndex(object item) => null;

        /// <inheritdoc/>
        public virtual object? CreateNewItem() => null;

        /// <summary>
        /// Applies <see cref="IEnumerableTree"/> data to the property specified
        /// in <see cref="Instance"/> and <see cref="PropInfo"/>.
        /// </summary>
        /// <typeparam name="T">Type of the item in the result collection.</typeparam>
        /// <param name="tree">Data to apply.</param>
        protected void ApplyDataAsArray<T>(IEnumerableTree tree)
        {
            List<T> value = new();
            value.AddRange(EnumerableUtils.GetItems<T>(tree));
            T[] asArray = value.ToArray();
            PropInfo?.SetValue(Instance, asArray);
        }
    }
}
