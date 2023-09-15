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
    public abstract class ListEditSource : IListEditSource
    {
        private object? instance;
        private PropertyInfo? propInfo;

        public virtual bool AllowSubItems => false;

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

        public static IPropertyGridPropInfoRegistry RegisterListEditSource(
            Type type,
            string propName,
            Type editType)
        {
            var typeRegistry = PropertyGrid.GetTypeRegistry(type);
            var propRegistry = typeRegistry.GetPropRegistry(propName);
            propRegistry.ListEditSourceType = editType;
            return propRegistry;
        }

        public static void RegisterCreateFuncs()
        {
            RegisterListEditSource(
                typeof(ListView),
                nameof(ListView.Items),
                typeof(ListEditSourceListViewItem));

            RegisterListEditSource(
                typeof(ListView),
                nameof(ListView.Columns),
                typeof(ListEditSourceListViewColumn));

            RegisterListEditSource(
                typeof(TreeView),
                nameof(TreeView.Items),
                typeof(ListEditSourceTreeViewItem));
        }

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

        public virtual string? GetItemTitle(object item) => item?.ToString();

        public abstract IEnumerable? RootItems
        {
            get;
        }

        public virtual IEnumerable? GetChildren(object item) => null;

        public virtual object? GetProperties(object item) => item;

        public virtual ImageList? ImageList => null;

        public virtual int? GetItemImageIndex(object item) => null;

        public virtual object? CreateNewItem() => null;

        public virtual bool AllowAdd => true;
        public virtual bool AllowDelete => true;
    }
}
