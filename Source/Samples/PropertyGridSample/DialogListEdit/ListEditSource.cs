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

#pragma warning disable
        public static void RegisterListEditSource(Type type, string propName, Type editType)
#pragma warning restore
        {
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
            if (propInfo == null || instance == null)
                return null;

            var value = propInfo.GetValue(instance);
            if (value is not ICollection)
                return null;

            IListEditSource Fn(IListEditSource source)
            {
                source.Instance = instance;
                source.PropInfo = propInfo;
                return source;
            }

            if(instance is ListView)
            {
                if (propInfo.Name == nameof(ListView.Items))
                    return Fn(new ListEditSourceListViewItem());
                if (propInfo.Name == nameof(ListView.Columns))
                    return Fn(new ListEditSourceListViewColumn());
            }

            if (instance is TreeView)
            {
                if (propInfo.Name == nameof(TreeView.Items))
                    return Fn(new ListEditSourceTreeViewItem());
            }

            return null;
        }

        public virtual string? GetItemTitle(object item) => item?.ToString();

        public abstract IEnumerable? RootItems { get; }

        public virtual IEnumerable? GetChildren(object item) => null;

        public virtual object? GetProperties(object item) => item;

        public virtual ImageList? ImageList => null;

        public virtual int? GetItemImageIndex(object item) => null;
    }
}
