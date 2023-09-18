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
            propRegistry.NewItemParams.ButtonClick += EditWithListEdit;
            return propRegistry;
        }

        private static void EditWithListEdit(object? sender, EventArgs e)
        {
            if (sender is not IPropertyGridItem prop)
                return;
            var instance = prop.Instance;
            var propInfo = prop.PropInfo;

            UIDialogListEdit.EditProperty(instance, propInfo);
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

            RegisterListEditSource(
                typeof(ListViewItem),
                nameof(ListViewItem.Cells),
                typeof(ListEditSourceListViewCell));

            RegisterListEditSource(
                typeof(ListBox),
                nameof(ListBox.Items),
                typeof(ListEditSourceListBox));
            
            RegisterListEditSource(
                typeof(CheckListBox),
                nameof(CheckListBox.Items),
                typeof(ListEditSourceListBox));
            
            RegisterListEditSource(
                typeof(ComboBox),
                nameof(ComboBox.Items),
                typeof(ListEditSourceListBox));

            RegisterListEditSource(
                typeof(PropertyGridAdapterBrush),
                nameof(PropertyGridAdapterBrush.GradientStops),
                typeof(ListEditSourceGradientStops));
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

        public bool AllowApplyData => true;

        public virtual bool AllowAdd => true;

        public virtual bool AllowDelete => true;

        public static IEnumerable<T> GetItems<T>(IEnumerableTree tree)
        {
            foreach (var srcItem in tree)
            {
                if (tree.GetData(srcItem) is T data)
                    yield return data;
            }
        }

        public static IEnumerable<T> GetChildren<T>(IEnumerableTree tree, object item)
        {
            var children = tree.GetChildren(item);
            if (children != null)
                foreach (var srcItem in children)
                {
                    if (tree.GetData(srcItem) is T data)
                        yield return data;
                }
        }

        public static void ForEachItem(IEnumerableTree tree, Action<object> func)
        {
            void Fn(IEnumerable parent)
            {
                foreach (var item in parent)
                {
                    func(item);
                    var childs = tree.GetChildren(item);
                    if (childs != null)
                        Fn(childs);
                }
            }

            Fn(tree);
        }

        public abstract void ApplyData(IEnumerableTree tree);

        public abstract object CloneItem(object item);

        public virtual IEnumerable? GetChildren(object item) => null;

        public virtual object? GetProperties(object item) => item;

        public virtual ImageList? ImageList => null;

        public virtual int? GetItemImageIndex(object item) => null;

        public virtual object? CreateNewItem() => null;
    }
}
